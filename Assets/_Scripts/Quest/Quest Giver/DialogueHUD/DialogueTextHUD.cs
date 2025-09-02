using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTextHUD : CanvasController
{
    [SerializeField] private TextMeshProUGUI dialogueTextBox;
    [SerializeField] private TextMeshProUGUI dialogueTextName;
    [SerializeField] private Image dialogueIcon; 
    
    private void Awake()
    {
        DialogueManager.OnDialogueEvent.AddListener(WriteText);
        DialogueManager.OnFinishDialogue.AddListener(() => {
            //Cursor.visible = false;
            //Cursor.lockState = UnityEngine.CursorLockMode.Locked;
            HideCanvas();
        });
        
        dialogueTextName.text = "";
        dialogueTextBox.text = "";
        
        HideCanvas();
    }

    private void WriteText(MyDialogueInfo textToWrite)
    {
        ShowCanvas();

        dialogueIcon.color = Color.white;
        Cursor.visible = true;
        Cursor.lockState = UnityEngine.CursorLockMode.Confined;

        dialogueIcon.sprite = textToWrite.whosTalkingIcon;
        if(dialogueIcon.sprite == null) dialogueIcon.color = Color.clear;
        
        dialogueTextName.text = textToWrite.whosTalking;
        dialogueTextBox.text = textToWrite.textLine; // letter by letter
    }

    public void CallNextDialogue()
    {
        DialogueManager.OnNextDialogue?.Invoke();
    }
}
