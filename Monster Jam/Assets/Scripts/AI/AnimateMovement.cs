using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class AnimateMovement : MonoBehaviour
{
    public string MoveTrigger = "Move";
    public string TurnTrigger = "Turn";

    public float TurnTime = .5f;
    public float MoveTime = .5f;
    public float MoveSpeed = 2;

    private Animator animator;
    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void OnDisable()
    {
        turning = false;
        moving = false;
    }

    public bool turning = false;
    public IEnumerator Turn(float targetAngle)
    {
        if (turning)
        {
            Debug.LogError("Already turning!", this);
            yield break;
        }

        turning = true;

        animator.SetTrigger(TurnTrigger);

        float startY = transform.eulerAngles.y;
        float endTime = Time.time + TurnTime;
        while (Time.time < endTime)
        {
            yield return null;
            float lastY = transform.localEulerAngles.y;
            var t = 1 - ((endTime - Time.time) / TurnTime);
            float y = Mathf.LerpAngle(startY, targetAngle, t);
            transform.Rotate(Vector3.up, y - lastY);
        }

        turning = false;
    }

    public bool moving = false;
    public IEnumerator Move()
    {
        if (moving)
        {
            Debug.LogError("Already moving!", this);
            yield break;
        }

        moving = true;

        animator.SetTrigger(MoveTrigger);

        float endTime = Time.time + MoveTime;
        while (Time.time < endTime)
        {
            yield return null;

            characterController.SimpleMove(transform.forward * MoveSpeed);
        }

        moving = false;
    }

    public IEnumerator MoveAndTurn(float targetAngle)
    {
        if (moving || turning)
        {
            Debug.LogError("Already moving or turning!", this);
            yield break;
        }

        moving = true;
        turning = true;

        animator.SetTrigger(MoveTrigger);

        float startY = transform.eulerAngles.y;
        float endTime = Time.time + MoveTime;
        Vector3 cachedForward = transform.forward;
        while (Time.time < endTime)
        {
            yield return null;

            float lastY = transform.localEulerAngles.y;
            var t = 1 - ((endTime - Time.time) / TurnTime);
            float y = Mathf.LerpAngle(startY, targetAngle, t);
            transform.Rotate(Vector3.up, y - lastY);
            characterController.SimpleMove(cachedForward * MoveSpeed);
        }

        moving = false;
        turning = false;
    }
}
