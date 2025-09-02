using UnityEngine;
public class Spot : MonoBehaviour
{
    private Interactable _interactable;
    protected int FishingAttempts = 3;

    protected virtual void Start() => _interactable = GetComponent<Interactable>();

    public virtual void UseFishingAttempt()
    {
        FishingAttempts--;
        if (FishingAttempts < 0)
            FishingAttempts = 0;
    }

    public virtual bool CanFish() => FishingAttempts > 0;
    public virtual bool IsTreasureSpot() => false;

    public virtual void Interact()
    {
        _interactable.RemoveFromRange();
    }
    public void DestroySpot()
    {
        _interactable.RemoveFromRange();
        ObjectPoolManager.ReturnObjectToPool(this.gameObject);
        //Destroy(this);
    }

    public virtual void OnMinigameComplete(MinigameResult result)
    {

    }
    public void ShowInteraction() => _interactable.AddToRange();
}
