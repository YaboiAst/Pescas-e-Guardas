using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _priceText;
    [SerializeField] private Upgrade _upgrade;
    [SerializeField] private Image _image;
    private TooltipTriggerUI _tooltip;

    public Upgrade Upgrade => _upgrade;
    public int Cost => _upgrade.Cost;

    private void OnEnable() => _tooltip = GetComponent<TooltipTriggerUI>();

    public void SetItem(Upgrade upgrade)
    {
        _upgrade = upgrade;
        _nameText.text = upgrade.DisplayName;
        _priceText.text = $"${upgrade.Cost}";

        TooltipInfo tooltipInfo = new TooltipInfo
        {
            Header = _upgrade.DisplayName,
            Content = _upgrade.Description,
            Actions = new List<TooltipActionInfo>(),
            Elements = new List<TooltipElementInfo>()
        };

        _image.sprite = upgrade.Icon;

        _tooltip.SetTooltipInfo(tooltipInfo);
    }

    public void OnBuyButtonPressed()
    {
        _tooltip.KillTooltip();

        if(MoneyManager.Instance.TrySpend(Cost) && UpgradeManager.Equip(_upgrade))
            Destroy(gameObject);
    }
}
