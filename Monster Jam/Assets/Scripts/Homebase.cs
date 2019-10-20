using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homebase : MonoBehaviour
{
	string playerTag = "Player";

	private void OnTriggerEnter(Collider other)
	{
		Debug.Log("HIT");
		if (other.CompareTag(playerTag))
		{
			Debug.Log("Player");
			//var controller = GameObject.FindGameObjectWithTag("GameController");
			//controller.GetComponent<InventoryManager>().PickUp(this);

			//GetComponent<Animator>().SetTrigger(PickupTrigger);
		}
	}

	private void OnTriggerStay(Collider other)
	{
		Debug.Log("STAY");
		if (other.CompareTag(playerTag))
		{
			Debug.Log("Player");
			//var controller = GameObject.FindGameObjectWithTag("GameController");
			//controller.GetComponent<InventoryManager>().PickUp(this);

			//GetComponent<Animator>().SetTrigger(PickupTrigger);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Debug.Log("EXIT");
		if (other.CompareTag(playerTag))
		{
			Debug.Log("Player");
			//var controller = GameObject.FindGameObjectWithTag("GameController");
			//controller.GetComponent<InventoryManager>().PickUp(this);

			//GetComponent<Animator>().SetTrigger(PickupTrigger);
		}
	}
}
