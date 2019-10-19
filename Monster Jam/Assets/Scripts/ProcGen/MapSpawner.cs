using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public Vector2 mapDimension;
    public List<GameObject> mapTiles;
    public GameObject homeBaseTile;
    public Transform mapOrigin;

    private Vector2 tileDimension;
    private Dictionary<GameObject, MapTile> tileDictionary;
    private List<GameObject> tileInstances;
    protected void Awake()
    {
        tileInstances = new List<GameObject>();
        foreach(GameObject obj in mapTiles)
        {
            MapTile tile = obj.GetComponent<MapTile>();
            if (tile != null)
            {
                tileDictionary.Add(obj, tile);
            }
        }
        tileDimension = tileDictionary[mapTiles[0]].tileDimension;
        spawnMap();
    }

    protected void spawnMap()
    {
       
        for (int i = 0; i < mapDimension.x; i++)
        {
            for(int j = 0; j < mapDimension.y; j++)
            {
                GameObject tileObj = getStraightUpRandomTile();
                tileInstances.Add(Instantiate(tileObj, new Vector3(i * tileDimension.x, 0f, j * tileDimension.y), tileObj.transform.rotation));
            }
        }
    }

    protected GameObject getStraightUpRandomTile()
    {
        int count = mapTiles.Count;
        int randomNumber = Random.Range(0, count - 1);
        return mapTiles[randomNumber];
    }


}
