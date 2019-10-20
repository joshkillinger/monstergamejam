using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
public class InventoryItem : MonoBehaviour
{
    public InventoryManager.ItemType ItemType = InventoryManager.ItemType.PumpkinSeed;
    public string PickupTrigger = "PickUp";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = GameObject.FindGameObjectWithTag("GameController");
            controller.GetComponent<InventoryManager>().PickUp(ItemType);

            GetComponent<Animator>().SetTrigger(PickupTrigger);
        }
    }

    public void Event_Destroy()
    {
        Destroy(gameObject);
    }
}