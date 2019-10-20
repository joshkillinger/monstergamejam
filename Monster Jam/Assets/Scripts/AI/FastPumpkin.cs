using System.Collections;
using UnityEngine;

public class FastPumpkin : PumpkinController
{
    public float OutOfRangeRetries = 2;
    public float MoveDelay = .3f;

    protected override IEnumerator idle()
    {
        Debug.Log("idle");
        yield return base.idle();

        Debug.Log($"SqrDist to player = {sqrDistToPlayer}");
        if (sqrDistToPlayer < AlertRange)
        {
            nextState = alert();
        }
        else
        {
            float rand = Random.value;
            Debug.Log($"Random = {rand}");
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

    protected override IEnumerator alert()
    {
        Debug.Log("alert");
        int outOfRangeTries = 0;

        while (outOfRangeTries < OutOfRangeRetries)
        {
            yield return base.alert();

            Debug.Log($"SqrDist to player = {sqrDistToPlayer}");

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

    protected override IEnumerator pursuit()
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
}
