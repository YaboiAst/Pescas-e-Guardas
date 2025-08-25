using UnityEngine;
using Random = UnityEngine.Random;

public class FishingSpot : Spot
{
    [SerializeField] private FishLootTable _lootTable;
    [SerializeField] private Location _locationType;
    private FishItem _currentFish;

    private void OnValidate() => _lootTable.ValidateTable();

    protected override void Start()
    {
        UpdateFishingSpot(_locationType);
        base.Start();
    }

    public void UpdateFishingSpot(Location location)
    {
        //_lootTable.UpdateLootTable(fishData, _fishLocationType);

        FishingAttempts = Random.Range(3, 5);
        _lootTable.ValidateTable();
    }

    public override void Interact()
    {
        FishingManager.StartFishing(this, _lootTable);
        base.Interact();
    }

    public override void OnMinigameComplete(MinigameResult result)
    {
        if (result == MinigameResult.Won)
        {
            Debug.Log($"Voce pescou um {_currentFish.Item.DisplayName} de raridade {_currentFish.Item.Rarity}");
            Fish fish = new Fish(_currentFish.Item);
            DiaryManager.Instance.RegisterFish(fish);
            InventoryController.Instance.CreateItem(fish);
        }
        else
        {

        }
    }

    public override void UseFishingAttempt()
    {
        _currentFish = _lootTable.GetLootDropItem();
        base.UseFishingAttempt();
    }
}