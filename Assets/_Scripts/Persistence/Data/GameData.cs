using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    // Save slot data
    public string SaveName;
    public long LastUpdated;
    public string LastSession;
    
    // Boat Data
    public BoatData BoatData;
    
    // Date and Time Data
    public TimeSettings TimeSettings;
    
    // Diary Data
    public List<EntryData> AllEntries;

    public GameData()
    {
        // Initialize Data
        this.AllEntries = new List<EntryData>();
        this.TimeSettings = new TimeSettings(1,12,0);
        this.BoatData = new BoatData();
    }
}