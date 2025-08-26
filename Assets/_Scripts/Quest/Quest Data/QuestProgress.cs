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
        Failed
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
        if (this is null) return false;
        if (QuestData is null) return false;
        if (Status != QuestStatus.InProgress) return false;

        return true;
    }
}
