using NaughtyAttributes;
using UnityEngine;

public class TestScript : MonoBehaviour
{
   [SerializeField] private Fish _fish;

   [Button]
   public void SpawnFish() => InventoryController.Instance.CreateItem(_fish);
}
