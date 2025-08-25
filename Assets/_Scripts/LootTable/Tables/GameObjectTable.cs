using UnityEngine;

[System.Serializable]
public class GameObjectTable : GenericLootDropTable<GameObjectItem,GameObject>
{
    public void GenerateNewLootTable()
    {
        ClearLootTable();
    }

    private void ClearLootTable()
    {
        LootDropItems.Clear();
    }
}
