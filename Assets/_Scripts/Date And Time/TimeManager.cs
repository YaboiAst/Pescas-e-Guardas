using System;
using NaughtyAttributes;
using UnityEngine;

public class TimeManager : MonoBehaviour, IDataPersistence
{
    public static TimeManager Instance { get; private set; }
    
    [Header("Time Settings")] 
    [SerializeField] private bool _overrideSaveTime; 
    [Range(0, 23), SerializeField, ShowIf(nameof(_overrideSaveTime))] 
    private int _initialHour = 8;
    [Range(0, 59), SerializeField, ShowIf(nameof(_overrideSaveTime))] 
    private int _initialMinute = 0;

    [SerializeField] private float _secondsPerMinute = 5.0f;

    [Header("Date Settings")] 
    [SerializeField] private int _day = 1;
    //[SerializeField] private int _month = 1;
    //[SerializeField] private int _year = 1;

    [Header("Current Time")] 
    [SerializeField, ReadOnly] private int _currentHour;
    [SerializeField, ReadOnly] private int _currentMinute;
    [SerializeField, ReadOnly] private float _currentSeconds;

    public static event Action<int, int> OnMinuteChanged;

    public static event Action<int> OnHourChanged;

    //public static event Action<int, int, int> OnDayChanged;
    public static event Action<int> OnDayChanged;

    private void Start()
    {
        OnMinuteChanged?.Invoke(_currentHour, _currentMinute);
        OnHourChanged?.Invoke(_currentHour);
        OnDayChanged?.Invoke(_day);
    }

    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(gameObject);
        
        Instance = this;
    }

    private void Update()
    {
        UpdateTime();
    }
    private void UpdateTime()
    {
        _currentSeconds += Time.deltaTime;

        if (!(_currentSeconds >= _secondsPerMinute)) 
            return;
        
        _currentSeconds = 0f;
        _currentMinute++;

        if (_currentMinute >= 60)
        {
            _currentMinute = 0;
            _currentHour++;

            if (_currentHour >= 24)
            {
                _currentHour = 0;
                _day++;

                // if (_day > 30)
                // {
                //     _day = 1;
                //     _month++;
                //
                //     if (_month > 12)
                //     {
                //         _month = 1;
                //         _year++;
                //     }
                // }
                
                //OnDayChanged?.Invoke(_day, _month, _year);
                OnDayChanged?.Invoke(_day);
            }
            OnHourChanged?.Invoke(_currentHour);
        }
        OnMinuteChanged?.Invoke(_currentHour, _currentMinute);
    }

    public string GetTimeAsString() => $"{_currentHour:D2}:{_currentMinute:D2}";

    public string GetDateAsString() => $"Day {_day}";

    public int GetCurrentHour() => _currentHour;

    public int GetCurrentMinute() => _currentMinute;

    public int GetCurrentDay() => _day;

    // public int GetCurrentMonth() => _month;

    // public int GetCurrentYear() => _year;

    public void LoadData(GameData data)
    {
        if (_overrideSaveTime)
        {
            _currentHour = _initialHour;
            _currentMinute = _initialMinute;
            _currentSeconds = 0f;
            Debug.LogWarning("Overrode Date and Time");
        }
        else
        {
            _currentHour = data.CurrentHour;
            _currentMinute = data.CurrentMinute;
            _currentSeconds = data.CurrentSeconds;
            _day = data.CurrentDay;
            //_month = data.CurrentMonth;
            //_year = data.CurrentYear;
        }

        OnMinuteChanged?.Invoke(_currentHour, _currentMinute);
        OnHourChanged?.Invoke(_currentHour);
        OnDayChanged?.Invoke(_day);
        //OnDayChanged?.Invoke(_day, _month, _year);
    }

    public void SaveData(GameData data)
    {
        data.CurrentHour = _currentHour;
        data.CurrentMinute = _currentMinute;
        data.CurrentSeconds = _currentSeconds;
        data.CurrentDay = _day;
        //data.CurrentMonth = _month;
        //data.CurrentYear = _year;
    }
}