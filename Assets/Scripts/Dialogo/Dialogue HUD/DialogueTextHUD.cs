using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTextHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueTextBox;
    [SerializeField] private TextMeshProUGUI dialogueTextName;
    [SerializeField] private Image dialogueIcon; 
    
    private void Awake()
    {
        DialogueManager.OnDialogueEvent.AddListener(WriteText);
        DialogueManager.OnFinishDialogue.AddListener(() => {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            gameObject.SetActive(false);
        });
        
        dialogueTextName.text = "";
        dialogueTextBox.text = "";
        
        this.gameObject.SetActive(false);
    }

    private void WriteText(MyDialogueInfo textToWrite)
    {
        this.gameObject.SetActive(true);

        dialogueIcon.color = Color.white;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        dialogueIcon.sprite = textToWrite.whosTalkingIcon;
        if(dialogueIcon.sprite == null) dialogueIcon.color = Color.clear;
        
        dialogueTextName.text = textToWrite.whosTalking;
        dialogueTextBox.text = textToWrite.textLine; // letter by letter
    }

    public void CallNextDialogue()
    {
        // Debug.Log("NEXT");
        DialogueManager.OnNextDialogue?.Invoke();
    }
}
