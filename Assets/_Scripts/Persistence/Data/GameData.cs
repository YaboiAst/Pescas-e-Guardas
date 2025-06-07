using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    // Save slot data
    public string SaveName;
    public long LastUpdated;
    public string LastSession;
    
    // Date and Time Data
    public int CurrentHour;
    public int CurrentMinute;
    public float CurrentSeconds;
    public int CurrentDay;
    public int CurrentMonth;
    public int CurrentYear;
    
    // Diary Data
    public List<EntryData> AllEntries;

    public GameData()
    {
        // Initialize Data
        AllEntries = new List<EntryData>();
        
        CurrentHour = 8;
        CurrentMinute = 0;
        CurrentSeconds = 0;
        CurrentDay = 1;
        CurrentMonth = 1;
        CurrentYear = 1;
    }
}