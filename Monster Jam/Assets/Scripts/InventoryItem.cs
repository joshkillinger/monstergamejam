using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
public class InventoryItem : MonoBehaviour
{
    public InventoryManager.ItemType ItemType = InventoryManager.ItemType.PumpkinSeed;
    public string PickupTrigger = "PickUp";
    
	public Collider PickupCollider = null;

	public int Points = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var controller = GameObject.FindGameObjectWithTag("GameController");
            controller.GetComponent<InventoryManager>().PickUp(this);

            GetComponent<Animator>().SetTrigger(PickupTrigger);
        }
    }

	public void Event_Show()
	{
		gameObject.SetActive(true);
	}

	public void Event_Hide()
	{
		gameObject.SetActive(false);
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
        foreach(var particle in GetComponentsInChildren<ParticleSystem>())
        {
            if (!particle.gameObject.activeInHierarchy)
            {
                continue;
            }
            particle.transform.SetParent(null);
            var runner = particle.gameObject.AddComponent<RunAnyCoroutine>();
            runner.Run(destroyParticleAfterDelay(3, particle));
        }
        Destroy(gameObject);
    }

    private IEnumerator destroyParticleAfterDelay(float delay, ParticleSystem particle)
    {
        particle.Stop();
        yield return new WaitForSeconds(delay);
        Destroy(particle.gameObject);
    }
}