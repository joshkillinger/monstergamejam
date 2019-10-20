using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Stun : MonoBehaviour
{
	List<IPreventable> preventOnStun = null;
	Coroutine stunRoutine;

	private void Start()
	{
		var behaviors = GetComponentsInChildren<MonoBehaviour>();
		preventOnStun = new List<IPreventable>();
		foreach(var behavior in behaviors)
		{
			if (behavior is IPreventable)
			{
				preventOnStun.Add((IPreventable)behavior);
			}
		}
	}

	public void StartStun(float duration)
	{
		if (isActiveAndEnabled)
		{
			if (stunRoutine != null)
			{
				StopCoroutine(stunRoutine);
			}
			stunRoutine = StartCoroutine(stunAndWait(duration));

			var childStuns = GetComponentsInChildren<Stun>();
			foreach (var child in childStuns)
			{
				if (child != this)
				{
					child.StartStun(duration);
				}
			}
		}
	}

	public void EndStun()
	{
		foreach (var preventable in preventOnStun)
		{
			preventable.StopPrevent();
		}

		var childStuns = GetComponentsInChildren<Stun>();
		foreach (var child in childStuns)
		{
			if (child != this)
			{
				child.EndStun();
			}
		}
	}

	private IEnumerator stunAndWait(float duration)
	{
		foreach (var preventable in preventOnStun)
		{
			preventable.StartPrevent();
		}
		yield return new WaitForSeconds(duration);
		EndStun();
		stunRoutine = null;
	}
}
