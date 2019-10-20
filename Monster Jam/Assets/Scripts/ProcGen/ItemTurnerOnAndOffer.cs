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
    GameObject playerInstance;
    Transform playerTransform;



    public ItemTurnerOnAndOffer(List<List<GameObject>> tiles, List<GameObject> items, List<GameObject> fences, GameObject player)
    {
        tileInstances = tiles;
        itemInstances = items;
        fenceInstances = fences;
    }

    protected void Awake()
    {
        squareDistanceToTurnOff = distanceToTurnOff * distanceToTurnOff;
        playerTransform = playerInstance.transform;
    }

    protected void Update()
    {
        
    }

    protected void decideToTurnOffStuff()
    {
        foreach(List<GameObject> obList in tileInstances)
        {
            foreach(GameObject obj in obList)
            {
                if (obj.activeSelf)
                {
                    if(Vector3.SqrMagnitude(playerTransform.position - obj.transform.position) > distanceToTurnOff)
                    {
                        obj.SetActive(false);
                    }
                }
                else
                {
                    if (Vector3.SqrMagnitude(playerTransform.position - obj.transform.position) < distanceToTurnOff)
                    {
                        obj.SetActive(true);
                    }
                }
            }
        }

        foreach (GameObject obj in fenceInstances)
        {
            if (obj.activeSelf)
            {
                if (Vector3.SqrMagnitude(playerTransform.position - obj.transform.position) > distanceToTurnOff)
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                if (Vector3.SqrMagnitude(playerTransform.position - obj.transform.position) < distanceToTurnOff)
                {
                    obj.SetActive(true);
                }
            }
        }
        foreach (GameObject obj in itemInstances)
        {
            if (obj.activeSelf)
            {
                if (Vector3.SqrMagnitude(playerTransform.position - obj.transform.position) > distanceToTurnOff)
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                if (Vector3.SqrMagnitude(playerTransform.position - obj.transform.position) < distanceToTurnOff)
                {
                    obj.SetActive(true);
                }
            }
        }
    }

    
}
