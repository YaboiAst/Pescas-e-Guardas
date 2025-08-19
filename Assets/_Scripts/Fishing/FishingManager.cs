using UnityEngine;
using UnityEngine.Events;

public class FishingManager
{
    public static FishLootTable CurrentLootTable { get; private set; }
    public static FishItem CurrentFish { get; private set; }

    private static FishingSpot _spot;

    public static readonly UnityEvent OnFishComplete = new();

    public static void StartFishing(FishLootTable lootTable, FishingSpot fishingSpot)
    {
        CurrentLootTable = lootTable;
        _spot = fishingSpot;
        CommonMinigameUI.Instance.ShowUI(CurrentLootTable);
    }

    public static void PrepFishMinigame()
    {
        MinigameManager.Instance.PrepMinigame(50, MinigameType.Circle, OnMinigameComplete);
    }
    
    public static bool PlayFishMinigame()
    {
        if (_spot.CanFish())
        {
            _spot.UseFishingAttempt();
            CurrentFish = CurrentLootTable.GetLootDropItem();
            MinigameManager.Instance.PlayMinigame();
            return true;
        }

        return false;
    }

    public static void CloseFishMinigame()
    {
        MinigameManager.Instance.CloseMinigame();
        if (_spot)
        {
            _spot.ShowInteraction();
        }
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

        if (!_spot.CanFish())
        {
            CloseFishMinigame();
            CommonMinigameUI.Instance.HideOnlyMinigameUI();
            _spot.DestroySpot();
            _spot = null;
        }

        CurrentFish = null;
    }
}
