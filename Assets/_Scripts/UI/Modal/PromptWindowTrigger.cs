using UnityEngine;

public class PromptWindowTrigger : MonoBehaviour
{
    public string Title;
    public string Message;
    public Sprite Sprite;

    public void Interact()
    {
        ModalWindowManager.Instance.ModalWindow.ShowAsPrompt(Title, Sprite, Message);
    }
}
