using DG.Tweening;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private RectTransform _background;
    [SerializeField] private bool _animateBackground = false;

    private void Start()
    {
        MoneyManager.OnMoneyChange += UpdateUI;
        UpdateUI(0,MoneyManager.Instance.GetCurrentMoneyAmount());
    }
    private void UpdateUI(int previousMoneyAmount, int newMoneyAmount)
    {
        if (_animateBackground)
            _background.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutQuint).OnComplete(() => TextAnimation(previousMoneyAmount, newMoneyAmount));
        else
            TextAnimation(previousMoneyAmount, newMoneyAmount);
    }
    private void TextAnimation(int previousMoneyAmount, int newMoneyAmount)
    {
        int startValue = previousMoneyAmount;
        int endValue = newMoneyAmount;
        DOTween.To(() => startValue, x => {
            startValue = x;
            _text.SetText("$" + x);
        }, endValue, 1f).SetEase(Ease.OutQuint).OnComplete(HideBackground);
    }
    private void HideBackground()
    {
        if (_animateBackground)
            _background.DOAnchorPosX(-500, 0.5f).SetEase(Ease.InQuint);
    }
}
