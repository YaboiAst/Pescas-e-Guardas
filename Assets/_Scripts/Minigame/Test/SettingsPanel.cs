using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{
    [FormerlySerializedAs("_data")] [SerializeField]
    private MinigameSettings _settings;

    [SerializeField] private Slider _speedSlider;
    [SerializeField] private Slider _targetAreaSlider;
    [SerializeField] private Slider _cSuccessSlider;
    [SerializeField] private Slider _successSlider;
    [SerializeField] private Slider _failSlider;
    [SerializeField] private Slider _rateSlider;
    [SerializeField] private Slider _amountSlider;
    [SerializeField] private Slider _durationSlider;
    [SerializeField] private Toggle _overtimeToggle;


    private void Start()
    {
        _settings = new MinigameSettings();

        _speedSlider.onValueChanged.AddListener((v) => { _settings.Speed = v; });
        _targetAreaSlider.onValueChanged.AddListener((v) => { _settings.TargetAreaSize = v; });
        _cSuccessSlider.onValueChanged.AddListener((v) => { _settings.CriticalSuccessProgress = Mathf.RoundToInt(v); });
        _successSlider.onValueChanged.AddListener((v) => { _settings.SuccessProgress = Mathf.RoundToInt(v); });
        _failSlider.onValueChanged.AddListener((v) => { _settings.FailureProgress = Mathf.RoundToInt(v); });
        _rateSlider.onValueChanged.AddListener((v) => { _settings.DecreaseTimer = v; });
        _amountSlider.onValueChanged.AddListener((v) => { _settings.DecreaseAmount = Mathf.RoundToInt(v); });
        _durationSlider.onValueChanged.AddListener((v) => { _settings.Duration = v; });
        _overtimeToggle.onValueChanged.AddListener((v) => { _settings.DecreaseProgressOvertime = v; });
    }

    public void UseDefaultSettings()
    {
        MinigameSettings settings = new MinigameSettings
        {
            Speed = 50f,
            TargetAreaSize = 20,
            CriticalSuccessProgress = 25,
            SuccessProgress = 15,
            FailureProgress = 10,
            DecreaseProgressOvertime = false,
            DecreaseAmount = 0,
            DecreaseTimer = 0f,
            Duration = 20f
        };

        _speedSlider.value = settings.Speed;
        _targetAreaSlider.value = settings.TargetAreaSize;
        _cSuccessSlider.value = settings.CriticalSuccessProgress;
        _successSlider.value = settings.SuccessProgress;
        _failSlider.value = settings.FailureProgress;
        _rateSlider.value = settings.DecreaseTimer;
        _amountSlider.value = settings.DecreaseAmount;
        _durationSlider.value = settings.Duration;
        _overtimeToggle.isOn = settings.DecreaseProgressOvertime;

        _settings = settings;
    }

    public void StartMinigame(int index)
    {
        var type = MinigameType.Bar;
        if(index == 1)
            type = MinigameType.Key;
        else if(index == 2)
            type = MinigameType.Circle;

        _settings.Type = type;
        EventSystem.current.SetSelectedGameObject(null);
        MinigameManager.Instance.StartMinigame(_settings, OnComplete);
    }

    private void OnComplete(MinigameResult result)
    {
        
    }
}