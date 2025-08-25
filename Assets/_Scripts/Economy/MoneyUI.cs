using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private void Start()
    {
        MoneyManager.OnMoneyChange += UpdateUI;
        UpdateUI(MoneyManager.Instance.GetCurrentMoneyAmount());
    }
    private void UpdateUI(int moneyAmount)
    {
        _text.SetText("$" + MoneyManager.Instance.GetCurrentMoneyAmount());
    }
}
