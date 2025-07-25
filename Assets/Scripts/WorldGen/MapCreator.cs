using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MapCreator : MonoBehaviour
{
    // we want this script to take a series of tiles
    // we want to create a datastructure to represent the tile.
    // we instantiate a 2d array of these data structures. randomise their layout
    // set the visible playspace to a 9 x 9 grid.
    // we want to instantiate these at the position

    public SetTilePrefab setTilePrefab;
    public PrintPlaneID printPlaneID;
    public ARAnchor worldOrigin { get; set; }

    [Range(10,200)] public int mapSize; // sets grid size
    GameTile[,] mapGrid;                // stores instances of the gameTile asset


    private void Awake()
    {
        setTilePrefab = GetComponent<SetTilePrefab>();  // Grabs the setTile script
        printPlaneID = GetComponent<PrintPlaneID>(); 
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
                setTilePrefab.setPrefabForTile(newTileInstance); // this needs to instantiate acounting for the anchor position.

            }
        }

    }

    public void updateMap()
    {
        printPlaneID.PrintMessage("Update map met");

        if (worldOrigin == null || worldOrigin.transform == null)
        {
            
            return;
        }


        
        foreach (GameTile gameTile in mapGrid)        
        {
            Vector3 AdjustedCoordinates = worldOrigin.transform.position + new Vector3(gameTile.tilePosition.x, gameTile.tilePosition.y, 0);
            Instantiate(gameTile.tilePrefab, AdjustedCoordinates, worldOrigin.transform.rotation);
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
