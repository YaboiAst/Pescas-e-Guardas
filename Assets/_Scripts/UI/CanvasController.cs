using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] protected GameObject _canvas;
    [SerializeField] protected CanvasGroup _canvasGroup;

    [SerializeField] private bool _disableGO = true;

    protected bool IsOpen;

    protected void ShowCanvas()
    {
        if (_disableGO)
        {
            _canvas.SetActive(true);
        }
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1;
        IsOpen = true;
    }
    
    protected void HideCanvas()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0;
        if (_disableGO)
        {
            _canvas.SetActive(false);
        }

        IsOpen = false;
    }

    protected void ToggleCanvas()
    {
        if (IsOpen)
            HideCanvas();
        else
            ShowCanvas();
    }
}
