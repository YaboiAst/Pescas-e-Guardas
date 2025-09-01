using UnityEngine;
using System.Collections.Generic;

public abstract class GenericLootDropTable<T,U> where T: GenericLootDropItem<U>
{
    [SerializeField] public List<T> LootDropItems;

    private float _probabilityTotalWeight;

    private float _luckModifier = 0f;

    public void ValidateTable()
    {
        if(LootDropItems != null && LootDropItems.Count > 0)
        {
            float currentProbabilityWeightMaximum = 0f;

            foreach(T lootDropItem in LootDropItems)
            {
                if(lootDropItem.ProbabilityWeight < 0f)
                {
                    lootDropItem.ProbabilityPercent = 0f;
                }
                else
                {
                    lootDropItem.ProbabilityRangeFrom = currentProbabilityWeightMaximum;
                    currentProbabilityWeightMaximum += lootDropItem.ProbabilityWeight;
                    lootDropItem.ProbabilityRangeTo = currentProbabilityWeightMaximum;
                }
            }

            _probabilityTotalWeight = currentProbabilityWeightMaximum;

            foreach(T lootDropItem in LootDropItems)
            {
                lootDropItem.ProbabilityPercent = (lootDropItem.ProbabilityWeight / _probabilityTotalWeight) * 100;
            }
        }        
    }

    public T GetLootDropItem(float luckModifier)
    {
        _luckModifier = Mathf.Clamp(luckModifier, -1f, 1f);

        float pickedNumber = Random.Range(0, _probabilityTotalWeight);

        pickedNumber += _luckModifier * (_probabilityTotalWeight / 4f);
        pickedNumber = Mathf.Clamp(pickedNumber, 0, _probabilityTotalWeight);

        foreach(T lootDropItem in LootDropItems)
        {
            if(pickedNumber > lootDropItem.ProbabilityRangeFrom && pickedNumber < lootDropItem.ProbabilityRangeTo)
                return lootDropItem;
        }

        return LootDropItems[0];
    }
}