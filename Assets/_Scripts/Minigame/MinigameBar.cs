using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class MinigameBar : Minigame
{
    [SerializeField] private RectTransform _pointer;
    [SerializeField] private RectTransform _targetArea;
    [SerializeField] private RectTransform _criticalTargetArea;
    [SerializeField] private RectTransform _skillCheckBar;
    
    private bool _isMovingRight = true;
    
    [Button]
    public void StartButton()
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

    public override void StartMinigame(MinigameSettings settings, Action<MinigameResult> completeMinigame)
    {
        base.StartMinigame(settings, completeMinigame);
        _speed *= 10;
        _targetArea.DOScaleX(1, 0.2f);
        GenerateNewTargetArea(false);
    }

    protected override void Update()
    {
        if (_isStopped)
            return;

        MovePointer();

        base.Update();
    }

    private void MovePointer()
    {
        Vector3 position = _pointer.localPosition;

        if (_isMovingRight)
        {
            position.x += _speed * Time.deltaTime;
            if (position.x >= _skillCheckBar.rect.width / 2)
                _isMovingRight = false;
        }
        else
        {
            position.x -= _speed * Time.deltaTime;
            if (position.x <= -_skillCheckBar.rect.width / 2)
                _isMovingRight = true;
        }

        _pointer.localPosition = position;
    }

    private void GenerateNewTargetArea(bool animate = true)
    {
        float barWidth = _skillCheckBar.rect.width;
        float targetWidth = Mathf.Clamp((barWidth / 100) * _targetAreaSize, 50f, barWidth / 2);
        _targetArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth);
        _criticalTargetArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, targetWidth / 5);
        float maxPosition = (barWidth - targetWidth) / 2;
        float randomX = Random.Range(-maxPosition, maxPosition);
        Vector3 targetPosition = new Vector3(randomX, _targetArea.localPosition.y, _targetArea.localPosition.z);

        if (animate)
            _targetArea.DOLocalMove(targetPosition, 0.2f, true);
        else
            _targetArea.localPosition = targetPosition;
    }

    protected override void CheckSuccess()
    {
        float pointerX = _pointer.localPosition.x;
        float criticalTargetMaxX = _targetArea.localPosition.x + (_criticalTargetArea.rect.width / 2f);
        float criticalTargetMinX = _targetArea.localPosition.x - (_criticalTargetArea.rect.width / 2f);
        float targetMaxX = _targetArea.localPosition.x + (_targetArea.rect.width / 1.9f);
        float targetMinX = _targetArea.localPosition.x - (_targetArea.rect.width / 1.9f);

        if (pointerX <= criticalTargetMaxX && pointerX >= criticalTargetMinX)
        {
            ModifyProgressAmount(_criticalSuccessProgressAmount);
            GenerateNewTargetArea();
        }
        else if (pointerX >= targetMinX && pointerX <= targetMaxX)
        {
            ModifyProgressAmount(_successProgressAmount);
            GenerateNewTargetArea();
        }
        else
        {
            ModifyProgressAmount(-_failureProgressAmount);
        }
    }

    protected override void WonMinigame()
    {
        _pointer.DOAnchorPosX(0, 0.2f).SetEase(Ease.OutQuint);
        _targetArea.DOScaleX(0, 0.2f);

        base.WonMinigame();
    }

    protected override void FailMinigame()
    {
        _pointer.DOAnchorPosX(0, 0.2f).SetEase(Ease.OutQuint);
        _targetArea.DOScaleX(0, 0.2f);
        
        base.FailMinigame();
    }

    protected override void ResetMinigame()
    {
        _pointer.localPosition = new Vector2(0, _pointer.localPosition.y);
        _targetArea.localScale = new Vector2(0, _targetArea.localScale.y);
    }
}