using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector2Int _coord;
    public Vector2Int GetCoord() => _coord;

    private ItemPlacer _itemInTile = null;
    public void SetItemInTile(ItemPlacer i) => _itemInTile = i; 
    public bool IsOccupied => _itemInTile is not null;
    
    private RectTransform _rectT;
    private Rect _gridRect;
    private Image _tileVisual;
    private Color _defaultColor;
    
    public void InitializeTile(int xIndex, int yIndex)
    {
        _coord = new Vector2Int(xIndex, yIndex);
        InventoryController.CheckOverlap.AddListener(CheckOverlap);
        InventoryController.ClearGrid.AddListener(ClearTile);
        DraggableItem.OnBeginDrag.AddListener(UpdateTile);
        
        _tileVisual = GetComponent<Image>();
        _defaultColor = _tileVisual.color;
    }
    
    // METHODS CALLED BY EVENTS
    public void CheckOverlap(ItemPlacer item)
    {
        _tileVisual.color = _defaultColor;

        if (_rectT is null)
        {
            _rectT = GetComponent<RectTransform>();
            _gridRect = new Rect(_rectT.position.x, _rectT.position.y, _rectT.rect.width, _rectT.rect.height);
        }
        
        foreach (var itemRect in item.GetRects())
        {
            if (!_gridRect.Overlaps(itemRect)) continue;
            
            InventoryController.Instance.AddToBuffer(this);
            return;
        }
    }
    
    private void UpdateTile(ItemPlacer item)
    {
        if (this._itemInTile != item) return;
        this._itemInTile = null;
    }

    private void ClearTile()
    {
        _tileVisual.color = _defaultColor;
    }
    
    // METHODS CALLED BY USER INTERACTION
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging) return;
        
        var item = eventData.pointerDrag.GetComponent<ItemPlacer>();
        if (_rectT is null)
        {
            _rectT = GetComponent<RectTransform>();
            _gridRect = new Rect(_rectT.position.x, _rectT.position.y, _rectT.rect.width, _rectT.rect.height);
        }
 
        // Snap to grid
        item.SnapToGrid(this._rectT);
        
        // Overlap check
        InventoryController.CheckGrid(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.dragging) return;
        InventoryController.ClearGrid?.Invoke();
    }
    
    // UTILS
    public void PaintTile(Color newColor)
    {
        _tileVisual.color = newColor;
    }
}
