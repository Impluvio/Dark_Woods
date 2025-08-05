using System;
using System.Collections;
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
        // Debug.Log(playAreaID.ToString());
        ARAnchor origin = anchorManager.GetAnchor(playAreaID);
        atlas = Instantiate(gridManagerPrefab, origin.transform); //sets the parent for the grid/map.
        setGrid(mapSize);
        PopulateAtlas();
        updateMap();


        //printPlaneID.PrintMessage(worldOrigin.transform.position.ToString());
        //


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
                Debug.Log("tile created: " + tileName);
            }
        }

    }

    public void updateMap()
    {
        

        // here we were deciding whether to put a ref to the atlas, and attach the gameobjects that represent the tiles below. 
        Vector3 mapOrigin = atlas.transform.position;


        foreach (GameTile gameTile in mapGrid)
        {
            Vector3 rawCoordinates = new Vector3(gameTile.tilePosition.x, 0, gameTile.tilePosition.z);
            Vector3 decimatedCoordinates = new Vector3(rawCoordinates.x / 10, 0, rawCoordinates.z / 10);
            Vector3 adjustedCoordinates = new Vector3(decimatedCoordinates.x + mapOrigin.x, 0.1f, decimatedCoordinates.z + mapOrigin.z);

            Instantiate(baseTile, adjustedCoordinates, Quaternion.identity);


          
        }

       
    }

    private void PopulateAtlas()  //GameObject atlas
    {
        foreach (GameTile gameTile in mapGrid)
        {
            Debug.Log("tilePosition is set to: " + gameTile.tilePosition);
        }
    }



}
