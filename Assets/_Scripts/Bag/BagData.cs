using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bag", menuName = "Inventory/New Bag")]
public class BagData : ScriptableObject
{
    [Tooltip("O inventário estará sempre na escala 3:4")] 
    [MinValue(1), MaxValue(4)]
    public int scale;
    
    public Sprite bagMask;
}
