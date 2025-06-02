using System.Collections.Generic;
using PedronsaDev.Console;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public static List<FishData> AllFishes;
    
    private void OnValidate()
    {
        AllFishes = new List<FishData>(Resources.LoadAll<FishData>("Fishes"));
    }

    [Command("discover_all_fishes", "Discovers all fishes and add all diary entries")]
    public void DiscoverAllFishes()
    {
        foreach (FishData fishData in AllFishes)
        {
            Fish fish = new Fish(fishData);
            DiaryManager.Instance.RegisterFish(fish);
        }
    }
}
