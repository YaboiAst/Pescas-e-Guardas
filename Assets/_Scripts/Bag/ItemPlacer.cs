using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemPlacer : MonoBehaviour
{
    [SerializeField] private RectTransform blocksRoot;
    [SerializeField] private List<ItemPlacerBlock> blocksRects;

    private bool _isOnGrid = false;

    [SerializeField] private ItemData _data;

    public ItemData GetItemData() => _data;

    public void SetItemData(ItemData data) => _data = data;
    
    public void SetPositionStatus(bool status) => _isOnGrid = status;

    public bool GetPositionStatus() => _isOnGrid;

    public RectTransform GetRoot()
    {
        return blocksRoot;
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
        var newX = tile.position.x - (tile.rect.width / 2) + (blocksRoot.rect.width / 2);
        var newY = tile.position.y - (tile.rect.height / 2) + (blocksRoot.rect.height / 2);
        blocksRoot.position = new Vector3(newX, newY, blocksRoot.position.z);
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