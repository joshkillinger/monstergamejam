using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
	private float total = 1f;
	public float Total => total;

	public bool Full => total >= 1f;
	public bool Empty => total <= 0f;

	public bool Consume(float amount)
	{
		if (total >= amount)
		{
			alterStamina(-amount);
			return true;
		}
		return false;
	}

	public bool Replenish(float amount)
	{
		if (total < 1)
		{
			alterStamina(amount);
			return true;
		}
		return false;
	}

	private void alterStamina(float amount)
	{
		total = Mathf.Clamp01(total + amount);
	}
}
