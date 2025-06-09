using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    // Save slot data
    public string SaveName;
    public long LastUpdated;
    public string LastSession;
    
    // Date and Time Data
    public TimeSettings TimeSettings;
    
    // Diary Data
    public List<EntryData> AllEntries;

    public GameData()
    {
        // Initialize Data
        AllEntries = new List<EntryData>();
        TimeSettings = new TimeSettings(1,12,0);
    }
}