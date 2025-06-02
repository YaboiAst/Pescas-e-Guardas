using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour
{
    [SerializeField] private TooltipInfo _info;

    private Tween _call;
    
    private void OnMouseEnter()
    {
        if (IsMouseOverUI())
            return;
        
        _call = DOVirtual.DelayedCall(1.3f, () => { TooltipSystem.Instance.Show(_info); });
    }
    
    private void OnMouseExit()
    {
        if (_call != null)
        {
            _call.Kill();
            TooltipSystem.Instance.Hide();
        }
    }
    
    public void SetTooltipInfo(TooltipInfo info) => _info = info;

    private bool IsMouseOverUI() => EventSystem.current.IsPointerOverGameObject();
}