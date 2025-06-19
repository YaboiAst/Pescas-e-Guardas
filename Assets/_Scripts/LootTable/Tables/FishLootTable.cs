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
}