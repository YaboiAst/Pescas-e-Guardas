using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTriggerUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TooltipInfo _info;

    private Tween _call;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _call = DOVirtual.DelayedCall(1.3f, () => { TooltipSystem.Instance.Show(_info); });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_call != null)
        {
            _call.Kill();
            TooltipSystem.Instance.Hide();
        }
    }

    public void SetTooltipInfo(TooltipInfo info) => _info = info;
}