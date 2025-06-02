using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInteraction : MonoBehaviour
{
    [SerializeField] private List<ScriptableDialogue> dialogues;
    private bool _canInteract;
    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        Debug.Log("testeA");
        _canInteract = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        Debug.Log("testeA");
        _canInteract = false;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _canInteract) {
            DialogueManager.OnStartDialogue?.Invoke(dialogues[0]);
        }
    }
}
