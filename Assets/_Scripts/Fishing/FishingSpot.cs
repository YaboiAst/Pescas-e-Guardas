using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishingSpot : MonoBehaviour
{
    [SerializeField] private MinigameType _minigameType;
    
    [SerializeField] private FishLootTable _lootTable;
    [SerializeField] private Location _locationType;
    private Interactable _interactable;

    private FishData _currentFish;

    private int _fishingAttempts = 3;


    private void Start()
    {
    }

    private void OnValidate() => _lootTable.ValidateTable();

    public void UpdateFishingSpot(Location location)
    {
        //_lootTable.UpdateLootTable(fishData, _fishLocationType);
        _fishingAttempts = Random.Range(3, 5);
        _lootTable.ValidateTable();
    }

    public void UseFishingAttempt()
    {
        _fishingAttempts--;
        if (_fishingAttempts < 0)
            _fishingAttempts = 0;
    }

    public bool CanFish() => _fishingAttempts > 0;

    public void Interact()
    {
        FishingManager.StartFishing(_lootTable, this);
    }
    public void DestroySpot()
    {
        _interactable.RemoveFromRange();
        ObjectPoolManager.ReturnObjectToPool(this.gameObject);
    }
}
