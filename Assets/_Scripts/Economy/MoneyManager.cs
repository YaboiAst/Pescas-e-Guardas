using System;
using PedronsaDev.Console;
using UnityEngine;

public class MoneyManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private int _currentMoney = 0;

    public static event Action<int> OnMoneyChange;

    [Command("set_money", "Sets the current money amount to value")]
    public void SetMoneyAmount(int amount)
    {
        int diff = _currentMoney - amount; 
        _currentMoney = amount;
        OnMoneyChange?.Invoke(diff);
    }

    [Command("modify_money", "Modifies the current money amount by value")]
    public void ModifyMoneyAmount(int amount)
    {
        _currentMoney += amount;
        
        OnMoneyChange?.Invoke(amount);
    }

    public bool TrySpend(int amount)
    {
        if (CanBuy(amount))
        {
            ModifyMoneyAmount(-amount);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanBuy(int value) => value <= _currentMoney;

    public int GetCurrentMoneyAmount() => _currentMoney;

    public void LoadData(GameData data)
    {
        _currentMoney = data.MoneyAmount;
    }

    public void SaveData(GameData data)
    {
        data.MoneyAmount = _currentMoney;
    }
}