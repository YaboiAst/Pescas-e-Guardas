using System;
using TMPro;
using UnityEngine;

public class DateAndTimeUI : CanvasController
{
    [SerializeField] private TMP_Text _dateText;
    [SerializeField] private TMP_Text _timeText;

    private void Start()
    {
        TimeManager.OnDayChange += UpdateDateText;
        UpdateDateText(TimeManager.Service.DaysPassed);
    }

    private void Update() => UpdateTimeText();

    private void UpdateTimeText()
    {

        if (_timeText)
            _timeText.text = TimeManager.Service.CurrentTime.ToString("HH:mm");
    }

    private void UpdateDateText(int day)
    {
        int mod = day % 7;

        string weekDay = "Sunday";
        
        switch (mod)
        {
            case 1:
                weekDay = "Monday";
                break;
            case 2:
                weekDay = "Tuesday";
                break;
            case 3:
                weekDay = "Wednesday";
                break;
            case 4:
                weekDay = "Thursday";
                break;
            case 5:
                weekDay = "Friday";
                break;
            case 6:
                weekDay = "Saturday";
                break;
        }
        
        _dateText.SetText($"{weekDay}, Day {day}");

    }


}