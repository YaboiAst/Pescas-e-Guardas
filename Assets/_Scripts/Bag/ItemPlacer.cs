using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{
    [SerializeField] private RectTransform blocksRoot;
    [SerializeField] private List<ItemPlacerBlock> blocksRects;

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

    public void SnapToGrid(RectTransform tile)
    {
        var newX = tile.position.x - (tile.rect.width / 2) + (blocksRoot.rect.width / 2);
        var newY = tile.position.y - (tile.rect.height / 2) + (blocksRoot.rect.height / 2);
        blocksRoot.position = new Vector3(newX, newY, blocksRoot.position.z);
    }
}
