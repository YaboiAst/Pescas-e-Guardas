using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPlacer : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;
    [SerializeField] private List<ItemPlacerBlock> blocksRects;
    [SerializeField] private RectTransform _blocksRoot;
    [SerializeField] private GameObject _blockPrefab;

    private GridLayoutGroup _grid;
    private DraggableItem _drag;

    private bool _isOnGrid = false;

    [SerializeField] private ItemData _data;
    
    [SerializeField] private InventoryController _currentInventory;

    public ItemData GetItemData() => _data;

    public void Initialize(Fish fish, InventoryController inventory)
    {
        _grid = _blocksRoot.GetComponent<GridLayoutGroup>();

        _rect.sizeDelta = new Vector2(
            fish.FishData.Width * _grid.cellSize.x,
            fish.FishData.Height * _grid.cellSize.y
        );

        _image.sprite = fish.FishData.Icon;

        foreach (ShapeRow row in fish.FishData.Shape)
        {
            foreach (bool rowCol in row.Cols)
            {
                GameObject block = Instantiate(_blockPrefab, _blocksRoot);
                ItemPlacerBlock itemPlacerBlock = block.GetComponent<ItemPlacerBlock>();
                if (rowCol)
                {
                    blocksRects.Add(itemPlacerBlock);
                    itemPlacerBlock.GetComponent<Image>().enabled = true;
                }
                _data = new ItemData(fish, block.transform.localPosition, 0);
            }
        }

        var elements = new List<TooltipElementInfo>();

        var weight = new TooltipElementInfo
        {
            Name = "Weight"
        };

        if (fish.Weight > 1f)
            weight.Value = $"{fish.Weight}kg";
        else
            weight.Value = $"{fish.Weight * 100}g";

        elements.Add(weight);

        TooltipInfo info = new TooltipInfo
        {
            Header = fish.FishData.DisplayName,
            Content = fish.FishData.Description,
            Elements = elements,
            Actions = new List<TooltipActionInfo>(),
            Icon = fish.FishData.Icon
        };


        SetInventory(inventory);

        _drag = this.GetComponent<DraggableItem>();
        _drag.InitializeDrag();
        GetComponent<TooltipTriggerUI>().SetTooltipInfo(info);
    }
    
    public void SetInventory(InventoryController inventory) => _currentInventory = inventory;

    public void SetItemData(ItemData data) => _data = data;

    public void SetPositionStatus(bool status) => _isOnGrid = status;

    public bool GetPositionStatus() => _isOnGrid;

    public RectTransform GetRoot()
    {
        return _blocksRoot;
    }

    public List<Rect> GetRects()
    {
        var rectList = new List<Rect>();
        foreach (var block in blocksRects)
        {
            rectList.Add(block.GetRect());
        }
        return rectList;
    }

    public void PlaceItem(Image targetVisual, Vector3 newPosition, float newRotation)
    {
        transform.position = newPosition;

        targetVisual.transform.eulerAngles = Vector3.forward * newRotation;
        GetRoot().transform.eulerAngles = Vector3.forward * newRotation;

        this.GetRoot().position = transform.position;
        targetVisual.transform.position = transform.position;

        // Add Fish Data to inventory
        InventoryController.ClearGrid?.Invoke();
    }

    public void SetItemInGrid()
    {
        InventoryController.Instance.UpdatePlacement(this);
    }

    public void HandleClick(PointerEventData eventData)
    {
        _drag.OnPointerClick(eventData);
    }
    
    // Returns the center point (in local space of _blocksRoot) of all active blocks
    private Vector3 GetBlocksLocalCenter()
    {
        if (blocksRects == null || blocksRects.Count == 0)
            return Vector3.zero;
        Vector3 sum = Vector3.zero;
        foreach (Transform child in _blocksRoot)
        {
            sum += child.localPosition;
            
        }
        return sum / _blocksRoot.childCount;
    }

    // Returns the local position of the top-left active block
    private Vector3 GetTopLeftBlockLocalPosition()
    {
        if (blocksRects == null || blocksRects.Count == 0)
            return Vector3.zero;
        Vector3 topLeft = blocksRects[0].transform.localPosition;
        foreach (var block in blocksRects)
        {
            Vector3 pos = block.transform.localPosition;
            if (pos.x < topLeft.x || (Mathf.Approximately(pos.x, topLeft.x) && pos.y > topLeft.y))
                topLeft = pos;
        }
        return topLeft;
    }

    // public void SnapToGrid(RectTransform tile)
    // {
    //     Vector3 tileCenter = tile.TransformPoint(tile.rect.center);
    //     Vector3 topLeftLocal = GetTopLeftBlockLocalPosition();
    //     Vector3 blocksLocalCenter = GetBlocksLocalCenter();
    //     Vector3 offsetLocal = blocksLocalCenter - topLeftLocal;
    //     Vector3 desiredWorldPos = tileCenter - _blocksRoot.TransformVector(offsetLocal);
    //     _blocksRoot.position = desiredWorldPos;
    // }
    
    public void SnapToGrid(RectTransform tile)
    {
        Vector3 tileCenter = tile.TransformPoint(tile.rect.center);
        Vector3 blocksLocalCenter = GetBlocksLocalCenter();
        Vector3 blocksWorldCenter = _blocksRoot.TransformPoint(blocksLocalCenter);
        Vector3 offset = tileCenter - blocksWorldCenter;
        _blocksRoot.position += offset;

        int restX = _data.Fish.FishData.Width % 2;
        int restY = _data.Fish.FishData.Height % 2;
        
        Vector2 cellSize = _grid.cellSize;
        
        if (Mathf.Approximately(_blocksRoot.rotation.eulerAngles.z % 180f, 90f) || Mathf.Approximately(_blocksRoot.rotation.eulerAngles.z % 180f, -90f))
        {
            // Rotated 90 or 270 degrees, swap x and y
            if (restY == 0)
                _blocksRoot.position -= new Vector3(cellSize.y / 2f, 0, 0);
            if (restX == 0)
                _blocksRoot.position -= new Vector3(0, cellSize.x / 2f, 0);
        }
        else
        {
            if (restX == 0)
                _blocksRoot.position -= new Vector3(cellSize.x / 2f, 0, 0);
            if (restY == 0)
                _blocksRoot.position -= new Vector3(0, cellSize.y / 2f, 0);
        }
    }
}


[Serializable]
public class ItemData
{
    [SerializeField] private Fish _fish;
    public Fish Fish => _fish;
    public Vector2 Position { get; private set; }
    public float Rotation { get; private set; }

    public ItemData(Fish fishData, Vector2 position, float rotation)
    {
        _fish = fishData;
        Position = position;
        Rotation = rotation;
    }
}