using UnityEngine;

[CreateAssetMenu(menuName = "Interactable/Iteraction Type" )]
public class InteractionType : ScriptableObject
{
    public KeyCode Hotkey = KeyCode.E;
    //[Space(15)]
    //public string BeforeInteraction;
    //public string DuringInteraction;
    public string CompletedInteraction;
    public string FailedInteraction;

    public bool IsDefault;
}