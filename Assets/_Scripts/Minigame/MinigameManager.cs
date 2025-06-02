using System;
using PedronsaDev.Console;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance;

    private static Minigame _currentMinigame;

    [SerializeField] private GameObject _minigameBar;
    [SerializeField] private GameObject _minigameKey;
    [SerializeField] private GameObject _minigameCircle;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;   
    }

    #if UNITY_EDITOR
    [Command("start_minigame","Starts a minigame using default settings")]
    public void StartMinigame(MinigameType type)
    {
        StartMinigame(50, type, (MinigameResult result) =>
        {
            Debug.Log("Minigame Finished");
        });
    }
  #endif
    public void StartMinigame(MinigameSettings settings, Action<MinigameResult> completeMinigame)
    {
        switch (settings.Type)
        {
            case MinigameType.Bar:
                _minigameBar.SetActive(true);
                _minigameKey.SetActive(false);
                _minigameCircle.SetActive(false);
                _currentMinigame = _minigameBar.GetComponentInChildren<Minigame>();
                break;
            case MinigameType.Key:
                _minigameBar.SetActive(false);
                _minigameKey.SetActive(true);
                _minigameCircle.SetActive(false);
                _currentMinigame = _minigameKey.GetComponentInChildren<Minigame>();
                break;
            case MinigameType.Circle:
                _minigameBar.SetActive(false);
                _minigameKey.SetActive(false);
                _minigameCircle.SetActive(true);
                _currentMinigame = _minigameCircle.GetComponentInChildren<Minigame>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(settings.Type), settings.Type, null);
        }
        
        _currentMinigame.StartMinigame(settings, completeMinigame);
    }
    
    public void StartMinigame(int difficulty, MinigameType type, Action<MinigameResult> completeMinigame)
    {
        switch (type)
        {
            case MinigameType.Bar:
                _minigameBar.SetActive(true);
                _minigameKey.SetActive(false);
                _minigameCircle.SetActive(false);
                _currentMinigame = _minigameBar.GetComponentInChildren<Minigame>();
                break;
            case MinigameType.Key:
                _minigameBar.SetActive(false);
                _minigameKey.SetActive(true);
                _minigameCircle.SetActive(false);
                _currentMinigame = _minigameKey.GetComponentInChildren<Minigame>();
                break;
            case MinigameType.Circle:
                _minigameBar.SetActive(false);
                _minigameKey.SetActive(false);
                _minigameCircle.SetActive(true);
                _currentMinigame = _minigameCircle.GetComponentInChildren<Minigame>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        MinigameSettings minigameSettings = GenerateMinigameSettings(difficulty, type);

        _currentMinigame.StartMinigame(minigameSettings, completeMinigame);
    }

    private MinigameSettings GenerateMinigameSettings(int difficulty, MinigameType type)
    {
                const float MIN_SPEED = 10f, MAX_SPEED = 100f;
                const float MIN_TARGET_AREA_SIZE = 10f, MAX_TARGET_AREA_SIZE = 100f;
                const float MIN_CRITICAL_SUCCESS = 10f, MAX_CRITICAL_SUCCESS = 40f;
                const float MIN_SUCCESS = 5f, MAX_SUCCESS = 25f;
                const float MIN_FAILURE = 5f, MAX_FAILURE = 20f;
                const float MIN_DURATION = 10f, MAX_DURATION = 30f;
                const float MIN_OVERTIME_DECREASE = 10f, MAX_OVERTIME_DECREASE = 20f;
                const float MIN_DECREASE_TIMER = 1f, MAX_DECREASE_TIMER = 5f;

                difficulty = difficulty switch
                {
                    > 100 => 100,
                    < 1 => 1,
                    _ => difficulty
                };

                bool shouldDecreaseOvertime = difficulty > 50;
                float t = Mathf.Clamp01((difficulty - 1) / 99f);
                
        
                MinigameSettings settings = new MinigameSettings(type)
                {
                    Speed = Mathf.Lerp(MIN_SPEED, MAX_SPEED, t),
                    TargetAreaSize = Mathf.Lerp(MIN_TARGET_AREA_SIZE, MAX_TARGET_AREA_SIZE, t),
                    CriticalSuccessProgress = Mathf.Lerp(MIN_CRITICAL_SUCCESS, MAX_CRITICAL_SUCCESS, 1 - t),
                    SuccessProgress = Mathf.Lerp(MIN_SUCCESS, MAX_SUCCESS, 1 - t),
                    FailureProgress = Mathf.Lerp(MIN_FAILURE, MAX_FAILURE, t),
                    Duration = Mathf.Lerp(MIN_DURATION, MAX_DURATION, t),
                    DecreaseProgressOvertime = shouldDecreaseOvertime,
                    DecreaseAmount = Mathf.Lerp(MIN_OVERTIME_DECREASE, MAX_OVERTIME_DECREASE, t),
                    DecreaseTimer = Mathf.Lerp(MIN_DECREASE_TIMER, MAX_DECREASE_TIMER, 1 - t)
                };
        
                return settings;
    }
}
