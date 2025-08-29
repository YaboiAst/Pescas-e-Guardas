using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueInteraction : MonoBehaviour
{
    public static DialogueInteraction Instance;   

    public static readonly UnityEvent OnInteracted = new();

    private bool _canInteract;
    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Interact()
    {
        
        if (QuestManager.HasQuestActive)
        {
            return;
        }

        if (QuestManager.QuestCompleted)
        {
            QuestManager.OnFinishQuest?.Invoke();
        }
        else 
        {
            QuestManager.OnNextQuest?.Invoke();
        }
                    
    }
}
