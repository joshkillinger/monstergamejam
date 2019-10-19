using System.Collections;
using UnityEngine;

public class FastPumpkin : PumpkinController
{
    public float outOfRangeRetries = 2;

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
                targetAngle += (float)((Random.value - .5) * (MaxTurnAngle * .5));
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

        while (outOfRangeTries < outOfRangeRetries)
        {
            yield return base.alert();

            var dist = sqrDistToPlayer;
            if (dist < PursuitRange)
            {
                nextState = pursuit();
                break;
            }
            else if (dist > AlertRange)
            {
                ++outOfRangeTries;
            }
            else
            {
                outOfRangeTries = 0;
            }
        }
    }
}
