using System;
using TMPro;
using UnityEngine;

public class InventoryUI : CanvasController
{
    [SerializeField] private TMP_Text _pointsText;

    private void Start()
    {
        InventoryPoints.OnPointsUpdated += UpdateUI;
        HideCanvas();
    }

    private void OnDestroy()
    {
        InventoryPoints.OnPointsUpdated -= UpdateUI;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
            ToggleCanvas();
    }
    private void UpdateUI()
    {
        _pointsText.SetText($"Pontuação: {InventoryPoints.Instance.TotalPoints}");
    }

    public void ShowInventory() => ShowCanvas();
    public void HideInventory() => HideCanvas();

}
