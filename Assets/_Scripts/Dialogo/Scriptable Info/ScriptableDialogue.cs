using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class DialogueEvent
{
    public enum DialogueEventType { Dialogue };

    public DialogueEventType type;

    [ShowIf("type", DialogueEventType.Dialogue)] [AllowNesting]
    public MyDialogueInfo textLine;
    
}
[System.Serializable]
public class DialogueBlock
{
    [AllowNesting] public List<DialogueEvent> dialogueBlock;

    [Space(5)]
    public bool overrideJump;
    [ShowIf(nameof(overrideJump)), AllowNesting] public int jumpToBlock;

    
}

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/New Dialogue")]
public class ScriptableDialogue : ScriptableObject
{
    public List<DialogueBlock> dialogueBlocks;
}