using System;
using UnityEngine;
using UnityEngine.Events;

public class CanvasController : MonoBehaviour
{
    [SerializeField] protected GameObject _canvas;
    [SerializeField] protected CanvasGroup _canvasGroup;

    [SerializeField] private bool _disableGO = true;

    //private static CanvasController _lastCanvasOpened;

    protected bool IsOpen;

    public static readonly UnityEvent OnUIOpen = new(), OnUIClosed = new();
    
    protected virtual void ShowCanvas(bool triggerEvent = true)
    {
        if (_disableGO)
        {
            _canvas.SetActive(true);
        }
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1;
        IsOpen = true;
        
        if (!triggerEvent) return;
        
        OnUIOpen?.Invoke();
    }
    
    protected virtual void HideCanvas(bool triggerEvent = true)
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.alpha = 0;
        if (_disableGO)
        {
            _canvas.SetActive(false);
        }
        IsOpen = false;
        
        if (!triggerEvent) return;
        
        OnUIClosed?.Invoke();
    }

    protected void ToggleCanvas()
    {
        if (IsOpen)
            HideCanvas();
        else
            ShowCanvas();
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Escape))
    //         _lastCanvasOpened.HideCanvas();
    // }
}
