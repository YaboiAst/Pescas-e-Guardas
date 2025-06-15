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
    
    // Money Data
    public int MoneyAmount;
    
    // Date and Time Data
    public TimeSettings TimeSettings;
    
    // Diary Data
    public List<EntryData> AllEntries;

    public GameData()
    {
        // Initialize Data
        this.AllEntries = new List<EntryData>();
        this.BoatData = new BoatData();
        this.MoneyAmount = 1000;
        this.TimeSettings = new TimeSettings(1,12,0);
        this.BoatData = new BoatData();
    }
}