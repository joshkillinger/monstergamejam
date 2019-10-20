using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTile : MonoBehaviour
{
    public Vector2Int tileDimension = new Vector2Int(10,10);
    public Biome biome;
}

public enum Biome { field, pumpkin, corn, cemetery, fence}
