using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiaryManager : MonoBehaviour
{
    public static DiaryManager Instance { get; private set; }
    private readonly HashSet<EntryData> _allEntries = new HashSet<EntryData>();
    public event Action<HashSet<EntryData>> OnEntriesLoaded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
    }

    private IEnumerator Start()
    {
        foreach (FishData fishData in FishManager.AllFishes)
        {
            EntryData entry = new EntryData(fishData);
            _allEntries.Add(entry);
        }
        
        yield return null;
        
        OnEntriesLoaded?.Invoke(_allEntries);
    }

    public bool IsDiscovered(FishData fish)
    {
        EntryData data = _allEntries.FirstOrDefault(t => t.UniqueID == fish.UniqueID);

        return data is { IsDiscovered: true };
    }

    public void RegisterFish(Fish fish)
    {
        EntryData data = _allEntries.FirstOrDefault(t => t.UniqueID == fish.FishData.UniqueID);

        if (data != null)
            data.FishCaught(fish);
        else
            Debug.LogError("The fish you caught isn't in the Diary");
    }
}

[Serializable]
public class EntryData
{
    public FishData FishData { get; private set; }
    public string UniqueID => FishData.UniqueID;
    public int TimesCaught { get; private set; }
    public float HighestWeight { get; private set; }
    public bool IsDiscovered => TimesCaught > 0;

    public event Action OnEntryUpdated; 

    public EntryData(FishData fishData)
    {
        FishData = fishData;
        TimesCaught = 0;
        HighestWeight = 0;
    }

    public void FishCaught(Fish fish)
    {
        TimesCaught++;
        if (fish.Weight > HighestWeight)
        {
            HighestWeight = fish.Weight;
            OnEntryUpdated?.Invoke();
        } 
        
    }
}