using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeController : MonoBehaviour
{
    private static readonly int Blend = Shader.PropertyToID("_Blend");
    
    //[SerializeField] private bool _overrideSave;
    [SerializeField] private TimeSettings _timeSettings;

    [SerializeField] private Light _sun;
    [SerializeField] private Light _moon;
    
    [SerializeField] private AnimationCurve _lightIntensityCurve;
    
    [SerializeField] private float _maxSunIntensity = 1f;
    [SerializeField] private float _maxMoonIntensity = 0.5f;

    [SerializeField] private Color _dayAmbientLight;
    [SerializeField] private Color _nightAmbientLight;

    [SerializeField] private Volume _volume;
    [SerializeField] private Material _skyboxMaterial;

    private ColorAdjustments _colorAdjustments;

    private static TimeService _service;
    public static TimeService Service => _service;
    
    public static event Action OnSunrise
    {
        add => _service.OnSunrise += value;
        remove => _service.OnSunrise -= value;
    }
    public static event Action OnSunset
    {
        add => _service.OnSunset += value;
        remove => _service.OnSunset -= value;
    }
    public static event Action<int> OnHourChange
    {
        add => _service.OnHourChange += value;
        remove => _service.OnHourChange -= value;
    }
    
    public static event Action<int>  OnDayChange
    {
        add => _service.OnDayChange += value;
        remove => _service.OnDayChange -= value;
    }

    private void Awake()
    {
        _service = new TimeService(_timeSettings);
    }

    private void Start()
    {
        _volume.profile.TryGet(out _colorAdjustments);
    }

    private void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
        UpdateSkyBlend();
    }

    private void UpdateTimeOfDay()
    {
        _service.UpdateTime(Time.deltaTime);
    }

    private void UpdateSkyBlend()
    {
        float dotProduct = Vector3.Dot(_sun.transform.forward, Vector3.up);
        float blend = Mathf.Lerp(0, 1, _lightIntensityCurve.Evaluate(dotProduct));
        _skyboxMaterial.SetFloat(Blend, blend);
    }

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(_sun.transform.forward, Vector3.down);
        float lightIntensity = _lightIntensityCurve.Evaluate(dotProduct);
        
        _sun.intensity = Mathf.Lerp(0, _maxSunIntensity, lightIntensity);
        _moon.intensity = Mathf.Lerp(_maxMoonIntensity, 0, lightIntensity);

        if (!_colorAdjustments)
            return;

        _colorAdjustments.colorFilter.value = Color.Lerp(_nightAmbientLight, _dayAmbientLight, _lightIntensityCurve.Evaluate(dotProduct));
        
    }

    private void RotateSun()
    {
        float rotation = _service.CalculateSunAngle();
        _sun.transform.rotation = Quaternion.AngleAxis(rotation, Vector3.right);
    }
}