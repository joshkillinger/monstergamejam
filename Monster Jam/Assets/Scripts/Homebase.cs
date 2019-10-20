using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homebase : MonoBehaviour
{
	string playerTag = "Player";
	[SerializeField]
	float scoreDelay = 1f;
	float timeToScore;
	[SerializeField]
	float staminaReplenishRate = 1f;

	InventoryManager inventory;
	ScoreManager score;
	StaminaManager stamina;

	void Start()
	{
		var gameController = GameObject.FindWithTag("GameController");
		inventory = gameController.GetComponentInChildren<InventoryManager>();
		score = gameController.GetComponentInChildren<ScoreManager>();
		stamina = gameController.GetComponentInChildren<StaminaManager>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(playerTag))
		{
			timeToScore = scoreDelay;
		}
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.CompareTag(playerTag))
		{
			timeToScore -= Time.deltaTime;

			if (timeToScore <= 0)
			{
				timeToScore = scoreDelay;

			}

			//var controller = GameObject.FindGameObjectWithTag("GameController");
			//controller.GetComponent<InventoryManager>().PickUp(this);

			//GetComponent<Animator>().SetTrigger(PickupTrigger);
		}
	}
}
