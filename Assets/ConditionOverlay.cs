using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConditionOverlay : CanvasController
{
    [SerializeField] private CanvasGroup winCanvas, loseCanvas;
    [SerializeField] private Button winButton, loseButton;
    [SerializeField, Scene] private string menuScene;
    
    public static readonly UnityEvent OnFinish = new(), OnLose = new();

    private void Awake()
    {
        InitCanvasGroup(winCanvas);
        winButton.onClick.AddListener(BackToMenu);
        InitCanvasGroup(loseCanvas);
        loseButton.onClick.AddListener(BackToMenu);
        
        OnFinish.AddListener(OpenWinOverlay);
        OnLose.AddListener(OpenLoseOverlay);
    }
    
    private void OpenWinOverlay()
    {
        Fade(winCanvas);
    }
    
    private void OpenLoseOverlay()
    {
        Fade(loseCanvas);
    }

    private void InitCanvasGroup(CanvasGroup cg)
    {
        cg.interactable = false;
        cg.blocksRaycasts = false;
        cg.alpha = 0f;
    }
    
    private void Fade(CanvasGroup cg)
    {
        cg.DOFade(1f, .5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                cg.interactable = true;
                cg.blocksRaycasts = true;

                OnUIOpen?.Invoke();
            });
    }

    public void BackToMenu()
    {
        loseCanvas.interactable = false;
        winCanvas.interactable = false;
        
        SceneManager.LoadScene(menuScene);
    }

    [Button("Toggle Win")]
    public void ToggleWinButton()
    {
        if (!winCanvas.interactable)
            OpenWinOverlay();
        else
            InitCanvasGroup(winCanvas);
    }
    [Button("Toggle Lose")]
    public void ToggleLoseButton()
    {
        if (!loseCanvas.interactable)
            OpenLoseOverlay();
        else
            InitCanvasGroup(loseCanvas);
    }
}
