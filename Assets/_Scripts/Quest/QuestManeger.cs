using System;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public QuestProgress currentProgress { get; private set; }

    //eventos 
    public static readonly UnityEvent OnFinishQuest = new();
    public static readonly UnityEvent<QuestProgress> OnStartQuest = new UnityEvent<QuestProgress>();

    //adicionar evento do progresso 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void AddQuest(MyQuestInfo quest)
    {
        currentProgress = new QuestProgress(quest);
        OnStartQuest?.Invoke(currentProgress);
        Debug.Log($"Missão '{quest.title}' iniciada com status {currentProgress.Status}");
    }

    public void CompleteQuest()
    {
        if (!currentProgress.IsValid()) return;
        
        currentProgress.Status = QuestProgress.QuestStatus.Completed;
        Debug.Log($"Missão '{currentProgress.QuestData.title}' concluída!");
        OnFinishQuest?.Invoke();
    }
}

