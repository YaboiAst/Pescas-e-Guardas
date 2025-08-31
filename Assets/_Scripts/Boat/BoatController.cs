using System;
using Cinemachine;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [SerializeField] private BoatTypeData _debugBoatType;
    [SerializeField] private BoatData _boatData;
    
    private BoatHealth _health;
    private BoatMovement _movement;
    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        _health = GetComponent<BoatHealth>();
        _movement = GetComponent<BoatMovement>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        _boatData.BoatTypeData = _debugBoatType;
        SetupBoat();
        _health.OnBoatDestroy += DestroyBoat;
    }

    private void OnDestroy() => _health.OnBoatDestroy -= DestroyBoat;

    private void SetupBoat()
    {
        _health.SetupHealth(_boatData.BoatTypeData, _boatData.Health);
        _movement.SetupMovement(_boatData.BoatTypeData);
        //transform.position = _boatData.Position;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            DamageBoat();
    }

    private void DamageBoat()
    {
        _health.TakeDamage();
        _impulseSource.GenerateImpulse();
    }

    private void DestroyBoat()
    {
        Debug.Log("Boat Destroyed");
    }
}

[Serializable]
public class BoatData
{
    public BoatTypeData BoatTypeData;
    public Vector3 Position = Vector3.zero;
    public int Health = 0;
}
