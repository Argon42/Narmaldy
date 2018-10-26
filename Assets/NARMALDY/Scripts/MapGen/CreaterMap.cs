using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Map;

public class CreaterMap : MonoBehaviour{

    [SerializeField] Tilemap FloorMap;
    [SerializeField] Tile FloorTile;
    [SerializeField] uint LevelLength, LevelHeight, MaxWidthRoomInLevel, MaxHightRoomInLevel,seed;

    public void Gen()
    {
        Debug.Log(FloorMap.size);
        MapGenerator1 generator1 = new MapGenerator1();
        generator1.SetGeneratorSettings(LevelLength, LevelHeight, MaxWidthRoomInLevel, MaxHightRoomInLevel,(int)seed);
        generator1.Gen(FloorMap, FloorTile);
    }
}
