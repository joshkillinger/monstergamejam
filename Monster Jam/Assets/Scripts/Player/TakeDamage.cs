using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
	InventoryManager inventory = null;

	[SerializeField]
	int invincibilityFrames = 30;
	int invincibilityFramesRemaining = 30;

	private void Start()
	{
		inventory = GameObject.FindWithTag("GameController").GetComponentInChildren<InventoryManager>();
	}

	private void Update()
	{
		if (invincibilityFramesRemaining > 0)
		{
			invincibilityFramesRemaining--;
		}
	}

	public void Damage()
	{
		if (invincibilityFramesRemaining <= 0)
		{
			if (!inventory.PlayerDamaged())
			{
				Die();
			}
			else
			{
				invincibilityFramesRemaining = invincibilityFrames;
			}
		}
	}

	public void Die()
	{
		Destroy(gameObject);
	}
}
