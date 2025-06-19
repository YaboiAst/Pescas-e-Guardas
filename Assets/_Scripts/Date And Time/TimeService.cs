using System;
using UnityEngine;

public class TimeService
{
    private readonly TimeSettings _settings;
    private DateTime _currentTime;
    private readonly TimeSpan _sunriseTime;
    private readonly TimeSpan _sunsetTime;

    public event Action OnSunrise = delegate { };
    public event Action OnSunset = delegate { };
    public event Action<int> OnHourChange = delegate { };
    public event Action<int> OnDayChange = delegate { };

    private readonly Observable<bool> _isDayTime; 
    private readonly Observable<int> _currentHour; 
    private readonly Observable<int> _currentDay; 

    private int _daysPassed = 0;
    private int _lastDay;

    public TimeService(TimeSettings settings)
    {
        this._settings = settings;
        _currentTime = DateTime.Now.Date 
                       + TimeSpan.FromHours(settings.StartHour) 
                       + TimeSpan.FromMinutes(settings.StartMinute);
        _sunriseTime = TimeSpan.FromHours(settings.SunriseHour);
        _sunsetTime = TimeSpan.FromHours(settings.SunsetHour);

        _isDayTime = new Observable<bool>(IsDayTime());
        _currentHour = new Observable<int>(_currentTime.Hour);
        _currentDay = new Observable<int>(_daysPassed);

        
        _isDayTime.ValueChanged += day => (day ? OnSunrise : OnSunset)?.Invoke();
        _currentHour.ValueChanged += _ => OnHourChange?.Invoke(_currentHour.Value);
        _currentDay.ValueChanged += _ => OnDayChange?.Invoke(_daysPassed);
        
        _daysPassed = settings.StartDay;
        _lastDay = _currentTime.Day;
    }

    public void UpdateTime(float deltaTime)
    {
        _currentTime = _currentTime.AddSeconds(deltaTime * _settings.TimeMultiplier);
        _isDayTime.Value = IsDayTime();
        _currentHour.Value = _currentTime.Hour;
        _currentDay.Value = _daysPassed;
        
        if (_currentTime.Day != _lastDay)
        {
            _daysPassed++;
            _lastDay = _currentTime.Day;
        }
    }

    public float CalculateSunAngle()
    {
        bool isDay = IsDayTime();
        float startDegree = isDay ? 0 : 180;
        TimeSpan start = isDay ? _sunriseTime : _sunsetTime;
        TimeSpan end = isDay ? _sunsetTime : _sunriseTime;

        TimeSpan totalTime = CalculateDifference(start, end);
        TimeSpan elapsedTime = CalculateDifference(start, _currentTime.TimeOfDay);

        double percentage = elapsedTime.TotalMinutes / totalTime.TotalMinutes;
        return Mathf.Lerp(startDegree, startDegree + 180, (float)percentage);
    }

    public DateTime CurrentTime => _currentTime;
    public int DaysPassed => _daysPassed;

    private bool IsDayTime() => _currentTime.TimeOfDay > _sunriseTime && _currentTime.TimeOfDay < _sunsetTime;

    private TimeSpan CalculateDifference(TimeSpan from, TimeSpan to)
    {
        TimeSpan difference = to - from;
        return difference.TotalHours < 0 ? difference + TimeSpan.FromHours(24) : difference;
    }
}
