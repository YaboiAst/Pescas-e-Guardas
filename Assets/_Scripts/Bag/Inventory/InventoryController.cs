using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[Serializable]
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
        valid &= buffer.Count > 0;
        
        var color = valid ? Color.green : Color.red;
        buffer.ForEach(tile => tile.PaintTile(color));

        return valid;
    }

    public bool IsValid() => valid;
}

public class InventoryController : MonoBehaviour, IDataPersistence
{
    public static InventoryController Instance;

    [FormerlySerializedAs("_itemPrefab")] [SerializeField] private GameObject itemPrefab;
    [FormerlySerializedAs("_spawnParent")] [SerializeField] private Transform spawnParent;
    private BagData _currentBag;

    private Dictionary<Vector2Int, GridTile> _tiles;
    public void InitializeGrid() => _tiles = new Dictionary<Vector2Int, GridTile>();
    public void AddTile(GridTile t) => _tiles.Add(t.GetCoord(), t);

    public ItemPlacer CurrentSelectedItem { get; private set; }
    public static void SelectItem(ItemPlacer item) => Instance.CurrentSelectedItem ??= item;
    public static void DeselectItem() => Instance.CurrentSelectedItem = null;
    public static bool IsItemSelected() => Instance.CurrentSelectedItem is not null;
    
    public static readonly UnityEvent<ItemPlacer> CheckOverlap = new UnityEvent<ItemPlacer>();
    public static readonly UnityEvent ClearGrid = new UnityEvent();
    public static event Action OnItemPlaced;
    public static readonly UnityEvent OnProgressQuest = new();

    private SelectionBuffer _tileBuffer;
    public int TotalPoints { get; private set; } = 0;

    public List<ItemPlacer> PlacedItems;
    private InventoryUI _ui;
    private GridGeneration _gridGenerator;

    public Vector2 GridCellSize => _gridGenerator.GridRectSize;
    
    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(this.gameObject);

        _tileBuffer = new SelectionBuffer();
        ClearGrid.AddListener(ResetBuffer);

        PlacedItems = new List<ItemPlacer>();
        _ui = GetComponentInParent<InventoryUI>();
        
        _gridGenerator = GetComponent<GridGeneration>();
        _gridGenerator.SetBag(_currentBag);
    }

    private void Start()
    {
        QuestManager.OnStartQuest.AddListener(InitializeBag);
        QuestManager.OnFinishQuest.AddListener(ClearInventory);
    }

    private void InitializeBag(QuestProgress questProgress)
    {
        _gridGenerator ??= GetComponent<GridGeneration>();
        _currentBag = questProgress.QuestData.questBag;
        _gridGenerator.SetBag(_currentBag);
    }
    
    private void ClearInventory()
    {
        for (int i = PlacedItems.Count - 1; i >= 0; i--)
        {
            ItemPlacer item = PlacedItems[i];
            if (item)
                Destroy(item.gameObject);
        }

        PlacedItems.Clear();

        CalculatePoints();
    }
    public static void CheckGrid(ItemPlacer item)
    {
        Instance.ResetBuffer();
        CheckOverlap?.Invoke(item);
        Instance.ValidateBuffer(item);
    }

    public void CreateItem(Fish fish)
    {
        ItemPlacer item = Instantiate(itemPrefab, spawnParent).GetComponent<ItemPlacer>();
        item.Initialize(fish, this);
        SelectItem(item);
        _ui.ShowInventory();
    }

    public void AddToBuffer(GridTile tile) => _tileBuffer.Add(tile);
    public void ResetBuffer() => _tileBuffer.Clear();
    public void ValidateBuffer(ItemPlacer item) => _tileBuffer.Validate(item);
    public bool IsPositionValid() => _tileBuffer.IsValid();
    public void UpdatePlacement(ItemPlacer item = null)
    {
        foreach (var tile in _tileBuffer.buffer)
            tile.SetItemInTile(item);

        if (!PlacedItems.Contains(item))
            PlacedItems.Add(item);

        CalculatePoints();
    }

    public void AddItem(ItemPlacer item)
    {
        if (PlacedItems.Contains(item)) return;

        PlacedItems.Add(item);
        item.SetInventory(this);
        item.SetPositionStatus(true);
    }
    public bool RemoveItem(ItemPlacer item)
    {
        if (PlacedItems.Contains(item))
        {
            PlacedItems.Remove(item);
            CalculatePoints();
            return true;
        }

        return false;
    }

    private void CalculatePoints()
    {
        TotalPoints = 0;

        foreach (var item in PlacedItems)
            TotalPoints += item.GetItemData().Fish.Points;

        OnItemPlaced?.Invoke();
        OnProgressQuest.Invoke();
    }

    public static void ShowInventory()
    {
        Instance._ui.ShowInventory();
    }
    public static void HideInventory()
    {
        Instance._ui.HideInventory();
    }
    
    public void LoadData(GameData data)
    {
    }
    public void SaveData(GameData data)
    {
    }
    
    [Button("Test bag")]
    public void TestBag()
    { 
        HideInventory();
        _gridGenerator?.SetBag(_currentBag);
        ShowInventory();
    }
}