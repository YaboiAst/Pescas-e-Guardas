using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiaryManager : MonoBehaviour, IDataPersistence
{
    public static DiaryManager Instance { get; private set; }
    private List<EntryData> _allEntries = new List<EntryData>();

    private DiaryUI _diaryUI;

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        _diaryUI = GetComponent<DiaryUI>();
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

    public void LoadData(GameData data)
    {
        if (data.AllEntries.Count < 1)
        {
            foreach (FishData fishData in FishManager.AllFishes)
            {
                EntryData entry = new EntryData(fishData);
                _allEntries.Add(entry);
            }
        }
        else
        {
            _allEntries = data.AllEntries;
        }
        
        _diaryUI.BindDatas(_allEntries);
    }

    public void SaveData(GameData data)
    {
        data.AllEntries = _allEntries;
    }
}

[Serializable]
public class EntryData
{
    public FishData FishData;
    public string UniqueID => FishData.UniqueID;
    public int TimesCaught;
    public float HighestWeight;
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