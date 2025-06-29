using System.Collections.Generic;
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
    [SerializeField] private int _basePoints;
    [SerializeField] private Sprite _icon;
    [MinMaxSlider(0.1f, 10.0f)]
    [SerializeField] private Vector2 _weightRange;
    //[EnumFlags]
    [SerializeField] private Location _location;
    
    public string UniqueID => _uniqueID;
    public string DisplayName => _displayName;
    public string Description => _description;
    public FishRarity Rarity => _rarity;
    public int BasePoints => _basePoints;
    //public GameObject FishPrefab => _fishPrefab;
    public Sprite Icon => _icon;
    public float MinWeight => _weightRange.x;
    public float MaxWeight => _weightRange.y;
    public Location Location => _location;
    
    [Header("Shape")] 
    public bool HasCustomShape = false;

    [Range(1, 10)] 
    public int Width = 1;

    [Range(1, 10)] 
    public int Height = 1;

    public List<ShapeRow> Shape = new List<ShapeRow>();

    public bool IsPartOfShape(int x, int y)
    {
        if (y < 0 || y >= Height || x < 0 || x >= Width)
            return false;

        if (y < Shape.Count && x < Shape[y].Cols.Count)
            return Shape[y].Cols[x];

        return false;
    }
    
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
        
        while (Shape.Count < Height)
        {
            Shape.Add(new ShapeRow());
        }
        while (Shape.Count > Height)
        {
            Shape.RemoveAt(Shape.Count - 1);
        }

        for (int i = 0; i < Shape.Count; i++)
        {
            while (Shape[i].Cols.Count < Width)
                Shape[i].Cols.Add(false);
            while (Shape[i].Cols.Count > Width)
                Shape[i].Cols.RemoveAt(Shape[i].Cols.Count - 1);
        }

        if (HasCustomShape)
            return;

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (y < Shape.Count && x < Shape[y].Cols.Count)
                {
                    Shape[y].Cols[x] = true;
                }
            }
        }
    }
#endif
}

[System.Serializable]
public class ShapeRow
{
    public List<bool> Cols = new List<bool>();
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

public enum Location
{
    Ocean,
    Shallow, 
    Mangrove,
    Volcanic,
    Ice,
    Abyss
}