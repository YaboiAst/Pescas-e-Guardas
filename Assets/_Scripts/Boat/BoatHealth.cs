using System;
using PedronsaDev.Console;
using TMPro;
using UnityEngine;

public class BoatHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealthPoints;
    [SerializeField] private int _healthPoints;
    [SerializeField] private TMP_Text _healthText;
    
    public event Action OnBoatHealthUpdate; 
    public event Action OnBoatDestroy; 
    
    public void SetupHealth(BoatTypeData typeData, int health)
    {
        _maxHealthPoints = typeData.HealthPoints;
        SetHealth(health == 0 ? _maxHealthPoints : health);
    }
    
    public void TakeDamage(int damage = 1)
    {
        SetHealth(_healthPoints - damage);
        
        if (_healthPoints <= 0) 
            OnBoatDestroy?.Invoke();
    }
    
    [Command("reset_boat_health", "Resets the health of the boat to max health")]
    public void ResetHealth() => SetHealth(_maxHealthPoints);

    private void SetHealth(int health)
    {
        _healthPoints = health;
        
        OnBoatHealthUpdate?.Invoke();
        
        UpdateUI();
    }

    public int GetCurrentHealth() => _healthPoints;
    
    private void UpdateUI() => _healthText.SetText($"HP: {_healthPoints}/{_maxHealthPoints}");
}
