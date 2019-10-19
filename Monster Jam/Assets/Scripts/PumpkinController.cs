using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public abstract class PumpkinController : MonoBehaviour
{
    CharacterController characterController;

    public float Speed = 2;

    public float AlertRange = 100;
    public float IdleUpdateRate = 2;
    public float PursuitRange = 80;
    public float AlertUpdateRate = 1;

    protected Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        characterController = GetComponent<CharacterController>();
        nextState = idle();
        StartCoroutine(aiLoop());
    }

    private IEnumerator aiLoop()
    {
        while (true)
        {
            yield return nextState;
        }
    }

    protected IEnumerator nextState;

    protected IEnumerator idle()
    {
        yield return new WaitForSeconds(IdleUpdateRate);

        if (Vector3.SqrMagnitude(player.position - transform.position) < AlertRange)
        {
            nextState = alert();
        }
    }

    protected abstract IEnumerator alert();

    protected abstract IEnumerator pursuit();

    protected abstract IEnumerator attack();

    protected abstract IEnumerator turn();

    protected abstract IEnumerator move();
}
