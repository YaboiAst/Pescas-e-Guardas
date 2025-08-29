using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] private ScriptableDialogue _dialogue;


    public void Interact()
    {
        DialogueManager.OnStartDialogue?.Invoke(_dialogue);
    }
}
