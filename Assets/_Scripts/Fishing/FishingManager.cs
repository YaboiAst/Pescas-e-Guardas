using UnityEngine;

public class FishingManager
{
    public static FishLootTable CurrentLootTable { get; private set; }
    public static FishItem CurrentFish { get; private set; }

    public static void StartFishing(ref FishLootTable lootTable)
    {
        CurrentLootTable = lootTable;
        CurrentFish = CurrentLootTable.GetLootDropItem();
        
        MinigameManager.Instance.StartMinigame(50, MinigameType.Circle, OnMinigameComplete);
        
        FishingSpotProbabilitiesUI.Instance.GenerateUI(CurrentLootTable);
    }
    
    private static void OnMinigameComplete(MinigameResult result)
    {
        if (result == MinigameResult.Won)
        {
            Debug.Log($"Voce pescou um {CurrentFish.Item.DisplayName} de raridade {CurrentFish.Item.Rarity}");
            Fish fish = new Fish(CurrentFish.Item);
            DiaryManager.Instance.RegisterFish(fish);
        }
        else
        {
            Debug.Log("Voce fracassou e nao pegou o peixe");
        }
        
        FishingSpotProbabilitiesUI.Instance.ClearUI();
        CurrentFish = null;
    }
}
