using UnityEngine;
using System;
using System.Collections.Generic;

public class SetTilePrefab : MonoBehaviour
{
    public List<GameObject> prefabList;

    public void setPrefabForTile(GameTile tileToSet)
    {
        tileToSet.tilePrefab = prefabList[0];
    }

}
