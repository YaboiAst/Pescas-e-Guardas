using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] protected TMP_Text _text;

    private void Start()
    {
        MoneyManager.OnMoneyChange += UpdateUI;
        UpdateUI(0, MoneyManager.Instance.GetCurrentMoneyAmount());
    }
    protected virtual void UpdateUI(int previousMoneyAmount, int newMoneyAmount)
    {
        TextAnimation(previousMoneyAmount, newMoneyAmount);
    }
    protected virtual void TextAnimation(int previousMoneyAmount, int newMoneyAmount)
    {
        int startValue = previousMoneyAmount;
        int endValue = newMoneyAmount;
        DOTween.To(() => startValue, x =>
        {
            startValue = x;
            _text.SetText("$" + x);
        }, endValue, 1f).SetEase(Ease.OutQuint).OnComplete(AnimationFinished);
    }
    protected virtual void AnimationFinished()
    {

    }
}
