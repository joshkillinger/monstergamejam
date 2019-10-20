using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public enum ItemType
    {
        PumpkinSeed,
        CandyCorn
    }

	public ulong Version = 0;

    private Dictionary<ItemType, List<InventoryItem>> items;

	[SerializeField]
	Transform itemContainer = null;
	[SerializeField]
	InventoryEjector ejector = null;
	Transform player = null;

	// Start is called before the first frame update
	void Start()
    {
        items = new Dictionary<ItemType, List<InventoryItem>>()
        {
            { ItemType.PumpkinSeed, new List<InventoryItem>() },
            { ItemType.CandyCorn, new List<InventoryItem>() }
        };

		if (itemContainer = null)
		{
			itemContainer = transform;
		}

		player = GameObject.FindWithTag("Player").transform;
	}



	public void PickUp(InventoryItem item)
	{
		++Version;

		item.transform.parent = itemContainer;
		items[item.ItemType].Add(item);

		if (ejector != null)
		{
			ejector.RemoveUnwantedItems(item);
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
				if (ejector != null)
				{
					ejector.EjectFromTransform(itemCategory.Value[0], player, true);
				}
				else
				{
					itemCategory.Value[0].Event_Destroy();
				}
				itemCategory.Value.RemoveAt(0);
			}
		}

		return true;
	}

	public bool UseItem(ItemType item)
    {
        if (items.TryGetValue(item, out List<InventoryItem> typedItems))
        {
            if (typedItems.Count > 0)
            {
				var removedItem = typedItems[0];
				removedItem.Event_Destroy();
				typedItems.RemoveAt(0);
                ++Version;
                return true;
            }
        }
        return false;
    }

	public InventoryItem RemoveRandomItem()
	{
		List<List<InventoryItem>> validCategories = new List<List<InventoryItem>>();
		foreach(var category in items)
		{
			if (category.Value.Count > 0)
			{
				validCategories.Add(category.Value);
			}
		}

		if (validCategories.Count > 0)
		{
			var category = validCategories[Random.Range(0, validCategories.Count)];
			var item = category[Random.Range(0, category.Count)];
			category.Remove(item);
			return item;
		}

		return null;
	}
}
