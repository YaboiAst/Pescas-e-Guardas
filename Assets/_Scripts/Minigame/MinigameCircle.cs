using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MinigameCircle : Minigame
{
    private static readonly int Radius = Shader.PropertyToID("_Radius");
    
    [SerializeField] private RawImage _targetCircle;
    [SerializeField] private RawImage _shrinkingCircle;
    
    private const float PERFECT_THRESHOLD = 2f;

    private const float INITIAL_SIZE = 0.495f;
    private float _targetSize;

    private Material _targetMaterial;
    private Material _shrinkingMaterial;

    private void Awake()
    {
        _targetMaterial = _targetCircle.material;
        _shrinkingMaterial = _shrinkingCircle.material;
    }

    [Button]
    private void StartButton()
    {
        MinigameSettings settings = new MinigameSettings
        {
            Speed = 5f,
            TargetAreaSize = 20,
            CriticalSuccessProgress = 25,
            SuccessProgress = 15,
            FailureProgress = 10,
            DecreaseProgressOvertime = false,
            DecreaseAmount = 0,
            DecreaseTimer = 0f,
            Duration = 20f
        };
        
        StartMinigame(settings, null);
    }

    // protected override void Update()
    // {
    //     base.Update();
    // }

    public override void StartMinigame(MinigameSettings settings, Action<MinigameResult> completeMinigame)
    {
        base.StartMinigame(settings, completeMinigame);
        _speed *= 10;
        _speed = 5 / (_speed / 100);
        ResetMinigame();
        Restart();
    }

    private void GenerateNewTargetArea(bool animate = true)
    {
        _targetSize = Random.Range(0.07f, 0.19f);

        if (animate)
            _targetMaterial.DOFloat(_targetSize, Radius, 0.2f).SetEase(Ease.OutQuint);
        else
            _targetMaterial.SetFloat(Radius, _targetSize);
    }
    
    private void ShrinkCircle()
    {
        float speed = _speed * Random.Range(0.9f, 1.1f);
        
        _shrinkingMaterial.SetFloat(Radius, INITIAL_SIZE);
        _shrinkingMaterial.DOFloat(_targetSize, Radius, speed)
            .SetId("Shrink")
            .OnComplete(Restart);
    }

    protected override void CheckSuccess()
    {
        float diff = _shrinkingMaterial.GetFloat(Radius) - _targetSize;
        diff = Mathf.Abs(diff);
        diff *= 100f;
        
        if (diff <= 0.5f)
        {
            ModifyProgressAmount(_criticalSuccessProgressAmount);
        }
        else if (diff <= PERFECT_THRESHOLD)
        {
            ModifyProgressAmount(_successProgressAmount);
        }
        else
        {
            ModifyProgressAmount(-_failureProgressAmount);
        }

        
        Restart();
        base.CheckSuccess();
    }

    protected override void WonMinigame()
    {
        if(_isStopped)
            return;
        
        DOTween.Kill("Shrink");
        _shrinkingMaterial.DOFloat(1, Radius, 0.3f);
        _targetMaterial.DOFloat(0, Radius, 0.3f);
        base.WonMinigame();
    }

    protected override void FailMinigame()
    {
        DOTween.Kill("Shrink");
        _shrinkingMaterial.DOFloat(1, Radius, 0.3f);
        _targetMaterial.DOFloat(0, Radius, 0.3f);
        base.FailMinigame();
    }

    protected override void ResetMinigame()
    {
        _shrinkingMaterial.SetFloat(Radius, INITIAL_SIZE);
        GenerateNewTargetArea(false);
        base.ResetMinigame();
    }

    private void Restart()
    {
        if(_isStopped)
            return;
        
        DOTween.Kill("Shrink");
        GenerateNewTargetArea();
        ShrinkCircle();
    }
}
