using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Minigame : MonoBehaviour
{
    [SerializeField] private KeyCode _interactKeyCode;

    [Header("UI Settings")] 
    [SerializeField] private GameObject _canvas;
    [SerializeField] protected RectTransform _progressBar;
    [SerializeField] protected RectTransform _progressBarBackground;
    [SerializeField] protected Image _durationTimeBar;

    private static event Action<MinigameResult> OnMinigameComplete;
    public static event Action<bool> OnMinigameUpdated;
    
    protected float _speed = 500f;
    protected float _targetAreaSize = 10;
    protected float _criticalSuccessProgressAmount = 30;
    protected float _successProgressAmount = 15;
    protected float _failureProgressAmount = 10;
    private float _progressAmount;

    private bool _decreaseProgressOvertime;
    private float _decreaseAmount;
    private float _decreaseTimer;
    private float _timer;

    private float _duration = 10f;
    private float _durationTimer;
    
    protected bool _isStopped = true;

    protected virtual void Update()
    {
        if(_isStopped)
            return;
        
        if (Input.GetKeyDown(_interactKeyCode)) 
            CheckSuccess();
        
        if (_decreaseProgressOvertime && _decreaseTimer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _timer = _decreaseTimer;
                ModifyProgressAmount(-_decreaseAmount);
            }
        }
        
        if (_durationTimer <= 0)
            FailMinigame();
        else
            _durationTimer -= Time.deltaTime;
    }

    protected virtual void CheckSuccess()
    {

    }

    public virtual void StartMinigame(MinigameSettings settings, Action<MinigameResult> completeMinigame)
    {
        _speed = settings.Speed;
        _targetAreaSize = settings.TargetAreaSize;
        _criticalSuccessProgressAmount = settings.CriticalSuccessProgress;
        _successProgressAmount = settings.SuccessProgress;
        _failureProgressAmount = settings.FailureProgress;

        _decreaseProgressOvertime = settings.DecreaseProgressOvertime;
        _decreaseAmount = settings.DecreaseAmount;
        _decreaseTimer = settings.DecreaseTimer;
        
        _duration = settings.Duration;
        _durationTimer = settings.Duration;
        
        _timer = _decreaseTimer;
        _isStopped = false;
        
        _progressAmount = 0;
        
        OnMinigameComplete = completeMinigame;
        
        SetProgressAmount(0);
        
        _durationTimeBar.fillAmount = 1;
        DOTween.Kill("duration");
        _durationTimeBar.DOFillAmount(0, _duration).SetEase(Ease.Linear).SetId("duration");
        _progressBar.sizeDelta = new Vector2(0, _progressBar.sizeDelta.y);
        
        ResetMinigame();
        //Debug.Log("Minigame Started");
        OnMinigameUpdated?.Invoke(true);
    }

    protected virtual void FailMinigame()
    {
        CompleteMinigame();
        //Debug.Log("Minigame Failed");
    }

    protected virtual void WonMinigame()
    {
        CompleteMinigame();
        //Debug.Log("Minigame Completed");
    }
    
    protected virtual void CompleteMinigame()
    {
        ResetUI();
        OnMinigameComplete?.Invoke(MinigameResult.Won);
        OnMinigameUpdated?.Invoke(false);
        _canvas.gameObject.SetActive(false);
    }
    private void ResetUI()
    {
        _isStopped = true;
        
        _progressBar.DOSizeDelta(new Vector2(0, _progressBar.sizeDelta.y), 0.3f).SetEase(Ease.OutQuint);
        _durationTimeBar.fillAmount = 1;
        DOTween.Kill("duration");
    }

    protected virtual void ResetMinigame()
    {
        _progressBar.sizeDelta = new Vector2(0, _progressBar.sizeDelta.y);
        //Debug.Log("Minigame Reseted");
    }

    protected virtual void ModifyProgressAmount(float amount)
    {
        _progressAmount += amount;

        if (_progressAmount <= 0)
        {
            _progressAmount = 0;
        }
        else if (_progressAmount >= 100)
        {
            _progressAmount = 100f;
            WonMinigame();
        }
        
        float size = (_progressBarBackground.rect.width / 100) * _progressAmount;
        Vector2 targetSize = new Vector2(size, _progressBar.rect.size.y);
        _progressBar.DOSizeDelta(targetSize, 0.5f).SetEase(Ease.OutQuint);
    }
    
    private void SetProgressAmount(int amount)
    {
        _progressAmount = amount;

        float size = (_progressBarBackground.rect.width / 100) * _progressAmount;
        Vector2 targetSize = new Vector2(size, _progressBar.rect.size.y);

        _progressBar.sizeDelta = targetSize;
    }
}