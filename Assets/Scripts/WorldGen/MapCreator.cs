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

    public ARAnchorManager anchorManager; // is this required?
    public ARAnchor origin;

    [SerializeField] private GameObject gridManagerPrefab; //takes the prefab parent of the grid
    private GameObject atlas; //
    private GameObject baseTile; 

    [Range(10, 400)] public int mapSize = 10;   // sets grid size
    GameTile[,] mapGrid;                        // stores instances of the gameTile asset


    private void Awake()
    {
        setTilePrefab = GetComponent<SetTilePrefab>();  // Grabs the setTile script
        printPlaneID = GetComponent<PrintPlaneID>();
    }

    public void InitialiseMap(TrackableId playAreaID)
    {
        Debug.Log(playAreaID.ToString());
        ARAnchor origin = anchorManager.GetAnchor(playAreaID);
        atlas = Instantiate(gridManagerPrefab, origin.transform); //sets the parent for the grid/map.
        setGrid(mapSize);
        PopulateAtlas(atlas);
        updateMap();


        //printPlaneID.PrintMessage(worldOrigin.transform.position.ToString());
        //


    }

    private void PopulateAtlas(GameObject atlas)
    {
        
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
        //printPlaneID.PrintMessage(worldOrigin.transform.position);
        foreach (GameTile gameTile in mapGrid)
        {
            Vector3 AdjustedCoordinates = new Vector3(gameTile.tilePosition.x / 10, 0, gameTile.tilePosition.y / 10);
            Debug.Log($"tile poisiton: {AdjustedCoordinates.ToString()}");
            
            Instantiate(atlas, AdjustedCoordinates, Quaternion.identity);
        }
    }

    



}
