using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class Boost : MonoBehaviour
{
	private PlayerMover mover = null;
	private InventoryManager inventory = null;
	private StaminaManager stamina = null;
	[SerializeField]
	private Sprint sprint = null;

	private bool boosting = false;
    public bool Boosting => boosting;

	[SerializeField]
	private MoveStats boostStats = null;
	[SerializeField]
	private float boostDuration = 5f;
	private float boostRemaining = 0f;
	[SerializeField]
	private float boostCooldown = 1f;
	private float boostCooldownRemaining = 0f;

	private void Start()
	{
		mover = GetComponent<PlayerMover>();
		var gameController = GameObject.FindWithTag("GameController");
		inventory = gameController.GetComponentInChildren<InventoryManager>();
		stamina = gameController.GetComponentInChildren<StaminaManager>();
	}

	private void Update()
	{
		UpdateTimers();

		bool shouldBoost = Input.GetAxis("Boost") > 0 && AttemptBoost();

		if (shouldBoost)
		{
			boosting = true;
			if (sprint != null)
			{
				sprint.PreventSprinting = true;
			}
			mover.SetMoveStats(boostStats);
			boostRemaining = boostDuration;
			if (stamina != null)
			{
				stamina.Replenish(1);
			}
		}
	}

	private bool AttemptBoost()
	{
		return !boosting && boostRemaining <= 0 && boostCooldownRemaining <= 0 && inventory.UseItem(InventoryManager.ItemType.CandyCorn);
	}

	private void UpdateTimers()
	{
		if (boostCooldownRemaining > 0)
		{
			boostCooldownRemaining = Mathf.Max(boostCooldownRemaining - Time.deltaTime, 0);
		}

		if (boostRemaining > 0)
		{
			boostRemaining = Mathf.Max(boostRemaining - Time.deltaTime, 0);
		}
		else if (boosting)
		{
			EndBoost();
		}
	}

	private void EndBoost()
	{
		if (boosting)
		{
			boosting = false;
			if (sprint != null)
			{
				sprint.PreventSprinting = false;
			}
			boostCooldownRemaining = boostCooldown;
			mover.ResetMoveStats();
		}
	}
}
