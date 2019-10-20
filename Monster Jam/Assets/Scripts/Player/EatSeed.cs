﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatSeed : MonoBehaviour
{
	InventoryManager inventory = null;
	[SerializeField]
	Stamina stamina = null;
	[SerializeField]
	float staminaReplenish = 0.3f;
	bool eating = false;

	private void Start()
	{
		inventory = GameObject.FindWithTag("GameController").GetComponent<InventoryManager>();
	}

	private void Update()
	{
		bool inputEat = Input.GetAxis("Consume") > 0;
		bool shouldEat = inputEat && AttemptEat();
		if (shouldEat)
		{
			eating = true;
			if (stamina != null)
			{
				stamina.Replenish(staminaReplenish);
			}
		}

		if (!inputEat)
		{
			eating = false;
		}
	}

	private bool AttemptEat()
	{
		return !eating && (stamina == null || !stamina.Full) && inventory.UseItem(InventoryManager.ItemType.PumpkinSeed);
	}
}
