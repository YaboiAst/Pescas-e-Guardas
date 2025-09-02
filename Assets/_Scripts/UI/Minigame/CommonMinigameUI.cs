using UnityEngine;

public class CommonMinigameUI : CanvasController
{
    public static CommonMinigameUI Instance;

    [SerializeField] private KeyCode _playInteractionKey;
    [SerializeField] private KeyCode _closeInteractionKey;
    [SerializeField] private FishingSpotProbabilitiesUI _probabilitiesUI;
    
    private bool minigameStarted = false;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        HideCanvas();
        FishingManager.OnFishComplete.AddListener(() => minigameStarted = false);
    }
    
    public void ShowUI(FishLootTable lootTable = null)
    {
        if (IsOpen)
            return;
        
        minigameStarted = false;
        ShowCanvas();
        if (lootTable != null)
        {
            GenerateProbabilitiesUI(lootTable);
        }
        FishingManager.PrepFishMinigame();
    }
    public void GenerateProbabilitiesUI(FishLootTable lootTable) => _probabilitiesUI.GenerateUI(lootTable);
    public void HideUI()
    {
        InventoryController.HideInventory();

        if (minigameStarted || !IsOpen)
            return;

        FishingManager.CloseFishMinigame();
        _probabilitiesUI.ClearUI();
        HideCanvas();
    }

    public void HideOnlyMinigameUI(bool triggerEvent)
    {
        if (minigameStarted || !IsOpen) return;

        FishingManager.CloseFishMinigame();
        _probabilitiesUI.ClearUI();
        HideCanvas(triggerEvent: triggerEvent);
    }
    
    private void Update()
    {
        if (minigameStarted)
            return;

        if (Input.GetKeyDown(_playInteractionKey))
        {
            minigameStarted = FishingManager.PlayFishMinigame();;
        }
        else if (Input.GetKeyDown(_closeInteractionKey))
        {
            MinigameManager.Instance.StopMinigame();
            HideUI();
        }
    }
}
