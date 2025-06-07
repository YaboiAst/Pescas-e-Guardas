using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KeyDateHandler : MonoBehaviour
{
    [SerializeField] private List<KeyDate> _keyDates;

    [SerializeField] private List<KeyDate> _currentKeyDates;
    private void OnValidate() => _keyDates = Extensions.GetAllInstances<KeyDate>().ToList();

    private void OnEnable()
    {
        TimeManager.OnDayChanged += DayChanged;
        TimeManager.OnMinuteChanged += HandleCurrentKeyDates;
    }

    private void OnDestroy()
    {
        TimeManager.OnDayChanged -= DayChanged;
        TimeManager.OnMinuteChanged -= HandleCurrentKeyDates;
    }

    private void DayChanged(int day)
    {
        _currentKeyDates = new List<KeyDate>();
        
        foreach (KeyDate keyDate in _keyDates)
        {
            if (keyDate.ShouldTriggerToday(day)) 
                _currentKeyDates.Add(keyDate);
        }
    }
    
    private void HandleCurrentKeyDates(int minute, int hour)
    {
        if (_currentKeyDates.Count == 0) return;
        
        foreach (KeyDate keyDate in _currentKeyDates)
        {
            if (keyDate.Hour == hour && keyDate.Minute == minute) 
                keyDate.TriggerDate();
        }
    }

    public List<KeyDate> GetKeyDatesList() => _keyDates;
}