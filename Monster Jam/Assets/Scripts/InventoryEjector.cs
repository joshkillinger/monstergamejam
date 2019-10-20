using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryEjector : MonoBehaviour
{
	class ItemEjection
	{
		public InventoryItem item;
		public Vector3 ejection;
		public int collectDelayRemaining;
	}

	[Header("Item Ejection")]
	[SerializeField]
	int ejectCollectFrameDelay = 10;
	[SerializeField]
	float ejectDestroyDelay = 5f;
	[SerializeField]
	float ejectMinSpeed = 5f;
	[SerializeField]
	float ejectMaxSpeed = 10f;
	[SerializeField]
	float ejectDrag = 0.95f;
	List<ItemEjection> ejections;

	private void Start()
	{
		ejections = new List<ItemEjection>();

	}

	private void Update()
	{
		RemoveUnwantedItems();

		for (int i = 0; i < ejections.Count; i++)
		{
			var ejection = ejections[i];
			ejection.item.transform.position += ejection.ejection * Time.deltaTime;
			ejection.collectDelayRemaining--;
			if (ejection.collectDelayRemaining <= 0)
			{
				ejection.collectDelayRemaining = 0;
				ejection.item.PickupCollider.enabled = true;
			}
			ejection.ejection *= ejectDrag;
		}
	}

	public void RemoveUnwantedItems(InventoryItem unwantedItem = null)
	{
		for (int i = 0; i < ejections.Count; i++)
		{
			if (ejections[i]?.item == null || ejections[i].item == unwantedItem)
			{
				ejections.RemoveAt(i);
				i--;
			}
		}
	}

	public void EjectFromTransform(InventoryItem item, Transform source, bool destroyAfter)
	{
		var direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
		var speed = Random.Range(ejectMinSpeed, ejectMaxSpeed);
		ejections.Add(new ItemEjection()
		{
			item = item,
			ejection = direction * speed,
			collectDelayRemaining = ejectCollectFrameDelay
		});

		item.transform.position = source.position;
		item.Event_Show();
		item.PickupCollider.enabled = false;

		if (destroyAfter)
		{
			item.Event_DestroyOnDelay(ejectDestroyDelay);
		}
	}
}
