using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTutorial : MonoBehaviour
{
    public GameObject Indicador;
    [SerializeField] private List<ScriptableDialogue> dialogues;
    

    private void Start()
    {
        Debug.Log("a");
        DialogueManager.OnStartDialogue?.Invoke(dialogues[0]);
        DialogueManager.OnFinishDialogue.AddListener(() => {Indicador.SetActive(true);});
    }

  }
