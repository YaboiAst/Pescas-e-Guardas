using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class HeroWindowTrigger : MonoBehaviour
{
    public string Title;
    public string Message;
    public Sprite Sprite;
    public bool TriggerOnEnable;

    public UnityEvent onContinueEvent;
    public UnityEvent onCancelEvent;

    private Action _continueCallback = null;
    private Action _cancelCallback = null;

    private void OnEnable()
    {
        _continueCallback = null;
        _cancelCallback = null;

        if (onContinueEvent.GetPersistentEventCount() > 0)
        {
            _continueCallback = onContinueEvent.Invoke;
        }

        if (onCancelEvent.GetPersistentEventCount() > 0)
        {
            _cancelCallback = onCancelEvent.Invoke;
        }


        if (!TriggerOnEnable) return;

        Interact();
    }

    [Button("Test")]
    public void Interact()
    {
        ModalWindowManager.Instance.ModalWindow.ShowAsHero(Title, Sprite, Message, _continueCallback, _cancelCallback);
    }
}