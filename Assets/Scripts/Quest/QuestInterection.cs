using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestInteraction : MonoBehaviour
{
    [SerializeField] private List<ScriptableDialogue> dialogues;
    private bool _canInteract;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _canInteract = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _canInteract = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && _canInteract)
        {
            QuestManager.Instance.completeQuest();
        }
    }
}
