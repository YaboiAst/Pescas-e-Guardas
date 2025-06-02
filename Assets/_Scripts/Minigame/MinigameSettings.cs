using System;

[Serializable]
public struct MinigameSettings
{
    public MinigameType Type;
    public float Speed;
    public float TargetAreaSize;

    public float CriticalSuccessProgress;
    public float SuccessProgress;
    public float FailureProgress;

    public bool DecreaseProgressOvertime;
    public float DecreaseAmount;
    public float DecreaseTimer;

    public float Duration;
    public MinigameSettings(MinigameType type)
    {
        Type = type;
        Speed = 1;
        TargetAreaSize = 100;
        CriticalSuccessProgress = 150;
        SuccessProgress = 100;
        FailureProgress = 0.2f;
        DecreaseProgressOvertime = false;
        DecreaseAmount = 0.1f;
        DecreaseTimer = 1;
        Duration = 15;
    }
}