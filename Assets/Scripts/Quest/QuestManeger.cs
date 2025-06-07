using System;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public MyQuestInfo currentQuest { get; private set; }
    public QuestProgress currentProgress { get; private set; }

    //eventos 
    public static readonly UnityEvent OnFinishQuest = new();
    public static readonly UnityEvent<MyQuestInfo> OnStartQuest = new UnityEvent<MyQuestInfo>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void updateQuest(MyQuestInfo quest)
    {
        currentQuest = quest;
        currentProgress = new QuestProgress(QuestProgress.questStatus.InProgress);
        Debug.Log($"Missão '{quest.title}' iniciada com status {currentProgress.status}");
        OnStartQuest?.Invoke(quest);
    }

    public void completeQuest()
    {
        if (currentQuest != null && currentProgress != null)
        {
            currentProgress.status = QuestProgress.questStatus.Completed;
            Debug.Log($"Missão '{currentQuest.title}' concluída!");
            OnFinishQuest?.Invoke();
        }
    }
}

