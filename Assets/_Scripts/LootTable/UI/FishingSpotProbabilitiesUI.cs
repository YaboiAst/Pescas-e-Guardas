using Unity.Mathematics;
using UnityEngine;

public class FishingSpotProbabilitiesUI : MonoBehaviour
{
    [SerializeField] private Transform _prefabParent;
    [SerializeField] private GameObject _probabilityPrefab;
    
    public void GenerateUI(FishLootTable fishes)
    {
        foreach (FishItem fish in fishes.LootDropItems)
        {
            FishInfoUI fishInfo = ObjectPoolManager.SpawnGameObject(_probabilityPrefab, _prefabParent, quaternion.identity).GetComponent<FishInfoUI>();
            fishInfo.SetupFishInfo(fish, DiaryManager.Instance.IsDiscovered(fish.Item));
        }
    }

    public void ClearUI()
    {
        for (int i = _prefabParent.childCount - 1; i >= 0; i--) 
            ObjectPoolManager.ReturnObjectToPool(_prefabParent.GetChild(i).gameObject);
    }
}