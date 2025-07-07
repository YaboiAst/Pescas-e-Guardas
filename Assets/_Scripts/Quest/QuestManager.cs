using System;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public QuestProgress CurrentProgress { get; private set; }

    public static readonly UnityEvent OnFinishQuest = new();

    public static readonly UnityEvent<QuestProgress> OnStartQuest = new UnityEvent<QuestProgress>();


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InventoryController.OnProgressQuest.AddListener(CheckQuestProgress);
    }
    public void AddQuest(MyQuestInfo quest)
    {
        CurrentProgress = new QuestProgress(quest);
        CurrentProgress.Status = QuestProgress.QuestStatus.InProgress;
        OnStartQuest?.Invoke(CurrentProgress);
        Debug.Log($"Missão '{quest.title}' iniciada com status {CurrentProgress.Status}");
       
    }

    public void CompleteQuest()
    {
        if (!CurrentProgress.IsValid()) return;
        
        CurrentProgress.Status = QuestProgress.QuestStatus.Completed;
        Debug.Log($"Missão '{CurrentProgress.QuestData.title}' concluída!");
        OnFinishQuest?.Invoke();
    }

    private void CheckQuestProgress()
    {
        if (CurrentProgress == null) return;
        if (CurrentProgress.Status != QuestProgress.QuestStatus.InProgress) return;

        int current = InventoryController.Instance.TotalPoints;
        Debug.Log($"O jogador tem '{InventoryController.Instance.TotalPoints}' pontos. Ele precisa de '{CurrentProgress.QuestData.Points}' pontos.");
        int goal = CurrentProgress.QuestData.Points;

        if (current >= goal)
        {
            CompleteQuest();
        }
    }
}

