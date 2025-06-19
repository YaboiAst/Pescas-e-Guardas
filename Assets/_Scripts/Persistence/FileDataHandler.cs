using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler : DataHandler
{
    private string _dataDirPath = "";
    private string _dataFileName = "";
    private string _settingsFileName = "";
    private bool _useEncryption = false;
    private readonly string _encryptionCodeWord = "kj23f3aav";
    private readonly string _backupExtension = ".bak";

    public FileDataHandler(string dataDirPath, string dataFileName, string settingsFileName, bool useEncryption)
    {
        this._dataDirPath = dataDirPath;
        this._dataFileName = dataFileName;
        this._settingsFileName = settingsFileName;
        this._useEncryption = useEncryption;
    }

    public override GameData Load(string profileId, bool allowRestoreFromBackup = true)
    {
        if (profileId == null)
            return null;

        string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad;
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                if (_useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                if (allowRestoreFromBackup)
                {
                    Debug.LogWarning("Failed to load data file. Attempting to roll back.\n" + e);
                    bool rollbackSuccess = AttemptRollback(fullPath);
                    if (rollbackSuccess)
                    {
                        loadedData = Load(profileId, false);
                    }
                }

                else
                {
                    Debug.LogError("Error occured when trying to load file at path: "
                                   + fullPath + " and backup did not work.\n" + e);
                }
            }
        }

        return loadedData;
    }

    public override void Save(GameData data, string profileId)
    {
        if (profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
        string backupFilePath = fullPath + _backupExtension;
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));


            string dataToStore = JsonUtility.ToJson(data, true);

            // string dataToStore = JsonConvert.SerializeObject(data, new JsonSerializerSettings
            // {
            //     TypeNameHandling = TypeNameHandling.Auto
            // });

            // optionally encrypt the data
            if (_useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }

            GameData verifiedGameData = Load(profileId);

            if (verifiedGameData != null)
            {
                File.Copy(fullPath, backupFilePath, true);
            }

            else
            {
                throw new Exception("Save file could not be verified and backup could not be created.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public override void SaveSettings(SettingsData data)
    {
        string fullPath = Path.Combine(_dataDirPath, _settingsFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? string.Empty);

            string dataToStore = JsonUtility.ToJson(data, true);

            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to save settings to file: " + fullPath + "\n" + e);
        }
    }

    public override SettingsData LoadSettings()
    {
        string fullPath = Path.Combine(_dataDirPath, _settingsFileName);
        SettingsData loadedData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadedData = JsonUtility.FromJson<SettingsData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load file at path: "
                               + fullPath + " and backup did not work.\n" + e);
            }
        }

        return loadedData;
    }

    public override void Delete(string profileId)
    {
        if (profileId == null)
        {
            return;
        }

        string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
        try
        {
            if (File.Exists(fullPath))
            {
                Directory.Delete(Path.GetDirectoryName(fullPath), true);
            }
            else
            {
                Debug.LogWarning("Tried to delete profile data, but data was not found at path: " + fullPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to delete profile data for profileId: "
                           + profileId + " at path: " + fullPath + "\n" + e);
        }
    }

    public override Dictionary<string, GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        IEnumerable<DirectoryInfo> dirInfos = new DirectoryInfo(_dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dirInfo in dirInfos)
        {
            string profileId = dirInfo.Name;

            string fullPath = Path.Combine(_dataDirPath, profileId, _dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading all profiles because it does not contain data: "
                                 + profileId);
                continue;
            }

            GameData profileData = Load(profileId);

            if (profileData != null)
            {
                profileDictionary.Add(profileId, profileData);
            }
            else
            {
                Debug.LogError("Tried to load profile but something went wrong. ProfileId: " + profileId);
            }
        }

        return profileDictionary;
    }

    public override string GetMostRecentlyUpdatedProfileId()
    {
        string mostRecentProfileId = null;

        Dictionary<string, GameData> profilesGameData = LoadAllProfiles();
        foreach (KeyValuePair<string, GameData> pair in profilesGameData)
        {
            string profileId = pair.Key;
            GameData gameData = pair.Value;

            if (gameData == null)
            {
                continue;
            }

            if (mostRecentProfileId == null)
            {
                mostRecentProfileId = profileId;
            }

            else
            {
                DateTime mostRecentDateTime =
                    DateTime.FromBinary(profilesGameData[mostRecentProfileId].LastUpdated);
                DateTime newDateTime = DateTime.FromBinary(gameData.LastUpdated);

                if (newDateTime > mostRecentDateTime)
                {
                    mostRecentProfileId = profileId;
                }
            }
        }

        return mostRecentProfileId;
    }

    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ _encryptionCodeWord[i % _encryptionCodeWord.Length]);
        }

        return modifiedData;
    }

    private bool AttemptRollback(string fullPath)
    {
        bool success = false;
        string backupFilePath = fullPath + _backupExtension;
        try
        {
            if (File.Exists(backupFilePath))
            {
                File.Copy(backupFilePath, fullPath, true);
                success = true;
                Debug.LogWarning("Had to roll back to backup file at: " + backupFilePath);
            }
            else
            {
                throw new Exception("Tried to roll back, but no backup file exists to roll back to.");
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured when trying to roll back to backup file at: "
                           + backupFilePath + "\n" + e);
        }

        return success;
    }

    public override void DeleteAll()
    {
        if (Directory.Exists(_dataDirPath))
        {
            foreach (var dir in Directory.GetDirectories(_dataDirPath))
            {
                try
                {
                    Directory.Delete(dir, true);
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to delete profile directory: " + dir + "\n" + e);
                }
            }
        }

        string settingsPath = Path.Combine(_dataDirPath, _settingsFileName);
        if (File.Exists(settingsPath))
        {
            try
            {
                File.Delete(settingsPath);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to delete settings file: " + settingsPath + "\n" + e);
            }
        }

        PlayerPrefs.Save();
        Debug.LogWarning("All FileDataHandler data has been deleted.");
    }
}