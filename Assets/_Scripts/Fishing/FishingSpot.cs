using UnityEngine;

public class FishingSpot : MonoBehaviour
{
    [SerializeField] private MinigameType _minigameType;
    
    [SerializeField] private FishLootTable _lootTable;
    [SerializeField] private Location _locationType;

    private FishData _currentFish;

    private void OnValidate() => _lootTable.ValidateTable();

    public void UpdateFishingSpot(Location location)
    {
        //_lootTable.UpdateLootTable(fishData, _fishLocationType);
    }
    
    public void Interact()
    {
        _lootTable.ValidateTable();
        FishingManager.StartFishing(ref _lootTable);
    }
}
