using System.Collections.Generic;
using UnityEngine;
using System;

public class WebDataHandler : DataHandler
{

    private string _profileKeyPrefix = "profile_";
    private string _gameDataKeySuffix = "_gamedata";
    private string _settingsKey = "settingsdata"; 

    private bool _useEncryption = false;
    private readonly string _encryptionCodeWord = "kj23f3aav";
    private readonly string _backupKeySuffix = "_backup";

    private const string AllProfilesMasterKey = "ALL_PROFILE_IDS_MASTER_LIST";

    public WebDataHandler(string dataDirPath, string dataFileName, string settingsFileName, bool useEncryption)
    {
        this._profileKeyPrefix = SanitizeKeyPart(dataDirPath) + "profile_";
        this._gameDataKeySuffix = "_" + SanitizeKeyPart(dataFileName);
        this._settingsKey = SanitizeKeyPart(settingsFileName);
        this._useEncryption = useEncryption;

        if (string.IsNullOrEmpty(dataFileName))
        {
            Debug.LogError("dataFileName cannot be empty for WebDataHandler.");
            this._gameDataKeySuffix = "_gamedata_default"; // Fallback
        }
        if (string.IsNullOrEmpty(settingsFileName))
        {
            Debug.LogError("settingsFileName cannot be empty for WebDataHandler.");
            this._settingsKey = "settingsdata_default"; // Fallback
        }
    }

    private string SanitizeKeyPart(string part)
    {
        if (string.IsNullOrEmpty(part)) return "";
        return part.Replace(" ", "_").Replace("/", "_").Replace("\\", "_") + "_";
    }
    
    private string GetProfileGameDataKey(string profileId)
    {
        if (string.IsNullOrEmpty(profileId))
        {
            Debug.LogError("Profile ID cannot be null or empty when getting a key.");
            return null;
        }
        return _profileKeyPrefix + profileId + _gameDataKeySuffix;
    }
    
