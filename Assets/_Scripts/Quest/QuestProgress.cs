using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestProgress 
{

    public enum questStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Failed
    }

    public questStatus status;


    public QuestProgress(questStatus status)
    {
        this.status = status;
    }
}
