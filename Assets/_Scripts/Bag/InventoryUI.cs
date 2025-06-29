
using System;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _pointsText;

    private void Start()
    {
        InventoryController.OnItemPlaced += UpdateUI;
    }

    private void OnDestroy()
    {
        InventoryController.OnItemPlaced -= UpdateUI;
    }

    private void UpdateUI() => _pointsText.SetText(InventoryController.Instance.TotalPoints.ToString());
}
