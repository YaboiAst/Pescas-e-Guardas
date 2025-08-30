using System;
using PedronsaDev.Console;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{


    public static MoneyManager Instance { get; private set; }

    [SerializeField] private int _currentMoney = 0;

    public static event Action<int, int> OnMoneyChange;

    private void Awake() => Instance = this;

    [Command("set_money", "Sets the current money amount to value")]
    public void SetMoneyAmount(int amount)
    {
        int prev = _currentMoney;
        _currentMoney = amount;
        OnMoneyChange?.Invoke(prev, _currentMoney);
    }

    [Command("modify_money", "Modifies the current money amount by value")]
    public void ModifyMoneyAmount(int amount)
    {
        int prev = _currentMoney;
        _currentMoney += amount;
        
        OnMoneyChange?.Invoke(prev, _currentMoney);
    }

    public void AddMoney(int amount) => ModifyMoneyAmount(amount);
    public void RemoveMoney(int amount) => ModifyMoneyAmount(-amount);

    public bool TrySpend(int amount)
    {
        if (CanBuy(amount))
        {
            ModifyMoneyAmount(-amount);
            return true;
        }

        return false;
    }

    public bool CanBuy(int value) => value <= _currentMoney;

    public int GetCurrentMoneyAmount() => _currentMoney;
}