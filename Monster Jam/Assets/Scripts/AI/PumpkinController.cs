using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AnimateMovement))]
public abstract class PumpkinController : MonoBehaviour
{
    AnimateMovement animate; 

    public float MaxTurnAngle = 90;

    public float AlertRange = 100;
    public float IdleUpdateRate = 2;
    public float PursuitRange = 80;
    public float AlertUpdateRate = 1;

    protected Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animate = GetComponent<AnimateMovement>();
        nextState = idle();
        StartCoroutine(aiLoop());
    }

    protected float sqrDistToPlayer => Vector3.SqrMagnitude(player.position - transform.position);

    private IEnumerator aiLoop()
    {
        while (true)
        {
            yield return nextState;
            Debug.Log("Running next state");
        }
    }

    protected IEnumerator nextState;

    protected virtual IEnumerator idle()
    {
        yield return new WaitForSeconds(IdleUpdateRate);
    }

    protected virtual IEnumerator alert()
    {
        yield return new WaitForSeconds(AlertUpdateRate);
    }

    protected virtual IEnumerator pursuit() { yield return null; }

    protected virtual IEnumerator attack() { yield return null; }

    protected virtual IEnumerator turn(float targetAngle)
    {
        Debug.Log("turn");
        yield return animate.Turn(targetAngle);
    }

    protected virtual IEnumerator move()
    {
        Debug.Log("move");
        yield return animate.Move();
    }
}
