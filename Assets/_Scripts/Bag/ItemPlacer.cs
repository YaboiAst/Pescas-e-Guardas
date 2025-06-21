using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPlacer : MonoBehaviour
{
    [SerializeField] private RectTransform _rect;
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _blocksRoot;
    [SerializeField] private List<ItemPlacerBlock> blocksRects;
    [SerializeField] private GameObject _blockPrefab;

    private GridLayoutGroup _grid;

    private bool _isOnGrid = false;

    [SerializeField] private ItemData _data;

    public ItemData GetItemData() => _data;

    public void Initialize(Fish fish)
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
                    blocksRects.Add(itemPlacerBlock);
                _data = new ItemData(fish, block.transform.localPosition, 0);
            }
        }
    }

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

    public void SnapToGrid(RectTransform tile)
    {
        var newX = tile.position.x - (tile.rect.width / 2) + (_blocksRoot.rect.width / 2);
        var newY = tile.position.y - (tile.rect.height / 2) + (_blocksRoot.rect.height / 2);
        _blocksRoot.position = new Vector3(newX, newY, _blocksRoot.position.z);
    }
}


[Serializable]
public class ItemData
{
    [SerializeField] private Fish _fishData;
    public Fish FishData => _fishData;
    public Vector2 Position { get; private set; }
    public float Rotation { get; private set; }

    public ItemData(Fish fishData, Vector2 position, float rotation)
    {
        _fishData = fishData;
        Position = position;
        Rotation = rotation;
    }
}