using System;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class FishLootTable : GenericLootDropTable<FishItem,FishData>
{
    public void GenerateNewLootTable(Location locationType)
    {
        ClearLootTable();
        
        foreach (var fishData in FishManager.AllFishes.Where(t => t.Location == locationType))
        {
            if (fishData)
            {
                FishItem item = new FishItem();
                LootDropItems.Add(item);
            }
        }
    }

    private void ClearLootTable()
    {
        LootDropItems.Clear();
    }
    public void UpdateLootTable(List<FishData> randomFishes)
    {
        ClearLootTable();

        foreach (FishData fishData in randomFishes)
        {
            if (fishData)
            {
                FishItem item = new FishItem();

                int weight = fishData.Rarity switch
                {
                    FishRarity.Common => 50,
                    FishRarity.Uncommon => 30,
                    FishRarity.Rare => 15,
                    FishRarity.Epic => 4,
                    FishRarity.Legendary => 1,
                    _ => throw new ArgumentOutOfRangeException()
                };

                item.ProbabilityWeight = weight;
                item.Item = fishData;
                LootDropItems.Add(item);
            }
        }
    }
}