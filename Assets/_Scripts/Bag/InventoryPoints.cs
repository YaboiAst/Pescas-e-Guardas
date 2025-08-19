using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPoints : MonoBehaviour
{
    public static InventoryPoints Instance { get; private set; }

    [SerializeField] private PointsContext _currentContext = new PointsContext(0);

    private readonly List<Effect> _activeEffects = new List<Effect>();

    public int TotalPoints => _currentContext.FinalScore;

    public static Action OnPointsUpdated;

    private void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void RegisterUpgrade(Upgrade upgrade)
    {
        if (!upgrade || upgrade.Effects == null) return;
        foreach (Effect e in upgrade.Effects)
        {
            if (e == null) continue;
            _activeEffects.Add(e);
            e.OnEquip();
        }

        CalculateScore(_currentContext.BasePoints);
    }

    public void UnregisterUpgrade(Upgrade upgrade)
    {
        if (upgrade == null || upgrade.Effects == null) return;
        foreach (Effect e in upgrade.Effects)
        {
            if (e == null) continue;
            e.OnUnequip();
            _activeEffects.Remove(e);
        }

        CalculateScore(_currentContext.BasePoints);
    }

    public void NewFishAdded(List<FishData> fishes)
    {
        SerializableDictionary<FishRarity, int> dic = new SerializableDictionary<FishRarity, int>()
        {
            { FishRarity.Common, 0 },
            { FishRarity.Uncommon, 0 },
            { FishRarity.Rare, 0 },
            { FishRarity.Epic, 0 },
            { FishRarity.Legendary, 0 }
        };

        foreach (FishData fish in fishes)
        {
            dic[fish.Rarity]++;
        }
        _currentContext.Fishes = dic;
    }

    public int CalculateScore(int bp)
    {
        if (bp == 0)
            return 0;

        _currentContext.BasePoints = bp;

        foreach (Effect e in _activeEffects)
            e.OnScore(_currentContext);

        OnPointsUpdated?.Invoke();

        return _currentContext.FinalScore;
    }

    public void NotifyStartFishing()      { foreach (var e in _activeEffects) e.OnStartFishing(); }
    public void NotifyEndFishing()        { foreach (var e in _activeEffects) e.OnEndFishing(); }
    public void NotifyFishCaught(FishData fish) { foreach (var e in _activeEffects) e.OnFishCaught(fish); }
}

[Serializable]
public class PointsContext
{
    public int BasePoints = 0;
    public int Add;
    public float Mult = 1f;

    public PointsContext(int basePoints) => BasePoints = basePoints;

    [SerializeField]
    public SerializableDictionary<FishRarity, int> Fishes = new SerializableDictionary<FishRarity, int>();

    public int FinalScore => Mathf.RoundToInt((BasePoints + Add) * Mult);
}

[System.Serializable]
public class SerializableDictionary<K, V> : Dictionary<K, V>, ISerializationCallbackReceiver
{
    [SerializeField]
    private List<K> m_Keys = new List<K>();

    [SerializeField]
    private List<V> m_Values = new List<V>();

    public void OnBeforeSerialize()
    {
        m_Keys.Clear();
        m_Values.Clear();
        using Enumerator enumerator = GetEnumerator();
        while (enumerator.MoveNext())
        {
            KeyValuePair<K, V> current = enumerator.Current;
            m_Keys.Add(current.Key);
            m_Values.Add(current.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        Clear();
        for (int i = 0; i < m_Keys.Count; i++)
        {
            Add(m_Keys[i], m_Values[i]);
        }

        m_Keys.Clear();
        m_Values.Clear();
    }
}
