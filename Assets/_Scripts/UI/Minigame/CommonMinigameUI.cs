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
        if (Instance is not null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        HideCanvas();
        FishingManager.OnFishComplete.AddListener(() => minigameStarted = false);
    }
    
    public void ShowUI(FishLootTable lootTable)
    {
        if (IsOpen) return;
        
        minigameStarted = false;
        ShowCanvas();
        _probabilitiesUI.GenerateUI(lootTable);
        FishingManager.PrepFishMinigame();
    }
    public void HideUI()
    {
        if (minigameStarted || !IsOpen) return;
        
        FishingManager.CloseFishMinigame();
        _probabilitiesUI.ClearUI();
        InventoryController.HideInventory();
        HideCanvas();
    }


    public void HideOnlyMinigameUI()
    {
        if (minigameStarted || !IsOpen) return;

        FishingManager.CloseFishMinigame();
        _probabilitiesUI.ClearUI();
        HideCanvas();
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
