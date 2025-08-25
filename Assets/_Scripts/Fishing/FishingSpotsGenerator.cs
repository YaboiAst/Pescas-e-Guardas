using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class FishingSpotsGenerator : MonoBehaviour
{
    [SerializeField] private GameObjectTable _table;
    //[SerializeField] private GameObject _fishingSpotPrefab;
    [SerializeField] private Location _location;
    [SerializeField] private int _numberOfSpots = 10;
    [SerializeField] private float _range = 100f;
    [SerializeField] private float _fishingSpotDistance = 30f;

    private readonly List<GameObject> _spots = new List<GameObject>();


    private void OnValidate() => _table.ValidateTable();

    private void Start()
    {
        GenerateFishingSpots();
        QuestManager.OnStartQuest.AddListener(QuestStarted);
    }
    private void QuestStarted(QuestProgress progress) => GenerateFishingSpots();

    private void ClearFishingSpots()
    {
        foreach (GameObject spot in _spots)
            ObjectPoolManager.ReturnObjectToPool(spot);

        _spots.Clear();
    }


    [Button]
    public void GenerateFishingSpots()
    {
        ClearFishingSpots();

        List<Vector3> positions = PoissonDiskSample(_numberOfSpots, _range, _fishingSpotDistance);

        foreach (Vector3 position in positions)
        {
            var spotItem = _table.GetLootDropItem();
            GameObject spot = ObjectPoolManager.SpawnGameObject(spotItem.Item, position, Quaternion.identity);

            if (spot.TryGetComponent<FishingSpot>(out FishingSpot fishingSpot))
                fishingSpot.UpdateFishingSpot(_location);

            _spots.Add(spot);
        }
    }

    private List<Vector3> PoissonDiskSample(int count, float range, float minDist, int maxAttempts = 30)
    {
        List<Vector3> points = new List<Vector3>();
        List<Vector3> spawnPoints = new List<Vector3>();

        Vector3 center = transform.position;
        spawnPoints.Add(center);

        while (points.Count < count && spawnPoints.Count > 0)
        {
            int spawnIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            Vector3 spawnCenter = spawnPoints[spawnIndex];
            bool candidateAccepted = false;

            for (int i = 0; i < maxAttempts; i++)
            {
                float angle = UnityEngine.Random.value * Mathf.PI * 2;
                float radius = UnityEngine.Random.Range(minDist, 2 * minDist);
                Vector3 dir = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
                Vector3 candidate = spawnCenter + dir * radius;

                if (Vector3.Distance(candidate, center) > range)
                    continue;

                bool valid = true;
                foreach (Vector3 pt in points)
                {
                    if (Vector3.Distance(candidate, pt) < minDist)
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid && IsPositionValid(candidate))
                {
                    points.Add(candidate);
                    spawnPoints.Add(candidate);
                    candidateAccepted = true;
                    break;
                }
            }
            if (!candidateAccepted)
                spawnPoints.RemoveAt(spawnIndex);
        }

        return points;
    }

    private bool IsPositionValid(Vector3 position) => !Physics.CheckSphere(position, 30f, LayerMask.GetMask($"Obstacle"));

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {

        GUIStyle style = new GUIStyle
        {
            fontSize = 15,
            normal = { textColor = Color.white },
            alignment = TextAnchor.MiddleCenter,
        };

        Handles.Label(transform.position + Vector3.up * 15f, $"Fishing Spot Generator: {_location}", style);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(_range * 2, 1, _range * 2));
    }
#endif
}