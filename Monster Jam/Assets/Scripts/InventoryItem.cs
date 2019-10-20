using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
public class InventoryItem : MonoBehaviour
{
    public InventoryManager.ItemType ItemType = InventoryManager.ItemType.PumpkinSeed;
    public string PickupTrigger = "PickUp";
    
	public Collider PickupCollider = null;

	[SerializeField]
	int points = 1;
	public int Points => points;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = GameObject.FindGameObjectWithTag("GameController");
            controller.GetComponent<InventoryManager>().PickUp(this);

            GetComponent<Animator>().SetTrigger(PickupTrigger);
        }
    }

	public void Event_ToggleVisibility(bool visibility)
	{
		gameObject.SetActive(visibility);
	}

	public void Event_DestroyOnDelay(float delay)
	{
		StartCoroutine(delayDestroy(delay));
	}

	private IEnumerator delayDestroy(float delay)
	{
		yield return new WaitForSeconds(delay);
		Event_Destroy();
	}

	public void Event_Destroy()
    {
        Destroy(gameObject);
    }
}