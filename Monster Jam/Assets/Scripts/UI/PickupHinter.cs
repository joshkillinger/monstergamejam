using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHinter : MonoBehaviour
{
	public float wiggleSpeed = 2;
	class SmellLine
	{
		public GameObject root = null;
		public LineRenderer line = null;
		public Transform target = null;
		public bool faded = false;
	}

	public float fadeDuration = 1;
	public float minDistForHint = 1000;
	public float maxDistForHint = 5000;
	public float MaxScale;

	public GameObject lineRootTemplate;
	public float length = 20;
	List<SmellLine> smellLines = new List<SmellLine>();

	List<float> scales;
	List<float> times;

	private bool initialized = false;

	// Start is called before the first frame update
	void Start()
	{
		var lineTemplate = lineRootTemplate.GetComponentInChildren<LineRenderer>();
		scales = new List<float>(lineTemplate.positionCount);
		times = new List<float>(lineTemplate.positionCount);

		for (int i = 0; i < lineTemplate.positionCount; ++i)
		{
			scales.Add(Random.value * MaxScale);
			times.Add(Random.value);
			lineTemplate.SetPosition(i, new Vector3(Mathf.Sin(times[i] + Time.time * 2) * scales[i], 0, i * length / lineTemplate.positionCount));
		}
	}

	// Update is called once per frame
	void Update()
	{
		var bestSmell = getBestSmellLine();

		foreach (var smell in smellLines)
		{
			//wiggle
			for (int i = 0; i < smell.line.positionCount; ++i)
			{
				smell.line.SetPosition(i, new Vector3(Mathf.Sin(times[i] + Time.time) * scales[i], 0, smell.line.GetPosition(i).z));
			}

			//rotation
			if (smell.target != null)
			{
				var toTarget = (smell.target.position - transform.position).normalized;
				var angle = Vector3.SignedAngle(Vector3.forward, toTarget, Vector3.up);
				smell.root.transform.rotation = Quaternion.Euler(0, angle, 0);
			}

			var color = smell.line.startColor;
			if (smell == bestSmell)
			{
				color.a = 1f;
			}
			else
			{
				color.a -= fadeDuration * Time.deltaTime;
				smell.faded = color.a <= 0;
			}
			smell.line.startColor = color;
			smell.line.endColor = color;
		}

		for (int i = smellLines.Count - 1; i >= 0; i--)
		{
			if (smellLines[i].faded)
			{
				Destroy(smellLines[i].root);
				smellLines.RemoveAt(i);
			}
		}
	}

	List<GameObject> items;

	public void SetItems(List<GameObject> items)
	{
		this.items = items;
		initialized = true;
	}

	private SmellLine getBestSmellLine()
	{
		Transform closest = null;
		float closestDist = float.MaxValue;

		foreach (var item in items)
		{
			if (item == null || item.transform == null)
			{
				continue;
			}

			var dist = (item.transform.position - transform.position).sqrMagnitude;
			if (dist > minDistForHint && dist < maxDistForHint && dist < closestDist)
			{
				closestDist = dist;
				closest = item.transform;
			}
		}

		SmellLine bestSmell = null;
		foreach (var smell in smellLines)
		{
			if (closest == smell.target)
			{
				bestSmell = smell;
				break;
			}
		}

		if (bestSmell != null)
		{
			return bestSmell;
		}
		else
		{
			var root = Instantiate(lineRootTemplate, lineRootTemplate.transform.parent);
			root.gameObject.SetActive(true);
			var newSmell = new SmellLine()
			{
				root = root,
				line = root.GetComponentInChildren<LineRenderer>(),
				target = closest
			};
			smellLines.Add(newSmell);
			return newSmell;
		}
	}
}
