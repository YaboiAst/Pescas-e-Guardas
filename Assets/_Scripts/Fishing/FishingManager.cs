using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class FishingManager
{
    public static FishLootTable CurrentLootTable { get; private set; }
    public static FishItem CurrentFish { get; private set; }

    private static Spot _spot;

    public static readonly UnityEvent OnFishComplete = new();
    public static float LuckChance = 0;


    public static void StartFishing(Spot fishingSpot, FishLootTable lootTable = null)
    {
        _spot = fishingSpot;
        CurrentLootTable = lootTable;
        CommonMinigameUI.Instance.ShowUI(CurrentLootTable);
    }

    public static void PrepFishMinigame()
    {
        int randomMinigame = Random.Range(0, 2);
        MinigameType type = randomMinigame switch
        {
            0 => MinigameType.Key,
            1 => MinigameType.Circle,
            _ => MinigameType.Key
        };

        int difficulty = Random.Range(70, 85);

        //Debug.Log(difficulty);

        MinigameManager.Instance.PrepMinigame(difficulty, type, OnMinigameComplete);
    }

    public static bool PlayFishMinigame()
    {
        if (_spot.CanFish())
        {
            _spot.UseFishingAttempt();
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
        OnFishComplete?.Invoke();
        _spot.OnMinigameComplete(result);

        // if (result == MinigameResult.Won)
        // else
        //     Debug.Log("Voce fracassou e nao pegou o peixe");

        if (!_spot.CanFish())
        {
            CloseFishMinigame();
            CommonMinigameUI.Instance.HideOnlyMinigameUI(triggerEvent: _spot.IsTreasureSpot());
            _spot.DestroySpot();
            _spot = null;
        }
        else
        {
            CommonMinigameUI.Instance.GenerateProbabilitiesUI(CurrentLootTable);
        }

        CurrentFish = null;
    }
}
