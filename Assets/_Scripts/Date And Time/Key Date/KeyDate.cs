using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(menuName = "Key Date/New Key Date")]
public class KeyDate : ScriptableObject
{
    
    public string KeyDateName;
    [TextArea] public string KeyDateDescription;

    public KeyDateType Type;
    
    [Header("Event Date")]
    [ShowIf(nameof(Type), KeyDateType.Single)] public int Day = 1;
    //[ShowIf(nameof(Type), KeyDateType.Single)] public int Month = 1;
    //[ShowIf(nameof(Type), KeyDateType.Single)] public int Year = 1;

    [ShowIf(nameof(Type), KeyDateType.Recurring)]
    public int AmountOfDays = 1;
    
    [Header("Event Time")]
    [Range(0, 23)]
    public int Hour;

    
    [Header("Actions")]
    public List<GameEvent> KeyDateTriggerEvents;
    public event Action OnKeyDateTriggered;

    public bool ShouldTriggerToday(int day)
    {
        if (Type == KeyDateType.Recurring)
            return day % AmountOfDays == 0;
        
        return day == Day;
    }

    public void TriggerDate()
    {
        OnKeyDateTriggered?.Invoke();
        
        foreach (GameEvent gameEvent in KeyDateTriggerEvents) 
            gameEvent.Invoke();
        
        Debug.Log($"KeyDate Triggered: {KeyDateName}");
    }
}

public enum KeyDateType
{
    Single,
    Recurring
}
