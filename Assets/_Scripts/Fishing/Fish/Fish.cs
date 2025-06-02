using System;
using Random = UnityEngine.Random;

[Serializable]
public class Fish
{
    public FishData FishData { get; private set; }
    public float Weight { get; private set; }
    public float Points { get; private set; }
    public DateTime TimeCaught { get; private set; }
    
    public FishLocation LocationCaught { get; private set; }

    public Fish(FishData fishData)
    {
        FishData = fishData;
        Weight = (float)Math.Round(Random.Range(fishData.MinWeight, fishData.MaxWeight), 1);
        Points = fishData.BasePoints;
        TimeCaught = System.DateTime.Now;
    }
}

