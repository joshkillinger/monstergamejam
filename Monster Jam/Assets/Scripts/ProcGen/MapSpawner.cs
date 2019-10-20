using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public int biomeWeightingPasses;
    public Vector2Int mapDimension;
    public List<GameObject> biomeTiles;
    public GameObject playerPrefab;
    public GameObject cemeteryBiomeTile;
    public GameObject pumpkinBiomeTile;
    public GameObject fieldBiomeTile;
    public GameObject homeBaseTile;
    public Transform mapOrigin;
    public GameObject vFenceTile;
    public GameObject hFenceTile;
    public GameObject cornerFenceTileSW;
    public GameObject cornerFenceTileNE;
    public GameObject cornerFenceTileNW;
    public GameObject cornerFenceTileSE;
    public GameObject fenceObject;
    public GameObject candyCorn;
    public GameObject pumpkinSeed;

    private Vector2Int tileDimension;
    private List<List<GameObject>> tileInstances;
    private List<GameObject> itemInstances;
    private List<GameObject> fenceInstances;
    private GameObject playerInstance;
    protected void Awake()
    {
        tileInstances = new List<List<GameObject>>();
        itemInstances = new List<GameObject>();
        fenceInstances = new List<GameObject>();
        for(int i = 0; i < mapDimension.x; i++)
        {
            tileInstances.Add(new List<GameObject>());
        }

        tileDimension = new Vector2Int(20, 20);
        spawnMap();
    }

    protected void spawnMap()
    {
        mapSpawnInitialPass();
        for (int i = 0; i < biomeWeightingPasses; i++) {
            biomeWeightingPass();
        }
        spawnFences();
        spawnItems();
        
    }

    protected void mapSpawnInitialPass()
    {
        for (int i = 0; i < mapDimension.x; i++)
        {
            for (int j = 0; j < mapDimension.y; j++)
            {
                if (i == mapDimension.x / 2 && j == mapDimension.y / 2)
                {
                    tileInstances[i].Add(Instantiate(homeBaseTile, new Vector3(i * tileDimension.x, 0f, j * tileDimension.y), homeBaseTile.transform.rotation));
                    playerInstance = Instantiate(playerPrefab, new Vector3(i * tileDimension.x, 0f, j * tileDimension.y) + new Vector3(5f, 0, 1f), Quaternion.identity);

                }
                else if (i == 0 && j == 0)
                {
                    tileInstances[i].Add(Instantiate(cornerFenceTileSW, new Vector3(i * tileDimension.x, 0f, j * tileDimension.y), homeBaseTile.transform.rotation));
                }
                else if (i == 0 && j == mapDimension.y - 1)
                {
                    tileInstances[i].Add(Instantiate(cornerFenceTileNW, new Vector3(i * tileDimension.x, 0f, j * tileDimension.y), homeBaseTile.transform.rotation));
                }
                else if (i == mapDimension.x - 1 && j == mapDimension.y - 1)
                {
                    tileInstances[i].Add(Instantiate(cornerFenceTileNE, new Vector3(i * tileDimension.x, 0f, j * tileDimension.y), homeBaseTile.transform.rotation));
                }
                else if (i == mapDimension.x - 1 && j == 0)
                {
                    tileInstances[i].Add(Instantiate(cornerFenceTileSE, new Vector3(i * tileDimension.x, 0f, j * tileDimension.y), homeBaseTile.transform.rotation));
                }
                else if (i == mapDimension.x - 1 || i == 0)
                {
                    tileInstances[i].Add(Instantiate(vFenceTile, new Vector3(i * tileDimension.x, 0f, j * tileDimension.y), homeBaseTile.transform.rotation));
                }
                else if (j == mapDimension.y - 1 || j == 0)
                {
                    tileInstances[i].Add(Instantiate(hFenceTile, new Vector3(i * tileDimension.x, 0f, j * tileDimension.y), homeBaseTile.transform.rotation));
                }
                else
                {
                    GameObject tileObj = getStraightUpRandomTile();
                    tileInstances[i].Add(Instantiate(tileObj, new Vector3(i * tileDimension.x, 0f, j * tileDimension.y), tileObj.transform.rotation));
                }
            }
        }
    }

    protected void biomeWeightingPass()
    {
        for(int i = 0; i < mapDimension.x; i++)
        {
            for(int j = 0; j < mapDimension.y; j++)
            {
                GameObject obj = tileInstances[i][j];
                MapTile tile = obj.GetComponent<MapTile>();
                if (tile != null)
                {
                    Biome biome = tile.biome;
                    if (biome != Biome.fence)
                    {
                        List<MapTile> adjacentTiles = getAdjacentTiles(i, j);
                        float fieldProb = 0f;
                        float pumpkinProb = 0f;
                        float cemeteryProb = 0f;
                        foreach (MapTile t in adjacentTiles)
                        {
                            if (t.biome == Biome.field)
                            {
                                fieldProb += .20f;
                            }
                            else if (t.biome == Biome.pumpkin)
                            {
                                pumpkinProb += .20f;
                            }
                            else if (t.biome == Biome.cemetery)
                            {
                                cemeteryProb += .20f;
                            }
                        }
                        if (tile.biome == Biome.field)
                        {
                            fieldProb += .20f;
                        }
                        else if (tile.biome == Biome.cemetery)
                        {
                            cemeteryProb += .20f;
                        }
                        else if (tile.biome == Biome.pumpkin)
                        {
                            pumpkinProb += .20f;
                        }
                        float randomRoll = Random.Range(0, fieldProb + pumpkinProb + cemeteryProb);
                        if (randomRoll <= fieldProb)
                        {
                            swapTile(i, j, fieldBiomeTile);
                        }
                        else if (randomRoll > fieldProb && randomRoll <= cemeteryProb + fieldProb)
                        {
                            swapTile(i, j, cemeteryBiomeTile);
                        }
                        else
                        {
                            swapTile(i, j, pumpkinBiomeTile);
                        }
                    }
                }
            }
        }
    }

    protected void swapTile(int xCoord, int yCoord, GameObject tileToSwapTo)
    {
        GameObject oldTile = tileInstances[xCoord][yCoord];
        tileInstances[xCoord][yCoord] = Instantiate(tileToSwapTo, oldTile.transform.position, oldTile.transform.rotation);
        Destroy(oldTile);
    }

    protected void spawnItems()
    {
        for (int i = 0; i < mapDimension.x; i++)
        {
            for (int j = 0; j < mapDimension.y; j++)
            {
                if(Random.Range(0f, 1f) > .6f)
                {
                    Vector3 spawnPosition = tileInstances[i][j].transform.position + new Vector3(Random.Range(1f, 8f), 0f, Random.Range(1f, 8f));
                    if(Random.Range(0f, 1f) > .8f)
                    {
                        itemInstances.Add(Instantiate(candyCorn, spawnPosition, Quaternion.identity));
                    }
                    else
                    {
                        itemInstances.Add(Instantiate(pumpkinSeed, spawnPosition, Quaternion.identity));
                    }
                }
            }
        }
    }
 
    protected List<MapTile> getAdjacentTiles(int x, int y)
    {
        List<MapTile> adjacentTiles = new List<MapTile>();
        if(x > 0)
        {
            MapTile tile = tileInstances[x - 1][y].GetComponent<MapTile>();
            if(tile != null)
            {
                adjacentTiles.Add(tile);
            }
        }
        if(x < mapDimension.x - 1)
        {
            MapTile tile = tileInstances[x + 1][y].GetComponent<MapTile>();
            if(tile != null)
            {
                adjacentTiles.Add(tile);
            }
        }
        if(y < mapDimension.y - 1)
        {
            MapTile tile = tileInstances[x][y + 1].GetComponent<MapTile>();
            if(tile != null)
            {
                adjacentTiles.Add(tile);
            }
        }
        if(y > 0)
        {
            MapTile tile = tileInstances[x][y - 1].GetComponent<MapTile>();
            if(tile != null)
            {
                adjacentTiles.Add(tile);
            }
        }
        return adjacentTiles;
    }

    protected void spawnFences()
    {
        for (int i = 0; i < mapDimension.x; i++)
        {
            for (int j = 0; j < mapDimension.y; j++)
            {
                MapTile tile = tileInstances[i][j].GetComponent<MapTile>();
                if(tile != null)
                {
                    if(i > 0)
                    {
                        MapTile otherTile = tileInstances[i - 1][j].GetComponent<MapTile>();
                        if(otherTile != null)
                        {
                            if(otherTile.biome != tile.biome && otherTile.biome != Biome.fence && tile.biome != Biome.fence)
                            {
                                makeFenceBetween(tile, otherTile, true);
                            }
                        }
                    }
                    if (j > 0)
                    {
                        MapTile otherTile = tileInstances[i][j - 1].GetComponent<MapTile>();
                        if (otherTile != null)
                        {
                            if (otherTile.biome != tile.biome && otherTile.biome != Biome.fence && tile.biome != Biome.fence)
                            {
                                makeFenceBetween(tile, otherTile, false);
                            }
                        }
                    }
                }
            }
        }
    }

    protected void makeFenceBetween(MapTile tile1, MapTile tile2, bool vertical)
    {
        Vector3 tile1Pos = tile1.gameObject.transform.position;
        Vector3 tile2Pos = tile2.gameObject.transform.position;
        if(Random.Range(0f, 1f) < .8f) {
            if (vertical)
                fenceInstances.Add(Instantiate(fenceObject, (tile1Pos + tile2Pos) / 2f + new Vector3(10f, 0f, 10f) /* 👽 ayy lmao */, fenceObject.transform.rotation));
            else
                fenceInstances.Add(Instantiate(fenceObject, (tile1Pos + tile2Pos) / 2f + new Vector3(10f, 0f, 10f) /* 👽 ayy lmao */, fenceObject.transform.rotation * Quaternion.Euler(0f, 90f, 0f)));
        }
    }

    protected GameObject getStraightUpRandomTile()
    {
        int count = biomeTiles.Count;
        int randomNumber = Random.Range(0, count);
        return biomeTiles[randomNumber];
    }


}
