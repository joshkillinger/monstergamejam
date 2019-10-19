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

    public bool turning = false;
    public IEnumerator Turn(float targetAngle)
    {
        if (turning)
        {
            Debug.LogError("Already turning!", this);
            yield break;
        }

        Debug.Log($"Turning to {targetAngle}");
        turning = true;

        animator.SetTrigger(TurnTrigger);

        float startY = transform.eulerAngles.y;
        float endTime = Time.time + TurnTime;
        while (Time.time < endTime)
        {
            yield return null;
            var t = Mathf.Clamp01((endTime - Time.time) / TurnTime);
            float y = Mathf.LerpAngle(startY, targetAngle, t);
            transform.eulerAngles.Set(transform.eulerAngles.x, y, transform.eulerAngles.z);
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

        Debug.Log("Moving");
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
}
