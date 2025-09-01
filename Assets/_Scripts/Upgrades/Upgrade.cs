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
        if (ctx?.FishesByRarity == null)
            return;
        if (!ctx.FishesByRarity.TryGetValue(_rarity, out var count) || count <= 0)
            return;

        List<FishData> fishes = ctx.Fishes.FindAll(f => f.Rarity == _rarity);

        int add = 0;

        // if (_flatPerFish != 0)
        //     add += _flatPerFish * count;

        foreach (FishData fish in fishes)
        {
            add += _flatPerFish;
            add += Mathf.RoundToInt(fish.BasePoints * (_multPerFish - 1f) );
        }

        ctx.Add += add;
    }
}


[System.Serializable]
public class LuckEffect : Effect
{
    [Range(-100, 100)][SerializeField] private float _chanceIncrease = 10f;

    public LuckEffect() { }
    public LuckEffect(float chanceIncrease)
    {
        _chanceIncrease = chanceIncrease;
    }

    public override void OnEquip()
    {
        FishingManager.LuckChance += _chanceIncrease / 100;
    }

    public override void OnUnequip()
    {
        FishingManager.LuckChance -= _chanceIncrease / 100;
    }
}