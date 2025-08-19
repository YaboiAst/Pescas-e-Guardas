using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static readonly List<Upgrade> Equipped = new List<Upgrade>();

    public static void Equip(Upgrade upgrade)
    {
        if (!upgrade || Equipped.Contains(upgrade)) return;
        Equipped.Add(upgrade);
        upgrade.EquipEffect();
        if (InventoryPoints.Instance)
            InventoryPoints.Instance.RegisterUpgrade(upgrade);
    }

    public static void Unequip(Upgrade upgrade)
    {
        if (!upgrade || !Equipped.Remove(upgrade)) return;
        upgrade.UnequipEffect();
        if (InventoryPoints.Instance)
            InventoryPoints.Instance.UnregisterUpgrade(upgrade);
    }
}

