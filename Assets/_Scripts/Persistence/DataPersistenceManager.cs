using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NaughtyAttributes;
using PedronsaDev.Console;
using UnityEngine.SceneManagement;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("Debugging")] 
    [SerializeField] private bool _disableDataPersistence = false;
    [SerializeField] private bool _initializeDataIfNull = false;
    [SerializeField] private bool _showPopup = false;
    [Space(15)] 
    [SerializeField] private bool _overrideSelectedProfileId = false;

    [ShowIf(nameof(_overrideSelectedProfileId))] 
    [SerializeField] private string _testSelectedProfileId = "test";


    [Header("File Storage Config")] 
    [SerializeField] private bool _useEncryption = false;

    [SerializeField] private string _dataFileName = "game.sav";
    [SerializeField] private string _settingsFileName = "settings.cfg";


    [Header("Auto Saving Configuration")] 
    [SerializeField] private float _autoSaveTimeSeconds = 300f;

    private List<IDataPersistence> _dataPersistenceObjects;
    private DataHandler _dataHandler;

    [Header("Game Data Info")] 
    public string SelectedProfileId = "";
    [SerializeField] private GameData _gameData;

    private Coroutine _autoSaveCoroutine;

    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        if (_disableDataPersistence)
        {
            Debug.LogWarning("Data Persistence is currently disabled!");
        }

        bool isWeb = Application.platform == RuntimePlatform.WebGLPlayer;

        if (isWeb)
        {
            this._dataHandler = new WebDataHandler(Application.persistentDataPath, _dataFileName, _settingsFileName, _useEncryption);
        }
        else
        {
            this._dataHandler = new FileDataHandler(Application.persistentDataPath, _dataFileName, _settingsFileName, _useEncryption);
        }

        InitializeSelectedProfileId();
    }

    private void OnEnable() => UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

    private void OnDisable() => UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();

        LoadGame();

        if (_autoSaveCoroutine != null)
            StopCoroutine(_autoSaveCoroutine);

        _autoSaveCoroutine = StartCoroutine(AutoSave());
    }

    public SettingsData NewSettings() => new SettingsData();

    public void SaveSettings(SettingsData settingsData)
    {
        if (_disableDataPersistence)
        {
            return;
        }

        _dataHandler.SaveSettings(settingsData);
    }

    public SettingsData LoadSettings()
    {
        if (_disableDataPersistence)
        {
            return NewSettings();
        }

        SettingsData settingsData = _dataHandler.LoadSettings();

        if (settingsData == null && _initializeDataIfNull)
        {
            return NewSettings();
        }

        return settingsData;
    }

    public void NewGame() => this._gameData = new GameData();

    [Command("save_game", "Saves the game at current state")]
    public void SaveGame()
    {
        if (_disableDataPersistence)
        {
            return;
        }

        if (this._gameData == null)
        {
            Debug.LogWarning("No data was found. A New Game needs to be started before data can be saved.");
            return;
        }

        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(_gameData);
        }

        _gameData.LastUpdated = System.DateTime.Now.ToBinary();
        _gameData.LastSession = System.DateTime.Now.ToString("g");

        _dataHandler.Save(_gameData, SelectedProfileId);

        // if (PopupManager.Instance && _showPopup)
        // {
        //     PopupManager.Instance.ShowPopup(new PopupMessage(PopupType.WARNING, "Saving",
        //         "Please wait for game to save", 2f));
        // }
    }

    [Command("load_game", "Loads the game at saved state")]
    public void LoadGame()
    {
        if (_disableDataPersistence)
        {
            Debug.Log("Data persistence is disabled");
            return;
        }

        this._gameData = _dataHandler.Load(SelectedProfileId);

        if (this._gameData == null && _initializeDataIfNull)
        {
            NewGame();
        }

        if (this._gameData == null)
        {
            Debug.Log("No data was found. A New Game needs to be started before data can be loaded.");
            return;
        }

        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(_gameData);
        }
    }

    public void ChangeSelectedProfileId(string newProfileId)
    {
        this.SelectedProfileId = newProfileId;

        LoadGame();
    }

    public void DeleteProfileData(string profileId)
    {
        _dataHandler.Delete(profileId);

        InitializeSelectedProfileId();

        LoadGame();
    }

    private void InitializeSelectedProfileId()
    {
        this.SelectedProfileId = _dataHandler.GetMostRecentlyUpdatedProfileId();
        if (_overrideSelectedProfileId)
        {
            this.SelectedProfileId = _testSelectedProfileId;
            Debug.LogWarning("Overrode selected profile id with test id: " + _testSelectedProfileId);
        }
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects =
            Enumerable.OfType<IDataPersistence>(FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None));
        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return _gameData != null;
    }

    public void SetSaveName(string newSaveName)
    {
        _gameData.SaveName = newSaveName;
    }

    public Dictionary<string, GameData> GetAllProfilesGameData()
    {
        Debug.Log("Loading All Data Save Profiles");
        return _dataHandler.LoadAllProfiles();
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(_autoSaveTimeSeconds);
            SaveGame();
            Debug.Log("Auto Saved Game");
        }
    }
}