using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI: MonoBehaviour
{
    private List<UpgradeObjectUI> _upgrades = new List<UpgradeObjectUI>();
    [SerializeField] private GameObject _upgradePrefab;
    [SerializeField] private Transform _contentParent;


    private void Start()
    {
        UpgradeManager.OnUpgradeEquipped += UpgradeEquipped;
    }

    private void OnDisable()
    {
        UpgradeManager.OnUpgradeEquipped -= UpgradeEquipped;
    }

    public void UpgradeUnequipped(UpgradeObjectUI upgrade) => _upgrades.Remove(upgrade);

    private void UpgradeEquipped(Upgrade upgrade)
    {
        GameObject upgradeObject = Instantiate(_upgradePrefab, _contentParent);
        UpgradeObjectUI upgradeUI = upgradeObject.GetComponent<UpgradeObjectUI>();
        upgradeUI.SetItem(upgrade);
        _upgrades.Add(upgradeUI);
    }
}