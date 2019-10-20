using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AnimateMovement))]
public class PumpkinController : MonoBehaviour
{
    AnimateMovement animate; 

    public float MaxTurnAngle = 90;

    public float AlertRange = 100;
    public float IdleUpdateRate = 2;
    public float PursuitRange = 60;
    public float AlertUpdateRate = 1;

    public float OutOfRangeRetries = 2;
    public float MoveDelay = .3f;

    public Color AlertGizmoColor = Color.magenta;
    public Color PursuitGizmoColor = Color.magenta;

    private Transform player;

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

    private float sqrDistToPlayer => Vector3.SqrMagnitude(player.position - transform.position);

    private IEnumerator aiLoop()
    {
        while (true)
        {
            yield return nextState;
        }
    }

    private IEnumerator nextState;

    private IEnumerator idle()
    {
        yield return new WaitForSeconds(IdleUpdateRate);

        if (sqrDistToPlayer < AlertRange)
        {
            nextState = alert();
        }
        else
        {
            float rand = Random.value;
            //wander forward
            if (rand < .2)
            {
                yield return move();
            }
            //wander turn
            else if (rand < .5)
            {
                var targetAngle = transform.eulerAngles.y;
                targetAngle += (float)((Random.value - .5) * MaxTurnAngle);
                yield return turn(targetAngle);
            }
            //else stay

            nextState = idle();
        }
    }

    private IEnumerator alert()
    {
        int outOfRangeTries = 0;

        while (outOfRangeTries < OutOfRangeRetries)
        {
            yield return new WaitForSeconds(AlertUpdateRate);

            var dist = sqrDistToPlayer;
            if (dist < PursuitRange)
            {
                nextState = pursuit();
                yield break;
            }
            else
            {
                //look toward player
                var angle = angleToPlayer;
                var y = transform.eulerAngles.y;

                if (Mathf.Abs(y - angle) > .1)
                {
                    yield return turn(Mathf.Clamp(angle, y - MaxTurnAngle, y + MaxTurnAngle));
                }

                if (dist > AlertRange)
                {
                    ++outOfRangeTries;
                }
                else
                {
                    outOfRangeTries = 0;
                }
            }
        }

        nextState = idle();
    }

    private IEnumerator pursuit()
    {
        float dist = sqrDistToPlayer;
        do
        {
            var vecToPlayer = (player.position - transform.position).normalized;
            var y = transform.eulerAngles.y;
            var angle = Mathf.Clamp(angleToPlayer, y - MaxTurnAngle, y + MaxTurnAngle);

            if (Vector3.Dot(vecToPlayer, transform.forward) < 0)
            {
                //don't jump away from player, just turn
                yield return turn(angle);
            }
            else
            {
                yield return moveAndTurn(angle);
            }
            yield return new WaitForSeconds(MoveDelay);

            dist = sqrDistToPlayer;
        } while (dist < PursuitRange);

        nextState = alert();
    }

    private IEnumerator attack() { yield return null; }

    private IEnumerator turn(float targetAngle)
    {
        yield return animate.Turn(targetAngle);
    }

    private IEnumerator move()
    {
        yield return animate.Move();
    }

    private IEnumerator moveAndTurn(float targetAngle)
    {
        yield return animate.MoveAndTurn(targetAngle);
    }

    private float angleToPlayer
    {
        get
        {
            var toPlayer = (player.position - transform.position).normalized;
            return Vector3.SignedAngle(Vector3.forward, toPlayer, Vector3.up);
        }
    }

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			collision.collider.GetComponent<TakeDamage>().Damage();
		}
	}
}
