using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapSpawner : MonoBehaviour
{
    public int biomeWeightingPasses;
    public Vector2Int mapDimension;
    public List<GameObject> biomeTiles;
    public List<SceneryProbabiltyDeterminer> sceneryStuff;
    public GameObject playerPrefab;
    public GameObject enemySpawner;
    public List<GameObject> cemeteryBiomeTile;
    public List<GameObject> pumpkinBiomeTile;
    public List<GameObject> fieldBiomeTile;
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

    public Image loadingImage;
    public GameObject loadingUI;

    private float loadingCompletion = 0f;
    private Vector2Int tileDimension;
    private List<List<GameObject>> tileInstances;
    private List<GameObject> itemInstances;
    private List<GameObject> fenceInstances;
    private List<GameObject> enemySpawnerInstances;
    private List<GameObject> sceneryInstances;
    private GameObject playerInstance;
    protected void Awake()
    {
        tileInstances = new List<List<GameObject>>();
        itemInstances = new List<GameObject>();
        fenceInstances = new List<GameObject>();
        enemySpawnerInstances = new List<GameObject>();
        sceneryInstances = new List<GameObject>();
        for(int i = 0; i < mapDimension.x; i++)
        {
            tileInstances.Add(new List<GameObject>());
        }

        tileDimension = new Vector2Int(20, 20);
        updateLoading(0f);
        StartCoroutine(spawnMap());
    }

    protected IEnumerator spawnMap()
    {
        yield return null;

        //TODO make loading screen here
        yield return mapSpawnInitialPass();
        updateLoading(.1f);

        yield return biomeWeightingCoroutine();
        updateLoading(.3f);

        yield return spawnFences();
        updateLoading(.4f);

        yield return spawnItems();
        updateLoading(.5f);

        yield return spawnScenery();
        updateLoading(.6f);

        makeItemTurnerOfferThing();

        yield return spawnEnemySpawners();
        updateLoading(.7f);

        yield return spawnEnemies();
        updateLoading(.98f);

        playerInstance = Instantiate(playerPrefab, new Vector3((mapDimension.x / 2) * tileDimension.x, 0f, (mapDimension.y / 2) * tileDimension.y) + new Vector3(5f, 0, 1f), Quaternion.identity);
        playerInstance.GetComponentInChildren<PickupHinter>().SetItems(itemInstances);
        Debug.Log("Map spawning complete");
        updateLoading(1.0f);
        
    }

    protected void updateLoading(float amt)
    {
        loadingCompletion = amt;
        loadingImage.fillAmount = loadingCompletion;
        if(loadingCompletion > .99f)
        {
            endLoadingScreen();
        }
    }

    protected void endLoadingScreen()
    {
        loadingUI.SetActive(false);
    }
    protected IEnumerator mapSpawnInitialPass()
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
            updateLoading(0f + (float)i / mapDimension.x * .1f);
            yield return null;
        }
    }

    protected IEnumerator biomeWeightingCoroutine()
    {
        for (int i = 0; i < biomeWeightingPasses; i++)
        {
            
            biomeWeightingPass();
            updateLoading(.1f + (float)i / biomeWeightingPasses * .2f);
            yield return null;
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
                            swapTile(i, j, getTileFromBiome(Biome.field));
                        }
                        else if (randomRoll > fieldProb && randomRoll <= cemeteryProb + fieldProb)
                        {
                            swapTile(i, j, getTileFromBiome(Biome.cemetery));
                        }
                        else
                        {
                            swapTile(i, j, getTileFromBiome(Biome.pumpkin));
                        }
                    }
                }
            }
        }
    }

    protected GameObject getTileFromBiome(Biome biome)
    {
        if(biome == Biome.cemetery)
        {
            return cemeteryBiomeTile[Random.Range(0, cemeteryBiomeTile.Count)];
        }
        else if (biome == Biome.field)
        {
            return fieldBiomeTile[Random.Range(0, fieldBiomeTile.Count)];
        }
        else//pumpkin
        {
            return pumpkinBiomeTile[Random.Range(0, pumpkinBiomeTile.Count)];
        }
    }

    protected void swapTile(int xCoord, int yCoord, GameObject tileToSwapTo)
    {
        GameObject oldTile = tileInstances[xCoord][yCoord];
        tileInstances[xCoord][yCoord] = Instantiate(tileToSwapTo, oldTile.transform.position, oldTile.transform.rotation);
        Destroy(oldTile);
    }

    protected IEnumerator spawnItems()
    {
        for (int i = 0; i < mapDimension.x; i++)
        {
            for (int j = 0; j < mapDimension.y; j++)
            {
                if(Random.Range(0f, 1f)  < getTileDistanceRatioFromCenter(i, j))
                {
                    Vector3 spawnPosition = tileInstances[i][j].transform.position + new Vector3(Random.Range(1f, tileDimension.x - 1), 0f, Random.Range(1f, tileDimension.y - 1));
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
            updateLoading(.4f + (float)i / mapDimension.x * .1f);
            yield return null;
        }

    }

    protected IEnumerator spawnScenery()
    {
        float tileXMid = (tileDimension.x / 2);
        float tileYMid = (tileDimension.x / 2);
        float tileXMax = tileDimension.x - 1;
        float tileYMax = tileDimension.y - 1;

        for (int i = 0; i < mapDimension.x; i++)
        {
            for (int j = 0; j < mapDimension.y; j++)
            {
                MapTile tile = tileInstances[i][j].GetComponent<MapTile>();
                if (tile != null)
                {
                    if (tile.biome == Biome.field) //Hack to make this only apply to the field for now
                    {
                        if (Random.Range(0f, 1f) > .9f)
                        {
                            GameObject obj = getWeightedProbabilitySceneryItem(tile.biome);
                            if (obj != null)
                            {
                                Instantiate(obj, tileInstances[i][j].transform.position + new Vector3(Random.Range(1f, tileXMid), 0f, Random.Range(1f, tileYMid)), Quaternion.identity * Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
                                sceneryInstances.Add(obj);
                            }
                        }
                        if (Random.Range(0f, 1f) > .9f)
                        {
                            GameObject obj = getWeightedProbabilitySceneryItem(tile.biome);
                            if (obj != null)
                            {
                                Instantiate(obj, tileInstances[i][j].transform.position + new Vector3(Random.Range(tileXMid, tileXMax), 0f, Random.Range(1f, tileYMid)), Quaternion.identity * Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
                                sceneryInstances.Add(obj);
                            }
                        }
                        if (Random.Range(0f, 1f) > .9f)
                        {
                            GameObject obj = getWeightedProbabilitySceneryItem(tile.biome);
                            if (obj != null)
                            {
                                Instantiate(obj, tileInstances[i][j].transform.position + new Vector3(Random.Range(1f, tileXMid), 0f, Random.Range(tileYMid, tileYMax)), Quaternion.identity * Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
                                sceneryInstances.Add(obj);
                            }
                        }
                        if (Random.Range(0f, 1f) > .9f)
                        {
                            GameObject obj = getWeightedProbabilitySceneryItem(tile.biome);
                            if (obj != null)
                            {
                                Instantiate(obj, tileInstances[i][j].transform.position + new Vector3(Random.Range(tileXMid, tileXMax), 0f, Random.Range(tileYMid, tileYMax)), Quaternion.identity * Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
                                sceneryInstances.Add(obj);
                            }
                        }
                    }
                }
            }
            updateLoading(.5f + (float)i / mapDimension.x * .1f);
            yield return null;
        }
    }

    protected GameObject getRandomSceneryItem()
    {
        int roll = Random.Range(0, sceneryStuff.Count);
        return sceneryStuff[roll].obj;
    }

    protected GameObject getWeightedProbabilitySceneryItem(Biome biome)
    {
        if(biome == Biome.cemetery)
        {
            float sumOfProbs = 0;
            foreach(SceneryProbabiltyDeterminer spd in sceneryStuff) { sumOfProbs += spd.cemeteryProb; }
            float probTracker = 0;
            float previousProbabilityTracker = 0;
            float roll = Random.Range(0f, sumOfProbs);
            for (int i = 0; i < sceneryStuff.Count; i++)
            {
                probTracker += sceneryStuff[i].cemeteryProb;
                if (probTracker > roll && previousProbabilityTracker <= roll)
                {
                    return sceneryStuff[i].obj;
                }
                previousProbabilityTracker = probTracker;
            }
        }
        else if(biome == Biome.field)
        {
            float sumOfProbs = 0;
            foreach (SceneryProbabiltyDeterminer spd in sceneryStuff) { sumOfProbs += spd.fieldProb; }
            float probTracker = 0;
            float previousProbabilityTracker = 0;
            float roll = Random.Range(0f, sumOfProbs);
            for (int i = 0; i < sceneryStuff.Count; i++)
            {
                probTracker += sceneryStuff[i].fieldProb;
                if (probTracker > roll && previousProbabilityTracker <= roll)
                {
                    return sceneryStuff[i].obj;
                }
                previousProbabilityTracker = probTracker;
            }
        }
        else//pumpkin
        {
            float sumOfProbs = 0;
            foreach (SceneryProbabiltyDeterminer spd in sceneryStuff) { sumOfProbs += spd.pumpkinProb; }
            float probTracker = 0;
            float previousProbabilityTracker = 0;
            float roll = Random.Range(0f, sumOfProbs);
            for (int i = 0; i < sceneryStuff.Count; i++)
            {
                probTracker += sceneryStuff[i].pumpkinProb;
                if (probTracker > roll && previousProbabilityTracker <= roll)
                {
                    return sceneryStuff[i].obj;
                }
                previousProbabilityTracker = probTracker;
            }
        }
        return null;
    }

    


    protected IEnumerator spawnEnemySpawners()
    {
        for (int i = 0; i < mapDimension.x; i++)
        {
            
            for (int j = 0; j < mapDimension.y; j++)
            {
                Vector3 spawnPosition = tileInstances[i][j].transform.position + new Vector3(Random.Range(1f, tileDimension.x - 1), 0f, Random.Range(1f, tileDimension.y - 1));
                if (Random.Range(0f, 1f) < getTileDistanceRatioFromCenter(i, j))
                {
                    enemySpawnerInstances.Add(Instantiate(enemySpawner, spawnPosition, Quaternion.identity));
                }
            }
            updateLoading(.6f + (float)i / mapDimension.x * .1f);
            yield return null;
        }

        ItemTurnerOnAndOffer jeff = gameObject.GetComponent<ItemTurnerOnAndOffer>();
        if(jeff != null)
        {
            foreach(GameObject obj in enemySpawnerInstances)
            {
                EnemySpawner es = obj.GetComponent<EnemySpawner>();
                if(es != null)
                {
                    es.init(jeff);
                }
            }
        }
    }

    protected IEnumerator spawnEnemies()
    {
        for(int i = 0; i < enemySpawnerInstances.Count; i++)
        {
            EnemySpawner es = enemySpawnerInstances[i].GetComponent<EnemySpawner>();
            if(es != null)
            {
                es.spawnEnemy();
            }
            updateLoading(.7f + (float)i / enemySpawnerInstances.Count * .28f);
            yield return null;
        }

    }

    protected float getTileDistanceRatioFromCenter(int x, int y)
    {
        Vector2Int centerTile = new Vector2Int(mapDimension.x / 2, mapDimension.y / 2);
        float dist = Vector2Int.Distance(centerTile, new Vector2Int(x, y));
        float ratio = dist / (mapDimension.x);
        return ratio;
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

    protected IEnumerator spawnFences()
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
            updateLoading(.3f + (float)i / mapDimension.x * .1f);
            yield return null;
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

    protected void makeItemTurnerOfferThing()
    {
        gameObject.AddComponent<ItemTurnerOnAndOffer>().init(tileInstances, itemInstances, fenceInstances, sceneryInstances, playerInstance); 
    }

    protected GameObject getStraightUpRandomTile()
    {
        int count = biomeTiles.Count;
        int randomNumber = Random.Range(0, count);
        return biomeTiles[randomNumber];
    }


}

[System.Serializable]
public class SceneryProbabiltyDeterminer
{
    public GameObject obj;
    public float cemeteryProb;
    public float fieldProb;
    public float pumpkinProb;
}
