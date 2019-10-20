using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHinter : MonoBehaviour
{
    public float minDistForHint = 1000;
    public float maxDistForHint = 5000;
    public float MaxScale;

    public LineRenderer line;
    public float length = 20;

    List<float> scales;
    List<float> times;

    // Start is called before the first frame update
    void Start()
    {
        scales = new List<float>(line.positionCount);
        times = new List<float>(line.positionCount);

        for (int i = 0; i < line.positionCount; ++i)
        {
            scales.Add(Random.value * MaxScale);
            times.Add(Random.value);
            line.SetPosition(i, new Vector3(Mathf.Sin(times[i] + Time.time) * scales[i], 0, i * length / line.positionCount));
        }
    }

    // Update is called once per frame
    void Update()
    {
        //wiggle
        for (int i = 0; i < line.positionCount; ++i)
        {
            line.SetPosition(i, new Vector3(Mathf.Sin(times[i] + Time.time) * scales[i], 0, line.GetPosition(i).z));
        }

        //rotation
        var target = getTarget();
        if (target == null)
        {
            line.gameObject.SetActive(false);
        }
        else
        {
            line.gameObject.SetActive(true);
            var toTarget = (target.position - transform.position).normalized;
            var angle = Vector3.SignedAngle(Vector3.forward, toTarget, Vector3.up);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }

    List<GameObject> items;

    public void SetItems(List<GameObject> items)
    {
        this.items = items;
    }

    private Transform getTarget()
    {
        Transform closest = null;
        float closestDist = float.MaxValue;

        foreach (var item in items)
        {
            var dist = (item.transform.position - transform.position).sqrMagnitude;
            if (dist > minDistForHint && dist < maxDistForHint && dist < closestDist)
            {
                closestDist = dist;
                closest = item.transform;
            }
        }

        return closest;
    }
}
