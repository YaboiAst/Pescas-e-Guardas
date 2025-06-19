using System.Collections.Generic;
using PedronsaDev.Console;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public static List<FishData> AllFishes;
    [SerializeField] private List<FishData> _datas;
    
    private void OnValidate()
    {
        AllFishes = new List<FishData>(Resources.LoadAll<FishData>("Fishes"));
        _datas = AllFishes;
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

    // public FishData GetFishByID(string id)
    // {
    //     
    // }
}
