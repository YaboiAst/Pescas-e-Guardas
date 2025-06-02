using UnityEngine;

public class FishingSpotInteraction : MonoBehaviour
{
    [SerializeField] private MinigameType _minigameType;
    [SerializeField] private FishLootTable _lootTable;
    [SerializeField] private FishLocation _fishLocationType;

    private FishData _currentFish;

    private void OnValidate() => _lootTable.ValidateTable();

    public void Interact()
    {
        _lootTable.ValidateTable();
        FishingManager.StartFishing(ref _lootTable);
    }
}
