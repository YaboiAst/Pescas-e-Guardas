using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;
public class ShopController : CanvasController
{
    [SerializeField] private GameObject _shopItemPrefab;
    [SerializeField] private Transform _itemsParent;
    [SerializeField] private List<ShopItemUI> _items = new List<ShopItemUI>();
    [SerializeField] private int _itemsToShow = 6;

    public static bool ShopOpen;

    private void Start() => QuestManager.OnFinishQuest.AddListener(PopulateShop);

    //private void OnEnable() => PopulateShop();

    [Button("Populate Shop")]
    private void PopulateShop()
    {
        foreach (ShopItemUI item in _items)
        {
            Destroy(item.gameObject);
        }
        _items.Clear();

        IEnumerable<Upgrade> upgrades = UpgradeDatabase.GetRandomUpgrades(_itemsToShow);
        foreach (Upgrade upgrade in upgrades)
        {
            GameObject itemGO = Instantiate(_shopItemPrefab, _itemsParent);
            ShopItemUI itemUI = itemGO.GetComponent<ShopItemUI>();
            itemUI.SetItem(upgrade);
            _items.Add(itemUI);
        }
    }

    public void Interact()
    {
        ShowCanvas();
        //PopulateShop();
    }

    protected override void ShowCanvas()
    {
        base.ShowCanvas();
        ShopOpen = true;
    }

    protected override void HideCanvas()
    {
        base.HideCanvas();
        ShopOpen = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsOpen)
            HideCanvas();
    }

}
public static class UpgradeDatabase
{
    public static IEnumerable<Upgrade> GetRandomUpgrades(int amount)
    {
        Upgrade[] allUpgrades = Resources.LoadAll<Upgrade>("Upgrades");
        List<Upgrade> selectedUpgrades = new List<Upgrade>();
        HashSet<int> usedIndices = new HashSet<int>();

        while (selectedUpgrades.Count < amount && usedIndices.Count < allUpgrades.Length)
        {
            int randomIndex = Random.Range(0, allUpgrades.Length);
            if (usedIndices.Add(randomIndex))
                selectedUpgrades.Add(allUpgrades[randomIndex]);
        }

        return selectedUpgrades;
    }
}