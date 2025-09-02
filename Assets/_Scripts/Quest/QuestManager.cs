using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[System.Serializable]
public class QuestBucket
{
    public int worldLevel;
    public List<ScriptableDialogue> dialogues;
}

public class QuestManager : MonoBehaviour
{
    [SerializeField] public List<QuestBucket> dialoguesGroups;
    public static QuestManager Instance { get; private set; }
    public QuestProgress CurrentProgress { get; private set; }

    [ReadOnly] public int currentWorldLevelIndex = 0;
    [ReadOnly] public int currentDialogueIndex = 0;

    public static bool HasQuestActive => Instance?.CurrentProgress is { Status: QuestProgress.QuestStatus.InProgress };
    public static bool QuestCompleted => Instance?.CurrentProgress is { Status: QuestProgress.QuestStatus.Completed };

    public ProgressBar bar;

    [ReadOnly] public float progress = 0;

    public static readonly UnityEvent OnFinishQuest = new();

    public static readonly UnityEvent OnNextQuest = new();

    public static readonly UnityEvent<QuestProgress> OnStartQuest = new UnityEvent<QuestProgress>();

    [FormerlySerializedAs("IsComplete")] [ReadOnly] public bool isComplete = false;
    [ReadOnly] public bool isClaimed = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CurrentProgress = null;
        OnNextQuest.AddListener(NextQuest);
        OnFinishQuest.AddListener(CompleteQuest);

        InventoryController.OnProgressQuest.AddListener(CheckQuestProgress);
        DialogueInteraction.OnInteracted.AddListener(CheckQuestIsCompleted);
        DialogueManager.OnFinishDialogue.AddListener(() => isClaimed = !isClaimed);
    }

    public static void ParseInteraction()
    {
        if (HasQuestActive)
            return;

        if (QuestCompleted)
            OnFinishQuest?.Invoke();
        else 
            OnNextQuest?.Invoke();
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
        if (!CurrentProgress.IsValid())
            return;

        bar.AlterarProgresso(0);

        CurrentProgress.Status = QuestProgress.QuestStatus.Claimed;
        Debug.Log($"Missão '{CurrentProgress.QuestData.title}' concluída!");
        // OnFinishQuest?.Invoke();

        isComplete = false;
        isClaimed = false;
        DialogueManager.OnNextDialogueBlock?.Invoke(1);
    }


    private void CheckQuestProgress()
    {
        if (CurrentProgress == null) return;
        if (CurrentProgress.Status != QuestProgress.QuestStatus.InProgress) return;

        int current = InventoryPoints.Instance.TotalPoints;
        int goal = CurrentProgress.QuestData.objectivePoints;
        Debug.Log($"O jogador tem '{current}' pontos. Ele precisa de '{goal}' pontos.");

        progress = (float)current / goal;

        bar.AlterarProgresso(progress);

        if (current >= goal)
        {
            isComplete = true;
            CurrentProgress.Status = QuestProgress.QuestStatus.Completed;
        }
    }

    private void NextQuest()
    {
        if (CurrentProgress == null)
        {
            currentWorldLevelIndex = 0;
            currentDialogueIndex = 0;
        }
        else 
        {
            if (!isClaimed)
                return;
            UpdateQuest();
        }
        var currentDialogue = dialoguesGroups[currentWorldLevelIndex].dialogues[currentDialogueIndex];
        AddQuest(currentDialogue.questToStart.questInfo);
        DialogueManager.OnStartDialogue?.Invoke(currentDialogue);
    }
    private void CheckQuestIsCompleted()
    {
        if (isComplete)
        {
            CompleteQuest();
        }
    }

    private void UpdateQuest()
    {
   
        int maxWorldLevelIndex = dialoguesGroups.Count - 1;
               

        if (currentWorldLevelIndex <= maxWorldLevelIndex)
        {
            int maxDialogueIndex = dialoguesGroups[currentWorldLevelIndex].dialogues.Count - 1;
            if (currentDialogueIndex < maxDialogueIndex)
            {
                currentDialogueIndex++;
            }
            else
            {
                currentWorldLevelIndex++;
                currentDialogueIndex = 0;

                if (currentWorldLevelIndex > maxWorldLevelIndex)
                {
                    Debug.Log("Fim");
                    ConditionOverlay.OnFinish?.Invoke();
                }
            }
        }
        else
        {
            Debug.Log("Todas as quests foram completadas");
        }

    }
}