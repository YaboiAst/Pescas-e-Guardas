using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInteraction : MonoBehaviour
{
    [SerializeField] private List<ScriptableQuest> dialogues;
    private bool _canInteract;

    public void Interact()
    {
        QuestManager.Instance.CompleteQuest();
    }
}
