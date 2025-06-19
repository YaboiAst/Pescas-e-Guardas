using UnityEngine;

[CreateAssetMenu(menuName = "Boat/New Boat Data")]
public class BoatTypeData : ScriptableObject
{
    public string DisplayName;
    public int HealthPoints;
    public float MoveSpeed;
    public float TurnSpeed;
}
