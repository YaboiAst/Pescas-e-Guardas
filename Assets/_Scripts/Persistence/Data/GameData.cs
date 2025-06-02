using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    // Save slot data
    public string SaveName;
    public long LastUpdated;
    public string LastSession;
    
    
    
    // Diary Data
    public List<EntryData> AllEntries;

    public GameData()
    {
        // Initialize Data
        AllEntries = new List<EntryData>();
    }
}