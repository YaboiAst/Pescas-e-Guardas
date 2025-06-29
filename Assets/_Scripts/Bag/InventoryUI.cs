using System;
using TMPro;
using UnityEngine;

public class InventoryUI : CanvasController
{
    [SerializeField] private TMP_Text _pointsText;

    private void Start()
    {
        InventoryController.OnItemPlaced += UpdateUI;
        HideCanvas();
    }

    private void OnDestroy()
    {
        InventoryController.OnItemPlaced -= UpdateUI;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            ToggleCanvas();
    }
    private void UpdateUI() => _pointsText.SetText(InventoryController.Instance.TotalPoints.ToString());

    public void ShowInventory() => ShowCanvas();

}
