using UnityEngine;

public class TreasureSpot : Spot
{
    //[SerializeField] private MinigameType _minigameType;

    protected override void Start()
    {
        FishingAttempts = 1;
        base.Start();
    }

    public override void Interact()
    {
        FishingManager.StartFishing(this);
        base.Interact();
    }
    public override void OnMinigameComplete(MinigameResult result)
    {
        if (result == MinigameResult.Won)
        {
            MoneyManager.Instance.AddMoney(1000);
        }
        else
        {

        }
    }
}
