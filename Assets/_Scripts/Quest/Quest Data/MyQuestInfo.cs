using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class MyQuestInfo
{
    [Header("Quest description")]
    public string questGiver;
    public string title;
    [TextArea] public string description;
    public string objectiveDescription;
    
    [Header("Quest info")] 
    [FormerlySerializedAs("Points")] 
    public int objectivePoints;
    public BagData questBag;
    public float rewardValue;
}
