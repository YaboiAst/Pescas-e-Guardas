using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class QuestProgress 
{
    public enum QuestStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Failed,
        Claimed,
    }

    public MyQuestInfo QuestData;
    public QuestStatus Status;
    public int CurrentProgress;

    public QuestProgress(MyQuestInfo quest)
    {
        QuestData = quest;
        this.Status = QuestStatus.NotStarted;
        CurrentProgress = 0;
    }

    public bool IsValid()
    {
        if (QuestData is null) return false;
        if (Status != QuestStatus.Completed) return false;

        return true;
    }
}
