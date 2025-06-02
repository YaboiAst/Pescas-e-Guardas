using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;

public abstract class GenericLootDropItem<T>
{
    public T Item;
    public float ProbabilityWeight;

    [ProgressBar("Prob%", 100, EColor.Blue)]
    public float ProbabilityPercent;

    [HideInInspector]
    public float ProbabilityRangeFrom;
    [HideInInspector]
    public float ProbabilityRangeTo;
}