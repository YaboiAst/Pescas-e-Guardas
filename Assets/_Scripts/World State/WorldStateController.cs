using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class WorldStateController : MonoBehaviour
{
    public static WeatherState Weather { get; private set; }
    public static event Action<WeatherState> OnWeatherChange;

    [SerializeField] private int _weatherChangeInterval = 3; // Horas do jogo
    [SerializeField, Range(0,1)] private float _tenebroso = 0f;
    [SerializeField] private Material _waterMaterial;


    private int _currentCounter = 0;
    private void Start()
    {
        _currentCounter = _weatherChangeInterval;
        TimeController.OnHourChange += HourChanged;
    }
    private void HourChanged(int hour)
    {
        _currentCounter--;

        if (_currentCounter == 0)
        {
            ChangeWeather();
            _currentCounter = _weatherChangeInterval;
        }
    }
    private void ChangeWeather()
    {
        float rand = UnityEngine.Random.value;
        int prevWeather = _currentCounter;
        if (rand < 0.4f)
        {
            Weather = WeatherState.Clear;
        }
        else
        {
            Array values = Enum.GetValues(typeof(WeatherState));
            List<WeatherState> nonClear = new List<WeatherState>();
            foreach (WeatherState state in values)
            {
                if (state != WeatherState.Clear)
                    nonClear.Add(state);
            }
            Weather = nonClear[UnityEngine.Random.Range(0, nonClear.Count)];
        }

        if (_currentCounter != prevWeather)
            OnWeatherChange?.Invoke(Weather);

        //Debug.unityLogger.Log("Weather Changed: " + Weather);
    }

    [Button]
    public void UpdateTenebroso()
    {
        _waterMaterial.SetFloat("_GradientValue", _tenebroso);
    }
}


public enum WeatherState
{
    Clear,
    Cloudy,
    Raining,
    Stormy,
    Snowing,
    Foggy,
    Windy
}
