using NaughtyAttributes;
using UnityEngine;

public class TestScript : MonoBehaviour
{
   [SerializeField] private Upgrade _upgrade;

   [Button]
   public void EquipUpgrade() => UpgradeManager.Equip(_upgrade);
}
