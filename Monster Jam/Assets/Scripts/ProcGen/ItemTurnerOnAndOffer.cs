using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTurnerOnAndOffer : MonoBehaviour
{

    public float distanceToTurnOff;
    private float squareDistanceToTurnOff;


    List<List<GameObject>> tileInstances;
    List<GameObject> itemInstances;
    List<GameObject> fenceInstances;
    List<GameObject> enemyInstances;
    GameObject playerInstance;
    Transform playerTransform;

	InventoryManager inventory;


    public void init(List<List<GameObject>> tiles, List<GameObject> items, List<GameObject> fences, GameObject player)
    {
        tileInstances = tiles;
        itemInstances = items;
        fenceInstances = fences;
        playerInstance = player;
        enemyInstances = new List<GameObject>();
        distanceToTurnOff = 80;
    }

    protected void Start()
    {
        squareDistanceToTurnOff = distanceToTurnOff * distanceToTurnOff;
        playerTransform = playerInstance.transform;

		inventory = GameObject.FindWithTag("GameController").GetComponentInChildren<InventoryManager>();
    }

    protected void Update()
    {
        removeNulls();
        decideToTurnOffStuff();
    }

    public void addEnemyInstance(GameObject enemyInstance)
    {
        enemyInstances.Add(enemyInstance);
    }

    protected void decideToTurnOffStuff()
    {
        foreach(List<GameObject> obList in tileInstances)
        {
            foreach(GameObject obj in obList)
            {
                turnOnOrOffObject(obj);
            }
        }

        foreach (GameObject obj in fenceInstances)
        {
            turnOnOrOffObject(obj);
        }
        foreach (GameObject obj in itemInstances)
        {
			if (!inventory.IsHoldingItem(obj))
			{
				turnOnOrOffObject(obj);
			}
        }
        foreach (GameObject obj in enemyInstances)
        {
            turnOnOrOffObject(obj);
        }

    }

    protected void removeNulls()
    {
        for (int i = tileInstances.Count - 1; i >= 0; i--)
        {
            for(int j = tileInstances[i].Count - 1; j >= 0; j--)
            {
                if(tileInstances[i][j] == null)
                {
                    tileInstances[i].RemoveAt(j);
                }
            }
        }

        for (int i = itemInstances.Count - 1; i >= 0; i--)
        {
            if(itemInstances[i] == null)
            {
                itemInstances.RemoveAt(i);
            }
        }

        for(int i = enemyInstances.Count - 1; i >= 0; i--)
        {
            if(enemyInstances[i] == null)
            {
                enemyInstances.RemoveAt(i);
            }
        }

        for(int i = fenceInstances.Count - 1; i >= 0; i--)
        {
            if(fenceInstances[i] == null)
            {
                fenceInstances.RemoveAt(i);
            }
        }
    }

    protected void turnOnOrOffObject(GameObject obj)
    {
        if (playerTransform != null)
        {
            if (obj.activeSelf)
            {
                if (Vector3.SqrMagnitude(playerTransform.position - obj.transform.position) > squareDistanceToTurnOff)
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                if (Vector3.SqrMagnitude(playerTransform.position - obj.transform.position) < squareDistanceToTurnOff)
                {
                    obj.SetActive(true);
                }
            }
        }
    }

    
}
