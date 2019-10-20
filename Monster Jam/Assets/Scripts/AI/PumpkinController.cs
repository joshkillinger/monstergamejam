using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AnimateMovement))]
public abstract class PumpkinController : MonoBehaviour
{
    AnimateMovement animate; 

    public float MaxTurnAngle = 90;

    public float AlertRange = 100;
    public float IdleUpdateRate = 2;
    public float PursuitRange = 60;
    public float AlertUpdateRate = 1;

    public Color AlertGizmoColor = Color.magenta;
    public Color PursuitGizmoColor = Color.magenta;

    protected Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        animate = GetComponent<AnimateMovement>();
        nextState = idle();
        StartCoroutine(aiLoop());
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = AlertGizmoColor;
        Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(AlertRange));

        Gizmos.color = PursuitGizmoColor;
        Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(PursuitRange));
    }
#endif

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

    protected virtual IEnumerator moveAndTurn(float targetAngle)
    {
        Debug.Log("move and turn");
        yield return animate.MoveAndTurn(targetAngle);
    }

    protected float angleToPlayer
    {
        get
        {
            var toPlayer = (player.position - transform.position).normalized;
            return Vector3.SignedAngle(Vector3.forward, toPlayer, Vector3.up);
        }
    }
}