    public override GameData Load(string profileId, bool allowRestoreFromBackup = true)
    {
        if (string.IsNullOrEmpty(profileId))
        {
            Debug.LogWarning("Load called with null or empty profileId.");
            return null;
        }

        string prefsKey = GetProfileGameDataKey(profileId);
        if (prefsKey == null) return null;

        GameData loadedData = null;

        if (PlayerPrefs.HasKey(prefsKey))
        {
            try
            {
                string dataToLoad = PlayerPrefs.GetString(prefsKey);

                if (string.IsNullOrEmpty(dataToLoad))
                {
                    Debug.LogWarning($"Data for key '{prefsKey}' is empty. This might indicate a save issue or deletion.");
                    if (allowRestoreFromBackup) { /* proceed to backup logic below */ }
                    else { return null; }
                }
                else if (_useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
                if (loadedData == null && !string.IsNullOrEmpty(dataToLoad))
                {
                     Debug.LogWarning($"JsonUtility.FromJson returned null for key '{prefsKey}' even though data was present. Data: {dataToLoad.Substring(0, Math.Min(dataToLoad.Length,100))}");
                }

            }
            catch (Exception e)
            {
                Debug.LogError($"Exception when trying to load or decrypt data from PlayerPrefs key: {prefsKey}\n{e}");
                if (allowRestoreFromBackup)
                {
                    Debug.LogWarning($"Attempting to roll back for key: {prefsKey}");
                    bool rollbackSuccess = AttemptRollback(prefsKey);
                    if (rollbackSuccess) 
                        loadedData = Load(profileId, false);
                }
            }
        }
        else
        {
            if (allowRestoreFromBackup)
            {
                Debug.LogWarning($"No primary data found for key '{prefsKey}'. Attempting to load from backup.");
                string backupPrefsKey = prefsKey + _backupKeySuffix;
                if (PlayerPrefs.HasKey(backupPrefsKey))
                {
                     bool rollbackSuccess = AttemptRollback(prefsKey); // This will copy backup to primary
                     if (rollbackSuccess)
                     {
                        loadedData = Load(profileId, false); // Load the newly restored primary data
                     }
                }
                else
                {
                    Debug.Log($"No primary data or backup found for profileId: {profileId} with key: {prefsKey}");
                }
            }
            else
            {
                 Debug.Log($"No data found for profileId: {profileId} with key: {prefsKey} (backup not allowed).");
            }
        }
        return loadedData;
    }
    
    public override void Save(GameData data, string profileId)
    {
        if (string.IsNullOrEmpty(profileId))
        {
            Debug.LogError("Save called with null or empty profileId.");
            return;
        }
        if (data == null)
        {
            Debug.LogError($"Save called with null GameData for profileId: {profileId}.");
            return;
        }

        string prefsKey = GetProfileGameDataKey(profileId);
        if (prefsKey == null) return;
        string backupPrefsKey = prefsKey + _backupKeySuffix;

        try
        {
            string dataToStore = JsonUtility.ToJson(data, true);

            if (string.IsNullOrEmpty(dataToStore) || dataToStore == "{}" && JsonUtility.ToJson(new GameData(), true) != "{}") // Check if serialization produced meaningful data
            {
                Debug.LogError($"JsonUtility.ToJson produced empty or minimal string for profileId: {profileId}. Aborting save. Data: {data}");
                return;
            }

            if (_useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }
            
            PlayerPrefs.SetString(prefsKey, dataToStore);
            
            PlayerPrefs.SetString(backupPrefsKey, dataToStore);
            
            PlayerPrefs.Save(); // Crucial for WebGL to persist data immediately.

            Debug.Log($"Data saved to PlayerPrefs key: {prefsKey} and backup to {backupPrefsKey}");
            
            AddProfileIdToMasterList(profileId);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error occurred when trying to save data to PlayerPrefs key: {prefsKey}\n{e}");
        }
    }
    
    public override void SaveSettings(SettingsData data)
    {
        if (data == null)
        {
            Debug.LogError("SaveSettings called with null SettingsData.");
            return;
        }
        try
        {
            string dataToStore = JsonUtility.ToJson(data, true);

            PlayerPrefs.SetString(_settingsKey, dataToStore);
            PlayerPrefs.Save(); // Persist
            Debug.Log($"Settings saved to PlayerPrefs key: {_settingsKey}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error occurred when trying to save settings to PlayerPrefs key: {_settingsKey}\n{e}");
        }
    }
    
    public override SettingsData LoadSettings()
    {
        SettingsData loadedData = null;
        if (PlayerPrefs.HasKey(_settingsKey))
        {
            try
            {
                string dataToLoad = PlayerPrefs.GetString(_settingsKey);
                
                if (!string.IsNullOrEmpty(dataToLoad))
                {
                    loadedData = JsonUtility.FromJson<SettingsData>(dataToLoad);
                }

                if (loadedData == null)
                {
                    Debug.LogWarning($"Could not load settings from key '{_settingsKey}'. Returning new/default settings.");
                    loadedData = new SettingsData(); // Return a default/new instance
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error occurred when trying to load settings from PlayerPrefs key: {_settingsKey}\n{e}");
                loadedData = new SettingsData(); // Return a default/new instance on error
            }
        }
        else
        {
            Debug.Log($"No settings found at key '{_settingsKey}'. Returning new/default settings.");
            loadedData = new SettingsData(); // Return a default/new instance if no key exists
        }
        return loadedData;
    }

    public override void Delete(string profileId)
    {
        if (string.IsNullOrEmpty(profileId))
        {
            Debug.LogWarning("Delete called with null or empty profileId.");
            return;
        }

        string prefsKey = GetProfileGameDataKey(profileId);
        if (prefsKey == null) return;
        string backupPrefsKey = prefsKey + _backupKeySuffix;

        bool deletedSomething = false;
        if (PlayerPrefs.HasKey(prefsKey))
        {
            PlayerPrefs.DeleteKey(prefsKey);
            Debug.Log($"Deleted primary data for key: {prefsKey}");
            deletedSomething = true;
        }
        else
        {
            Debug.LogWarning($"Tried to delete profile data, but primary data was not found for PlayerPrefs key: {prefsKey}");
        }

        if (PlayerPrefs.HasKey(backupPrefsKey))
        {
            PlayerPrefs.DeleteKey(backupPrefsKey);
            Debug.Log($"Deleted backup data for key: {backupPrefsKey}");
            deletedSomething = true;
        }
        
        if (deletedSomething)
        {
            PlayerPrefs.Save(); // Persist changes
            RemoveProfileIdFromMasterList(profileId); // Update the master list
        }
    }

    [System.Serializable]
    private class ProfileIdListContainer
    {
        public List<string> ProfileIds = new List<string>();
    }
    
    private List<string> GetMasterProfileIdList()
    {
        if (PlayerPrefs.HasKey(AllProfilesMasterKey))
        {
            string jsonList = PlayerPrefs.GetString(AllProfilesMasterKey);
            if (!string.IsNullOrEmpty(jsonList))
            {
                ProfileIdListContainer container = JsonUtility.FromJson<ProfileIdListContainer>(jsonList);
                if (container != null && container.ProfileIds != null)
                {
                    return container.ProfileIds;
                }
                
                Debug.LogWarning($"Could not parse master profile ID list from JSON: {jsonList}");
            }
        }
        return new List<string>(); // Return empty list if not found or error
    }
    
    private void SaveMasterProfileIdList(List<string> profileIds)
    {
        ProfileIdListContainer container = new ProfileIdListContainer { ProfileIds = profileIds };
        string jsonList = JsonUtility.ToJson(container);
        PlayerPrefs.SetString(AllProfilesMasterKey, jsonList);
    }
    
    private void AddProfileIdToMasterList(string profileId)
    {
        if (string.IsNullOrEmpty(profileId)) return;
        List<string> profileIds = GetMasterProfileIdList();
        if (!profileIds.Contains(profileId))
        {
            profileIds.Add(profileId);
            SaveMasterProfileIdList(profileIds);
        }
    }
    
    private void RemoveProfileIdFromMasterList(string profileId)
    {
        if (string.IsNullOrEmpty(profileId)) return;
        List<string> profileIds = GetMasterProfileIdList();
        if (profileIds.Remove(profileId))
        {
            SaveMasterProfileIdList(profileIds);
        }
    }

    public override Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();
        List<string> profileIds = GetMasterProfileIdList();

        Debug.Log($"Found {profileIds.Count} profile IDs in master list.");

        foreach (string profileId in profileIds)
        {
            Debug.Log($"Loading data for profile ID: {profileId}");

            GameData profileData = Load(profileId, true); 
            
            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogWarning($"LoadAllProfiles: Profile data for ID '{profileId}' was null after attempting load. It might be corrupted or was deleted without updating master list properly.");
            }
        }
        return profileDictionary;
    }

