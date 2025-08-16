using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
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

    public ARAnchorManager anchorManager;                   // is this required?
    public ARAnchor origin;

    [Tooltip("Empty game object to contain all grid tiles")] 
    [SerializeField] private GameObject gridManagerPrefab;  //takes the prefab parent of the grid

    private GameObject atlas;                               //this is the game object that is set to the AR Anchor
    [Tooltip("basic tile prefab goes here")]    
    public GameObject baseTile;                            //this is the tile prefab used in the update map function 

    [Range(10, 400)] public int mapSize = 10;   // sets grid size
    GameTile[,] mapGrid;                        // stores instances of the gameTile asset


    private void Awake()
    {
        setTilePrefab = GetComponent<SetTilePrefab>();  // Grabs the setTile script
        printPlaneID = GetComponent<PrintPlaneID>();
    }

    public void InitialiseMap(TrackableId playAreaID)
    {
        ARAnchor origin = anchorManager.GetAnchor(playAreaID);
        atlas = Instantiate(gridManagerPrefab, origin.transform); //sets the parent for the grid/map.
        setGrid(mapSize);
        setTileNeighbours();
        testNeighbours();
        //PopulateAtlas();
        updateMap();


        

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
                Debug.Log("tile created: " + tileName);
            }
        }

    }

    private void setTileNeighbours() 
    {
        int mapExtent = mapSize; // grid is always square, so only one var to ref height / width.
        

        for (int i = 0; i < mapExtent; i++)
        {
            for (int j = 0; j < mapExtent; j++)
            {
                GameTile[] neighbours = new GameTile[4];

                if (i + 1 >= 0 && i + 1 < mapExtent && j >= 0 && j < mapExtent) //check tile up from present tile
                {
                    neighbours[0] = mapGrid[i + 1, j];
                }
                else { }
                if (i >= 0 && i < mapExtent && j + 1 >= 0 && j + 1 < mapExtent) //check tile right from present tile
                {
                    neighbours[1] = mapGrid[i, j + 1];
                }
                else { }
                if (i - 1 >= 0 && i < mapExtent && j >= 0 && j < mapExtent) //check tile down from present tile
                {
                    neighbours[2] = mapGrid[i - 1, j];
                }
                else { }
                if (i >= 0 && i < mapExtent && j - 1 >= 0 && j - 1 < mapExtent) //check tile left from present tile
                {
                    neighbours[3] = mapGrid[i, j - 1];
                }
                else { }

                GameTile currentTile = mapGrid[i, j];
                currentTile.tileNeighbours = neighbours;
            }
        }
    }

    private void randomWalker(GameTile startTile, int stepNo, int walkerNo) // add a param that is tile type.
    {
       
        GameTile tileToCheck = startTile;
       
        while (stepNo > 0)
        {
            int randomDirection = UnityEngine.Random.Range(0, 3);
            GameTile tileNeighbour = tileToCheck.tileNeighbours[randomDirection];

            if (tileNeighbour != null)
            {
                //flip tile state - either using an array of tiles or using a switch within the tile class
                tileToCheck = tileNeighbour;
                stepNo--;
            }
            else
            {
                randomDirection = UnityEngine.Random.Range(0, 3);
            }
  
        }
    }

    private void testNeighbours()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                Debug.Log($"tile {mapGrid[i, j].name} has neighbours: ");
                int counter = 0;
                foreach (GameTile neighbour in  mapGrid[i, j].tileNeighbours)
                {
                    if (neighbour != null)
                    {
                        Debug.Log($"neighbour {counter} " + neighbour.name);
                        counter++;
                    }
                    else { }
                    
                }
            }



        }
    }

    public void updateMap()
    {
        // Todo: child grid to Anchor (then make the grid local space)
        // Look at offset as anchor and plane appear higher in simulation than expected.  


        Vector3 mapOrigin = atlas.transform.position;

        foreach (GameTile gameTile in mapGrid)
        {
            Vector3 rawCoordinates = new Vector3(gameTile.tilePosition.x, 0, gameTile.tilePosition.z);
            Vector3 decimatedCoordinates = new Vector3(rawCoordinates.x / 10, 0, rawCoordinates.z / 10);
            Vector3 adjustedCoordinates = new Vector3(decimatedCoordinates.x + mapOrigin.x, 0.1f, decimatedCoordinates.z + mapOrigin.z);

            Instantiate(baseTile, adjustedCoordinates, Quaternion.identity);
        }

       
    }

   



}
