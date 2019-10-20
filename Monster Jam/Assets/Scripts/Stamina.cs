using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : MonoBehaviour
{
	private float total = 1f;
	public float Total => total;

	public bool Consume(float amount)
	{
		if (total >= amount)
		{
			alterStamina(-amount);
			return true;
		}
		return false;
	}

	public void Repenlish(float amount)
	{
		alterStamina(amount);
	}

	private void alterStamina(float amount)
	{
		total = Mathf.Clamp01(total + amount);
	}
}
