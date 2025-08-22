using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup), typeof(RectTransform))]
public class GridGeneration : MonoBehaviour
{
    // Bag Data
    // TODO: Make it private after validation
    [SerializeField] private BagData currentBag;
    public static Vector2Int GRID_RATIO = new(3, 4);
    
    // Prefab Refs
    [SerializeField] private GridTile tilePrefab;
    
    // Component Refs
    [SerializeField] private RectTransform bagRoot;
    [SerializeField] private GridLayoutGroup bagLayoutGrid;
    [SerializeField] private Image bagMaskHolder;

    private int BagWidth; 
    private int BagHeight;

    public Vector2 gridRectSize => bagLayoutGrid.cellSize; 

    public void SetBag(BagData newBag)
    {
        if (newBag is null)
            return;
        ClearBag();

        BagWidth  = GRID_RATIO.x * newBag.scale;
        BagHeight = GRID_RATIO.y * newBag.scale;

        ResizeBag();
        
        var inventory = InventoryController.Instance;
        inventory.InitializeGrid();
        for (var i = 0; i < (BagWidth * BagHeight); i++)
        {
            var tileInstance = Instantiate(tilePrefab, bagRoot);
            tileInstance.InitializeTile(i / BagWidth, (i % BagWidth));
            inventory.AddTile(tileInstance);
        }

        bagMaskHolder.alphaHitTestMinimumThreshold = 0.1f;
        if (newBag.bagMask == null)
        {
            bagMaskHolder.color = Color.white - Color.black;
            bagMaskHolder.sprite = null;
            return;
        }
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
        var gridWidth = Mathf.CeilToInt(this.bagRoot.rect.width / BagWidth);
        var gridHeigth = Mathf.CeilToInt(this.bagRoot.rect.height / BagHeight);

        bagLayoutGrid ??= bagRoot.GetComponent<GridLayoutGroup>();
        bagLayoutGrid.cellSize = new Vector2(gridWidth, gridHeigth);
    }
}
