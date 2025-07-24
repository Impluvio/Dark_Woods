using System;
using UnityEngine;

public class MapCreator : MonoBehaviour
{
    // we want this script to take a series of tiles
    // we want to create a datastructure to represent the tile.
    // we instantiate a 2d array of these data structures. randomise their layout
    // set the visible playspace to a 9 x 9 grid.
    // we want to instantiate these at the position

    public SetTilePrefab setTilePrefab; 

    [Range(10,200)] public int mapSize;
    GameTile[,] mapGrid;

    private void Awake()
    {
        setTilePrefab = GetComponent<SetTilePrefab>();
    }

    void Start()
    {
        setGrid(mapSize);

    }

    private void setGrid(int sizeOfMap)
    {
        mapGrid = new GameTile[sizeOfMap, sizeOfMap];

        for (int i = 0; i < sizeOfMap; i++)
        {
            for (int j = 0; j < sizeOfMap; j++)
            {
                GameTile newTileInstance = new GameTile(i, j);
                string tileName = $"[{i},{j}]";
                newTileInstance.name = tileName;
                mapGrid[i, j] = newTileInstance;
                setTilePrefab.setPrefabForTile(newTileInstance);

            }
        }

    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
