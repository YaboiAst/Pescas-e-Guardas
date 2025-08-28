using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static readonly List<Upgrade> Equipped = new List<Upgrade>();
    private static int _maxUpgrades = 5;

    public static bool Equip(Upgrade upgrade)
    {
        if (!upgrade || Equipped.Contains(upgrade) || Equipped.Count >= _maxUpgrades)
            return false;
        Equipped.Add(upgrade);
        upgrade.EquipEffect();
        if (InventoryPoints.Instance)
            InventoryPoints.Instance.RegisterUpgrade(upgrade);

        return true;
    }

    public static void Unequip(Upgrade upgrade)
    {
        if (!upgrade || !Equipped.Remove(upgrade))
            return;

        upgrade.UnequipEffect();
        if (InventoryPoints.Instance)
            InventoryPoints.Instance.UnregisterUpgrade(upgrade);
    }
}

