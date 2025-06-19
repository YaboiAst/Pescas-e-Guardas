using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SelectionBuffer
{
    public List<GridTile> buffer { get; private set; }
    private bool valid;

    public SelectionBuffer()
    {
        this.buffer = new List<GridTile>();
        this.valid = false;
    }

    public void Add(GridTile tile)
    {
        buffer.Add(tile);
    }
    public void Clear()
    {
        buffer.Clear();
        valid = false;
    }
    public bool Validate(ItemPlacer item)
    {
        valid = !buffer.Any(tile => tile.IsOccupied);
        valid &= item.GetRects().Count <= buffer.Count;
        
        var color = valid ? Color.green : Color.red;
        buffer.ForEach(tile => tile.PaintTile(color));
        
        return valid;
    }
    public bool IsValid() => valid;
}

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;
    
    private Dictionary<Vector2Int, GridTile> _tiles;
    public void InitializeGrid() => _tiles = new Dictionary<Vector2Int, GridTile>();
    public void AddTile(GridTile t) => _tiles.Add(t.GetCoord(), t);

    public static readonly UnityEvent<ItemPlacer> CheckOverlap = new();
    public static readonly UnityEvent ClearGrid = new();
    private SelectionBuffer _tileBuffer;

    public static List<ItemPlacer> FishesInBag;

    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(this.gameObject);

        _tileBuffer = new SelectionBuffer();
        ClearGrid.AddListener(ResetBuffer);
        
        FishesInBag = new List<ItemPlacer>();
    }

    public static void CheckGrid(ItemPlacer item)
    {
        Instance.ResetBuffer();
        CheckOverlap?.Invoke(item);
        Instance.ValidateBuffer(item);
    }

    public void AddToBuffer(GridTile tile) => _tileBuffer.Add(tile);
    public void ResetBuffer() => _tileBuffer.Clear();
    public void ValidateBuffer(ItemPlacer item) => _tileBuffer.Validate(item);
    public bool IsPositionValid() => _tileBuffer.IsValid();
    public void UpdatePlacement(ItemPlacer item = null)
    {
        foreach (var tile in _tileBuffer.buffer)
            tile.SetItemInTile(item);
        
        FishesInBag.Add(item);
    }
}
