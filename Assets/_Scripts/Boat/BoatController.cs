using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    [SerializeField] private BoatTypeData _debugBoatType;
    [SerializeField] private BoatData _boatData;
    
    private BoatHealth _health;
    private BoatMovement _movement;
    private CinemachineImpulseSource _impulseSource;

    private bool _canCollide;
    
    private void Awake()
    {
        _health = GetComponent<BoatHealth>();
        _movement = GetComponent<BoatMovement>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _canCollide = true;
    }

    private void Start()
    {
        _boatData.BoatTypeData = _debugBoatType;
        SetupBoat();
    }
    
    private void SetupBoat()
    {
        _health.SetupHealth(_boatData.BoatTypeData, _boatData.Health);
        _movement.SetupMovement(_boatData.BoatTypeData);
        //transform.position = _boatData.Position;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!_canCollide)
            return;
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            DamageBoat();
    }

    private void DamageBoat()
    {
        _canCollide = false;
        _health.TakeDamage();
        var boundDir = transform.parent.position + (-transform.forward.normalized * 15f);
        transform.parent.DOMove(boundDir, 2f)
            .SetEase(Ease.OutQuart)
            .OnStart(() =>
            {
                _impulseSource.GenerateImpulse();
                _movement.EnableMove(false);
            })
            .OnComplete(() =>
            {
                _movement.EnableMove(true);
                _canCollide = true;
            });
    }
}

[Serializable]
public class BoatData
{
    public BoatTypeData BoatTypeData;
    public Vector3 Position = Vector3.zero;
    public int Health = 0;
}
