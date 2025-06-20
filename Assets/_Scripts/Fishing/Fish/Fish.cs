using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Fish
{
    [SerializeField] private FishData _fishData;
    [SerializeField] private float _weight;
    [SerializeField] private int _points;
    //[SerializeField] private Location _locationCaught;

    public FishData FishData => _fishData;
    public float Weight => _weight;
    public int Points => _points;
    //public Location LocationCaught => _locationCaught;

    public DateTime TimeCaught;
    
    public Fish(FishData fishData)
    {
        _fishData = fishData;
        _weight = (float)Math.Round(Random.Range(fishData.MinWeight, fishData.MaxWeight), 1);
        _points = fishData.BasePoints;
        TimeCaught = System.DateTime.Now;
    }
}

