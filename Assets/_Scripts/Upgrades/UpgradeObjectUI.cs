using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UpgradeObjectUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private Upgrade _upgrade;
    [SerializeField] private Sprite _promptIcon;
    private TooltipTriggerUI _tooltip;

    public Upgrade Upgrade => _upgrade;
    public int Cost => _upgrade.Cost;

    private void OnEnable() => _tooltip = GetComponent<TooltipTriggerUI>();

    public void SetItem(Upgrade upgrade)
    {
        _upgrade = upgrade;
        _nameText.text = upgrade.DisplayName;

        TooltipInfo tooltipInfo = new TooltipInfo
        {
            Header = _upgrade.DisplayName,
            Content = _upgrade.Description,
            Actions = new List<TooltipActionInfo>(),
            Elements = new List<TooltipElementInfo>()
        };

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
