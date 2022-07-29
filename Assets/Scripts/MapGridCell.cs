using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGridCell
{
    public GameObject gridCellInstance;
    public Vector2Int coords;

    public void Setup(GameObject instance, Vector2Int cellCoords)
    {
        gridCellInstance = instance;
        coords = cellCoords;
    }
}
