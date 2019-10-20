using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public enum ItemType
    {
        PumpkinSeed,
        CandyCorn
    }

	class ItemEjection
	{
		public InventoryItem item;
		public Vector3 ejection;
		public int collectDelayRemaining;
	}

	public ulong Version = 0;

    private Dictionary<ItemType, List<InventoryItem>> items;

	[SerializeField]
	Transform itemContainer = null;
	Transform player = null;

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

	// Start is called before the first frame update
	void Start()
    {
        items = new Dictionary<ItemType, List<InventoryItem>>()
        {
            { ItemType.PumpkinSeed, new List<InventoryItem>() },
            { ItemType.CandyCorn, new List<InventoryItem>() }
        };

		ejections = new List<ItemEjection>();

		if (itemContainer = null)
		{
			itemContainer = transform;
		}

		player = GameObject.FindWithTag("Player").transform;
	}

	private void Update()
	{
		for(int i = 0; i < ejections.Count; i++)
		{
			if (ejections[i].item != null)
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
			else
			{
				ejections.RemoveAt(i);
				i--;
			}
		}
	}

	public void PickUp(InventoryItem item)
	{
		++Version;

		item.Event_ToggleVisibility(false);
		item.transform.parent = itemContainer;
		items[item.ItemType].Add(item);

		for (int i = 0; i < ejections.Count; i++)
		{
			if (ejections[i].item)
			{
				ejections.RemoveAt(i);
				i--;
			}
		}
	}

    public int GetCount(ItemType type)
    {
        return items[type].Count;
    }

    public bool PlayerDamaged()
    {
		bool anyItems = false;
		foreach(var itemCategory in items)
		{
			if (itemCategory.Value.Count > 0)
			{
				anyItems = true;
			}
		}
		if (!anyItems)
		{
			return false;
		}

		++Version;

		foreach(var itemCategory in items)
		{
			while(itemCategory.Value.Count > 0)
			{
				EjectFromTransform(itemCategory.Value[0], player, true);
			}
		}

		return true;
	}

	public void EjectFromTransform(InventoryItem item, Transform source, bool destroyAfter)
	{
		items[item.ItemType].Remove(item);

		var direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
		var speed = Random.Range(ejectMinSpeed, ejectMaxSpeed);
		ejections.Add(new ItemEjection()
		{
			item = item,
			ejection = direction * speed,
			collectDelayRemaining = ejectCollectFrameDelay
		});

		item.transform.position = source.position;
		item.Event_ToggleVisibility(true);
		item.PickupCollider.enabled = false;

		if (destroyAfter)
		{
			item.Event_DestroyOnDelay(ejectDestroyDelay);
		}
	}

	public bool UseItem(ItemType item)
    {
        if (items.TryGetValue(item, out List<InventoryItem> typedItems))
        {
            if (typedItems.Count > 0)
            {
				var removedItem = typedItems[typedItems.Count - 1];
				removedItem.Event_Destroy();
				typedItems.RemoveAt(typedItems.Count - 1);
                ++Version;
                return true;
            }
        }
        return false;
    }
}
