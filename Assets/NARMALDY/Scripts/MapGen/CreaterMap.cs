using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Map;

public class CreaterMap : MonoBehaviour{

    [SerializeField] Tilemap FloorMap;
    [SerializeField] Tile FloorTile;
    [SerializeField] Tilemap ColMap;
    [SerializeField] Tile Wall;
    [SerializeField] Tile Roof;
    [SerializeField] GameObject Player;

    [SerializeField] uint LevelLength, LevelHeight, MaxWidthRoomInLevel, MaxHightRoomInLevel,seed;

    public void Gen()
    {
        ColMap.ClearAllTiles();
        FloorMap.ClearAllTiles();
        //seed++;
        MapGenerator1 generator1 = new MapGenerator1();
        generator1.SetGeneratorSettings(LevelLength, LevelHeight, MaxWidthRoomInLevel, MaxHightRoomInLevel,(int)seed);
        generator1.Gen(FloorMap, FloorTile);

        Debug.Log(FloorMap.size);
        for (int i = 0; i < FloorMap.size.x; i++)
        {
            for (int j = 0; j < FloorMap.size.y; j++)
            {
                if(FloorMap.GetTile(new Vector3Int(i,j,0)) == FloorTile)
                {
                    if (FloorMap.GetTile(new Vector3Int(i, j + 1, 0)) == null)
                    {
                        ColMap.SetTile(new Vector3Int(i, j + 1, 0), Wall);
                        if (FloorMap.GetTile(new Vector3Int(i, j + 2, 0)) == null)
                            ColMap.SetTile(new Vector3Int(i, j + 2, 0), Roof);
                    }
                    if (FloorMap.GetTile(new Vector3Int(i, j - 1, 0)) == null && ColMap.GetTile(new Vector3Int(i, j - 1, 0)) == null)
                    {
                        ColMap.SetTile(new Vector3Int(i, j - 1, 0), Roof);
                    }
                    if (FloorMap.GetTile(new Vector3Int(i- 1, j , 0)) == null && ColMap.GetTile(new Vector3Int(i- 1, j , 0)) == null)
                    {
                        ColMap.SetTile(new Vector3Int(i- 1, j , 0), Roof);
                    }
                    if (FloorMap.GetTile(new Vector3Int(i+1, j , 0)) == null && ColMap.GetTile(new Vector3Int(i+1, j , 0)) == null)
                    {
                        ColMap.SetTile(new Vector3Int(i+1, j , 0), Roof);
                    }
                }
            }
        }

        Player.transform.position = generator1.AllRooms[0].GetCenterPosition();
    }
}
