using UnityEngine;

public class GameTile
{
    public string name;
    public GameTile[] tileneighbours;
    public Vector3Int tilePosition;
    public GameObject tilePrefab {get; set; }

    public GameTile(int xCoordinate, int zCoordinate)
    {
        tilePosition = new Vector3Int(xCoordinate, 0, zCoordinate); // this needs to be related to the local space of ar anchor
    }

}







//add method to select from the prefab manager