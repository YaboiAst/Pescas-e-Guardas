using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup), typeof(RectTransform))]
public class GridGeneration : MonoBehaviour
{
    public static Vector2Int GRID_RATIO = new(3, 4);
    [SerializeField] private BagData defaultBag;
    
    // Prefab Refs
    [SerializeField] private GridTile tilePrefab;
    
    // Component Refs
    [SerializeField] private RectTransform bagRoot;
    [SerializeField] private GridLayoutGroup bagLayoutGrid;
    [SerializeField] private Image bagMaskHolder;

    private int _bagWidth; 
    private int _bagHeight;

    public Vector2 GridRectSize => bagLayoutGrid.cellSize; 

    public void SetBag(BagData newBag)
    {
        newBag ??= defaultBag;
        
        ClearBag();

        _bagWidth  = GRID_RATIO.x * newBag.scale;
        _bagHeight = GRID_RATIO.y * newBag.scale;
        ResizeBag();
        
        var inventory = InventoryController.Instance;
        inventory.InitializeGrid();
        for (var i = 0; i < (_bagWidth * _bagHeight); i++)
        {
            var tileInstance = Instantiate(tilePrefab, bagRoot);
            tileInstance.InitializeTile(i / _bagWidth, (i % _bagWidth));
            inventory.AddTile(tileInstance);
        }

        if (newBag.bagMask == null)
        {
            bagMaskHolder.color = Color.white - Color.black;
            bagMaskHolder.sprite = null;
            bagMaskHolder.enabled = false;
            return;
        }
        bagMaskHolder.alphaHitTestMinimumThreshold = 0.1f;
        bagMaskHolder.sprite = newBag.bagMask;
        bagMaskHolder.color = Color.white;
    }

    public void ClearBag()
    {
        if (bagRoot.childCount <= 0)
            return;
        
        foreach (Transform child in bagRoot.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ResizeBag()
    {
        var gridWidth = Mathf.CeilToInt(this.bagRoot.rect.width / _bagWidth);
        var gridHeigth = Mathf.CeilToInt(this.bagRoot.rect.height / _bagHeight);

        bagLayoutGrid ??= bagRoot.GetComponent<GridLayoutGroup>();
        bagLayoutGrid.cellSize = new Vector2(gridWidth, gridHeigth);
    }
}
