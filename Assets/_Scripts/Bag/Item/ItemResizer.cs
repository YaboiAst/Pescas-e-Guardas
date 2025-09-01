using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemResizer : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup itemLayoutGrid;
    [SerializeField] private RectTransform rect;
    public void Resize(int resizedCell, Vector2 fishSize)
    {
        itemLayoutGrid.cellSize = Vector2.one * resizedCell;
        rect.sizeDelta = new Vector2(
            fishSize.x * itemLayoutGrid.cellSize.x,
            fishSize.y * itemLayoutGrid.cellSize.y);
    }
}
