using UnityEngine;

[System.Serializable]
public class MyQuestInfo
{
    public string questGiver;
    public string title;
    public int pointReward;
    public int points;

    [TextArea] public string description;
    public string objectiveDescription;
}
