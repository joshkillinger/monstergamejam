using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;

    private List<GameObject> enemyInstances;
    private ItemTurnerOnAndOffer itemTurnerOnAndOffer;

    public void init(ItemTurnerOnAndOffer jeff)
    {
        enemyInstances = new List<GameObject>();
        itemTurnerOnAndOffer = jeff;
    }

    public void spawnEnemy()
    {
        if(Random.Range(0f, 1f) > .5f)
        {
            GameObject obj = Instantiate(enemyPrefab1, transform.position, Quaternion.identity);
            enemyInstances.Add(obj);
            itemTurnerOnAndOffer.addEnemyInstance(obj);
        }
        else
        {
            GameObject obj = Instantiate(enemyPrefab2, transform.position, Quaternion.identity);
            enemyInstances.Add(obj);
            itemTurnerOnAndOffer.addEnemyInstance(obj);
        }
    }
}