    public override string GetMostRecentlyUpdatedProfileId()
    {
        string mostRecentProfileId = null;
        long mostRecentTimestamp = 0;

        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
        if (profilesGameData.Count == 0)
        {
            Debug.Log("GetMostRecentlyUpdatedProfileId: No profiles found.");
            return null;
        }

        foreach (KeyValuePair<string, GameData> pair in profilesGameData)
        {
            if (pair.Value == null)
            {
                Debug.LogWarning($"GetMostRecentlyUpdatedProfileId: GameData for profile '{pair.Key}' is null. Skipping.");
                continue;
            }
            
            if (pair.Value.LastUpdated > mostRecentTimestamp)
            {
                mostRecentTimestamp = pair.Value.LastUpdated;
                mostRecentProfileId = pair.Key;
            }
        }
        
        if (mostRecentProfileId != null)
        {
            Debug.Log($"Most recently updated profile ID: {mostRecentProfileId} (Timestamp: {mostRecentTimestamp})");
        }
        else
        {
            Debug.Log("GetMostRecentlyUpdatedProfileId: Could not determine the most recent profile. All loaded profiles might have missing LastUpdated data or timestamps were not greater than initial.");
        }
        return mostRecentProfileId;
    }
    
    private string EncryptDecrypt(string data)
    {
        if (string.IsNullOrEmpty(data)) return "";
        
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
    
    private bool AttemptRollback(string primaryPrefsKey)
    {
        bool success = false;
        string backupPrefsKey = primaryPrefsKey + _backupKeySuffix;
        try
        {
            if (PlayerPrefs.HasKey(backupPrefsKey))
            {
                string backupData = PlayerPrefs.GetString(backupPrefsKey);
                if (!string.IsNullOrEmpty(backupData))
                {
                    PlayerPrefs.SetString(primaryPrefsKey, backupData);
                    PlayerPrefs.Save();

                    success = true;
                    Debug.LogWarning($"Rolled back to backup for PlayerPrefs key: {primaryPrefsKey} from {backupPrefsKey}");
                }
                else
                {
                    Debug.LogError($"Backup data for key '{backupPrefsKey}' is empty. Cannot roll back.");
                }
            }
            else
            {
                 Debug.LogError($"Tried to roll back for PlayerPrefs key: {primaryPrefsKey}, but no backup key '{backupPrefsKey}' exists.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error occurred when trying to roll back to backup for PlayerPrefs key: {primaryPrefsKey} from {backupPrefsKey}\n{e}");
        }
        return success;
    }
    
    public override void DeleteAll()
    {
        Debug.LogWarning("Attempting to delete ALL data managed by WebDataHandler.");
        List<string> profileIds = GetMasterProfileIdList();
        foreach(string profileId in profileIds)
        {
            string prefsKey = GetProfileGameDataKey(profileId);
            if(PlayerPrefs.HasKey(prefsKey)) PlayerPrefs.DeleteKey(prefsKey);
            if(PlayerPrefs.HasKey(prefsKey + _backupKeySuffix)) PlayerPrefs.DeleteKey(prefsKey + _backupKeySuffix);
        }
        
        if(PlayerPrefs.HasKey(AllProfilesMasterKey)) PlayerPrefs.DeleteKey(AllProfilesMasterKey);
        if(PlayerPrefs.HasKey(_settingsKey)) PlayerPrefs.DeleteKey(_settingsKey);
        
        PlayerPrefs.Save();
        Debug.LogWarning("All WebDataHandler data has been deleted.");
    }
}
