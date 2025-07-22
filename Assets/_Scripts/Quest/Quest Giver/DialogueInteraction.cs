using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueGroup
{
    public int worldLevel;
    public List<ScriptableDialogue> dialogues;
}

public class DialogueInteraction : MonoBehaviour
{
    public static DialogueInteraction Instance;

    public int currentWorldLevelIndex = 0;
    public int currentDialogueIndex = 0;

    [SerializeField] public List<DialogueGroup> dialoguesGroups;

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
        OnInteracted.Invoke();
        DialogueManager.OnStartDialogue?.Invoke(dialoguesGroups[currentWorldLevelIndex].dialogues[currentDialogueIndex]);
    }
}
