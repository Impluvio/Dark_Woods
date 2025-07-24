using UnityEngine;

public class GameTile
{
    public string name;
    public GameTile[] tileneighbours;
    public Vector3Int tilePosition;
    public GameObject tilePrefab {get; set; }

    public GameTile(int xCoordinate, int yCoordinate)
    {
        tilePosition = new Vector3Int(xCoordinate, yCoordinate, 0); // this needs to relate to  the 
    }

}





//add constructor

//add method to select from the prefab manager