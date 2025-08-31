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
            if (fishData != null)
            {
                var item = new FishItem();
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

        foreach (var fishData in randomFishes)
        {
            if (fishData != null)
            {
                var item = new FishItem();

                int weight = 0;
                switch (fishData.Rarity)
                {
                    case FishRarity.Common:
                        weight = 50;
                        break;
                    case FishRarity.Uncommon:
                        weight = 30;
                        break;
                    case FishRarity.Rare:
                        weight = 15;
                        break;
                    case FishRarity.Epic:
                        weight = 4;
                        break;
                    case FishRarity.Legendary:
                        weight = 1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                item.ProbabilityWeight = weight;
                LootDropItems.Add(item);
            }
        }
    }
}