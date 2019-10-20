using System.Collections;
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

    private Dictionary<ItemType, int> counts;

    // Start is called before the first frame update
    void Start()
    {
        counts = new Dictionary<ItemType, int>()
        {
            { ItemType.PumpkinSeed, 0 },
            { ItemType.CandyCorn, 0 }
        };
    }

    public void PickUp(ItemType item)
    {
        ++counts[item];
        ++Version;
    }

    public void PlayerDamaged()
    {
        //TODO: Dump most of items.
        ++Version;
    }

    public bool UseItem(ItemType item)
    {
        if (counts.TryGetValue(item, out int count))
        {
            if (count > 0)
            {
                --counts[item];
                ++Version;
                return true;
            }
        }
        return false;
    }
}
