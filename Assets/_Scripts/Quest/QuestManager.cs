using System;
using UnityEngine;
using UnityEngine.Events;


public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }
    public QuestProgress CurrentProgress { get; private set; }

    public ScriptableDialogue nextQuest;

    public static UnityEvent OnFinishQuest = new();

    public static readonly UnityEvent<QuestProgress> OnStartQuest = new UnityEvent<QuestProgress>();

    private bool IsComplete = false;

      private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InventoryController.OnProgressQuest.AddListener(CheckQuestProgress);
        DialogueInteraction.OnInteracted.AddListener(CheckQuestIsCompleted);
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
        
        CurrentProgress.Status = QuestProgress.QuestStatus.Completed;
        Debug.Log($"Missão '{CurrentProgress.QuestData.title}' concluída!");
        OnFinishQuest?.Invoke();

        UpdateQuest();
    }
        

    private void CheckQuestProgress()
    {
        if (CurrentProgress == null) return;
        if (CurrentProgress.Status != QuestProgress.QuestStatus.InProgress) return;

        int current = InventoryPoints.Instance.TotalPoints;
        int goal = CurrentProgress.QuestData.Points;
        Debug.Log($"O jogador tem '{current}' pontos. Ele precisa de '{goal}' pontos.");

        if (current >= goal)
        {
            IsComplete = true;
        }
    }

    private void CheckQuestIsCompleted()
    {
        if (IsComplete)
        {
            CompleteQuest();
        }
    }

    private void UpdateQuest()
    {
        DialogueInteraction di = DialogueInteraction.Instance;

        if (di == null) return;

        int maxWorldLevelIndex = di.dialoguesGroups.Count - 1;
               

        if ( di.currentWorldLevelIndex <= maxWorldLevelIndex)
        {
            int maxDialogueIndex = di.dialoguesGroups[di.currentWorldLevelIndex].dialogues.Count - 1;
            if (di.currentDialogueIndex < maxDialogueIndex)
            {
                di.currentDialogueIndex++;
            }
            else
            {
                di.currentWorldLevelIndex++;
                di.currentDialogueIndex = 0;
            }
        }
        else
        {
            Debug.Log("Todas as quests foram completadas");
        }

    }
}