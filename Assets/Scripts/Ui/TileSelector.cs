using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileSelector : MonoBehaviour
{

    public Material SelectionTexture;

    string MapTile = "MapTile";

    private void OnEnable() => RayCastHandler.OnRayCast += selectTile;

    private void OnDisable() => RayCastHandler.OnRayCast -= selectTile;

    public void selectTile(Ray ray)
    {
        GameObject gameTileToProcess;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            string objectName = hit.collider.name;
            Debug.Log(objectName);

            gameTileToProcess = hit.collider.gameObject;

            if (gameTileToProcess.tag == MapTile)
            {
                changeMaterial(gameTileToProcess);
            }


        }

    }

    private void changeMaterial(GameObject tile)
    {
        Renderer tileRenderer = tile.GetComponent<Renderer>();

        if (tileRenderer != null)
        {
            Material[] materials = tileRenderer.materials;

            materials[0] = SelectionTexture;

            tileRenderer.materials = materials;
        }


    }
}
