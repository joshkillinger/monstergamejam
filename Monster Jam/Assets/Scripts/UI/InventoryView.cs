using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class InventoryView : MonoBehaviour
{
    public InventoryManager.ItemType ItemType = InventoryManager.ItemType.PumpkinSeed;

    InventoryManager inventory;
    private uint cachedVersion;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.FindWithTag("GameController").GetComponent<InventoryManager>();
        text = GetComponent<Text>();
        text.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        if (cachedVersion != inventory.Version)
        {
            text.text = inventory.GetCount(ItemType).ToString();
        }
    }
}
