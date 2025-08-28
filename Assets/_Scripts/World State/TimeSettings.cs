using System;
using UnityEngine.Serialization;

[Serializable]
public class TimeSettings
{
    public float TimeMultiplier = 1;
    public int StartDay;
    public int StartHour;
    public int StartMinute;
    public int SunriseHour;
    public int SunsetHour;


    public TimeSettings(int startDay, int startHour, int startMinute)
    {
        StartDay = startDay;
        StartHour = startHour;
        StartMinute = startMinute;

        SunriseHour = 6;
        SunsetHour = 18;
    }
}
