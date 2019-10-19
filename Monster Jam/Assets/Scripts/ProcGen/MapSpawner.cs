using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public int biomeWeightingPasses;
    public Vector2Int mapDimension;
    public List<GameObject> biomeTiles;
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

    private Vector2Int tileDimension;
    private List<List<GameObject>> tileInstances;
    protected void Awake()
    {
        tileInstances = new List<List<GameObject>>();
        for(int i = 0; i < mapDimension.x; i++)
        {
            tileInstances.Add(new List<GameObject>());
        }

        tileDimension = new Vector2Int(10, 10);
        spawnMap();
    }

    protected void spawnMap()
    {
        mapSpawnInitialPass();
        for (int i = 0; i < biomeWeightingPasses; i++) {
            biomeWeightingPass();
        }
        
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
                    else if( randomRoll > fieldProb && randomRoll <= cemeteryProb + fieldProb)
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

    protected void swapTile(int xCoord, int yCoord, GameObject tileToSwapTo)
    {
        GameObject oldTile = tileInstances[xCoord][yCoord];
        tileInstances[xCoord][yCoord] = Instantiate(tileToSwapTo, oldTile.transform.position, oldTile.transform.rotation);
        Destroy(oldTile);
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

    protected GameObject getStraightUpRandomTile()
    {
        int count = biomeTiles.Count;
        int randomNumber = Random.Range(0, count);
        return biomeTiles[randomNumber];
    }


}
