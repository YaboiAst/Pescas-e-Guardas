using System.Collections.Generic;

public class DataHandler
{
   public virtual void SaveSettings(SettingsData settingsData) { }
   
   public virtual SettingsData LoadSettings() { return new SettingsData(); }

   public virtual void Save(GameData gameData, string selectedProfileId) { }

   public virtual GameData Load(string selectedProfileId, bool allowRestoreFromBackup = true) { return new GameData(); }

   public virtual void Delete(string profileId) { }

   public virtual string GetMostRecentlyUpdatedProfileId() { return string.Empty; }

   public virtual Dictionary<string, GameData> LoadAllProfiles() { return null; }

   public virtual void DeleteAll() { }
}
