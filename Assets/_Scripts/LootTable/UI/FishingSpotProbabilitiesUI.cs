using Unity.Mathematics;
using UnityEngine;

public class FishingSpotProbabilitiesUI : CanvasController
{
    [SerializeField] private Transform _prefabParent;
    [SerializeField] private GameObject _probabilityPrefab;

    public static FishingSpotProbabilitiesUI Instance;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start() => HideCanvas();

    public void GenerateUI(FishLootTable fishes)
    {
        ShowCanvas();        
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
        
        HideCanvas();
    }
}