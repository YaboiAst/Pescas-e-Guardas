using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UpgradeObjectUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Upgrade _upgrade;
    [SerializeField] private Sprite _promptIcon;
    [SerializeField] private Image _image;
    private TooltipTriggerUI _tooltip;


    public void SetItem(Upgrade upgrade)
    {
        _tooltip = GetComponent<TooltipTriggerUI>();
        _upgrade = upgrade;
        _nameText.text = upgrade.DisplayName;

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

    public void OnButtonPressed()
    {
        _tooltip.KillTooltip();
        ModalWindowManager.Instance.ModalWindow.ShowAsPrompt(
            "Remover Upgrade",
            _promptIcon,
            $"Deseja remover o upgrade {_upgrade.DisplayName}?",
            "Sim",
            "Não", confirmAction: () =>
        {
            UpgradeManager.Unequip(_upgrade);
            GetComponentInParent<UpgradeUI>().UpgradeUnequipped(this);
            Destroy(gameObject);
        }, declineAction: () => {  });
    }
}
