using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GridTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector2Int _coord;
    public Vector2Int GetCoord() => _coord;

    private RectTransform rectT;
    private Rect _gridRect;
    private Image _tileVisual;
    private Color defaultColor;
    
    public void InitializeTile(int xIndex, int yIndex)
    {
        _coord = new Vector2Int(xIndex, yIndex);
        InventoryController.CheckOverlap.AddListener(CheckOverlap);
        InventoryController.ClearGrid.AddListener(ClearTile);
        
        _tileVisual = GetComponent<Image>();
        defaultColor = _tileVisual.color;
    }

    public void CheckOverlap(ItemPlacer item)
    {
        _tileVisual.color = defaultColor;

        if (rectT is null)
        {
            rectT = GetComponent<RectTransform>();
            _gridRect = new Rect(rectT.position.x, rectT.position.y, rectT.rect.width, rectT.rect.height);
        }
        
        foreach (var itemRect in item.GetRects())
        {
            if (!_gridRect.Overlaps(itemRect)) continue;
            
            _tileVisual.color = Color.red;
            return;
        }
    }

    private void ClearTile()
    {
        _tileVisual.color = defaultColor;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!eventData.dragging) return;
        
        var item = eventData.pointerDrag.GetComponent<ItemPlacer>();
        if (rectT is null)
        {
            rectT = GetComponent<RectTransform>();
            _gridRect = new Rect(rectT.position.x, rectT.position.y, rectT.rect.width, rectT.rect.height);
        }
        
        // Snap to grid
        item.SnapToGrid(this.rectT);
        
        // Overlap check
        InventoryController.CheckOverlap?.Invoke(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.dragging) return;
        InventoryController.ClearGrid?.Invoke();
    }
}
