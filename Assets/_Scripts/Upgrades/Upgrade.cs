using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrades/New Upgrade", order = 0)]
public class Upgrade : ScriptableObject
{
    public Sprite Icon;
    public string DisplayName;
    [TextArea] public string Description;
    public int Cost;
    [SerializeReference]public List<Effect> Effects;

    public void EquipEffect()
    {
        foreach (Effect effect in Effects)
            effect.OnEquip();
    }

    public void UnequipEffect()
    {
        foreach (Effect effect in Effects)
            effect.OnUnequip();
    }
}

[System.Serializable]
public abstract class Effect
{
    public virtual Effect GetEffect() => this;
    public virtual void OnEquip() { }
    public virtual void OnUnequip() { }

    public virtual void OnStartFishing() { }
    public virtual void OnEndFishing() { }
    public virtual void OnFishCaught(FishData fish) { }

    public virtual void OnScore(PointsContext ctx) { }
}

[System.Serializable]
public class FlatBonusEffect : Effect
{
    [SerializeField] private int _amount = 10;

    public FlatBonusEffect() { }
    public FlatBonusEffect(int amount) { this._amount = amount; }

    public override void OnScore(PointsContext ctx)
    {
        ctx.Add += _amount;
    }
}


[System.Serializable]
public class MultiplierEffect : Effect
{
    [SerializeField] private float _multiplier = 1.5f;

    public MultiplierEffect() { }
    public MultiplierEffect(float multiplier) { this._multiplier = multiplier; }

    public override void OnScore(PointsContext ctx)
    {
        ctx.Mult *= _multiplier;
    }
}

[System.Serializable]
public class FishRarityEffect : Effect
{
    [SerializeField] private FishRarity _rarity;
    [SerializeField] private int _flatPerFish = 0;
    [SerializeField] private float _multPerFish = 1f;

    public FishRarityEffect() { }
    public FishRarityEffect(FishRarity rarity, int flatPerFish = 0, float multPerFish = 1f)
    {
        _rarity = rarity;
        _flatPerFish = flatPerFish;
        _multPerFish = multPerFish;
    }

    public override void OnScore(PointsContext ctx)
    {
        if (ctx?.Fishes == null) return;
        if (!ctx.Fishes.TryGetValue(_rarity, out var count) || count <= 0) return;

        if (_flatPerFish != 0)
            ctx.Add += _flatPerFish * count;

        if (!Mathf.Approximately(_multPerFish, 1f) && _multPerFish > 0f)
            ctx.Mult *= Mathf.Pow(_multPerFish, count);
    }
}


[System.Serializable]
public class LuckEffect : Effect
{
    [SerializeField] private float _chanceIncrease = 0.1f;

    public LuckEffect() { }
    public LuckEffect(float chanceIncrease)
    {
        _chanceIncrease = chanceIncrease;
    }

    public override void OnEquip()
    {
        FishingManager.LuckChance += _chanceIncrease;
    }

    public override void OnUnequip()
    {
        FishingManager.LuckChance -= _chanceIncrease;
    }
}