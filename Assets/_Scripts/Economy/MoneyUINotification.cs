using DG.Tweening;
using UnityEngine;
public class MoneyUINotification : MoneyUI
{
    [SerializeField] private RectTransform _background;

    protected override void AnimationFinished()
    {
        _background.DOAnchorPosX(-500, 0.5f).SetEase(Ease.InQuint);
    }

    protected override void UpdateUI(int previousMoneyAmount, int newMoneyAmount)
    {
        if (ShopController.ShopOpen)
            return;

        _background.DOAnchorPosX(0, 0.5f).SetEase(Ease.OutQuint).OnComplete(() => TextAnimation(previousMoneyAmount, newMoneyAmount));
    }
}
