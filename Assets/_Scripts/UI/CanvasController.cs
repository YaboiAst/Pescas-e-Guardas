using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField] protected GameObject _canvas;
    [SerializeField] protected CanvasGroup _canvasGroup;
    
    protected bool IsOpen => _canvas.activeSelf;

    protected void ShowCanvas()
    {
        _canvas.SetActive(true);
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1;
    }
    
    protected void HideCanvas()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0;
        _canvas.SetActive(false);
    }
}
