using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
	InventoryManager inventory = null;

	private void Start()
	{
		inventory = GameObject.FindWithTag("GameController").GetComponentInChildren<InventoryManager>();
	}

	public void Damage()
	{
		if (!inventory.PlayerDamaged())
		{
			Die();
		}
	}

	public void Die()
	{
		Destroy(gameObject);
	}
}
