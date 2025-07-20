using UnityEngine;
using UnityEngine.Events;

public class FishingManager
{
    public static FishLootTable CurrentLootTable { get; private set; }
    public static FishItem CurrentFish { get; private set; }

    public static readonly UnityEvent OnFishComplete = new();

    public static void StartFishing(ref FishLootTable lootTable)
    {
        CurrentLootTable = lootTable;
        CommonMinigameUI.Instance.ShowUI(CurrentLootTable);
    }

    public static void PrepFishMinigame()
    {
        MinigameManager.Instance.PrepMinigame(50, MinigameType.Circle, OnMinigameComplete);
    }
    
    public static void PlayFishMinigame()
    {
        CurrentFish = CurrentLootTable.GetLootDropItem();
        MinigameManager.Instance.PlayMinigame();
    }

    public static void CloseFishMinigame()
    {
        MinigameManager.Instance.CloseMinigame();
    }
    
    private static void OnMinigameComplete(MinigameResult result)
    {
        if (result == MinigameResult.Won)
        {
            Debug.Log($"Voce pescou um {CurrentFish.Item.DisplayName} de raridade {CurrentFish.Item.Rarity}");
            Fish fish = new Fish(CurrentFish.Item);
            DiaryManager.Instance.RegisterFish(fish);
            InventoryController.Instance.CreateItem(fish);
            OnFishComplete?.Invoke();
        }
        else
        {
            Debug.Log("Voce fracassou e nao pegou o peixe");
        }
        
        CurrentFish = null;
    }
}
