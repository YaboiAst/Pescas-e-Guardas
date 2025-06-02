using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Fish/New Fish")]
public class FishData : ScriptableObject
{
    [SerializeField, ReadOnly] private string _uniqueID = System.Guid.NewGuid().ToString();
    
    [SerializeField] private string _displayName;
    [TextArea]
    [SerializeField] private string _description;
    [SerializeField] private FishRarity _rarity;
    [SerializeField] private float _basePoints;
    [SerializeField] private GameObject _fishPrefab;
    [SerializeField] private Sprite _icon;
    [MinMaxSlider(0.1f, 10.0f)]
    [SerializeField] private Vector2 _weightRange;
    //[EnumFlags]
    [SerializeField] private FishLocation _location;
    
    public string UniqueID => _uniqueID;
    public string DisplayName => _displayName;
    public string Description => _description;
    public FishRarity Rarity => _rarity;
    public float BasePoints => _basePoints;
    public GameObject FishPrefab => _fishPrefab;
    public Sprite Icon => _icon;
    public float MinWeight => _weightRange.x;
    public float MaxWeight => _weightRange.y;
    public FishLocation Location => _location;
    
    
     #if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(_uniqueID))
        {
            _uniqueID = System.Guid.NewGuid().ToString();
    #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
    #endif
        }
    }
    #endif
    
    // public bool ContainsLocation(FishLocation location)
    // {
    //     return (_possibleLocations & location) == location && location != FishLocation.None;
    // }
    //
    // public FishLocation GetRandomLocation()
    // {
    //     Array values = System.Enum.GetValues(typeof(FishLocation));
    //     List<FishLocation> possible = new System.Collections.Generic.List<FishLocation>();
    //     foreach (FishLocation loc in values)
    //     {
    //         if (loc != FishLocation.None && (_possibleLocations & loc) == loc)
    //             possible.Add(loc);
    //     }
    //     
    //     return possible.Count == 0 ? FishLocation.None : possible[Random.Range(0, possible.Count)];
    // }
}

// public enum FishLocation
// {
//     None = 0,
//     Ocean = 1 << 0,
//     Shallow = 1 << 1,
//     Mangrove = 1 << 2,
//     Volcanic = 1 << 3,
//     Ice = 1 << 4,
//     Abyss = 1 << 5
// }

public enum FishLocation
{
    Ocean,
    Shallow, 
    Mangrove,
    Volcanic,
    Ice,
    Abyss
}