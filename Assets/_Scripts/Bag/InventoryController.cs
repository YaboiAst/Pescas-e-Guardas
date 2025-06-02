using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryController : MonoBehaviour
{
    private Dictionary<Vector2Int, GridTile> _tiles;
    public void InitializeGrid() => _tiles = new Dictionary<Vector2Int, GridTile>();
    public void AddTile(GridTile t) => _tiles.Add(t.GetCoord(), t);

    public static readonly UnityEvent<ItemPlacer> CheckOverlap = new();
    public static readonly UnityEvent ClearGrid = new();
}
