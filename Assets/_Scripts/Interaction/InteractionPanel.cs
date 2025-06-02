using System.Collections;
using TMPro;
using UnityEngine;

public class InteractionPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text _hintText;
    [SerializeField] private TMP_Text _completedText;

    private void OnEnable()
    {
        _completedText.gameObject.SetActive(false);
        _hintText.enabled = false;
        var interactionManager = FindFirstObjectByType<InteractionManager>(FindObjectsInactive.Exclude);
        interactionManager.CurrentInteractableChanged += UpdateInteractionText;
        Interactable.AnyInteractionComplete += ShowCompletedInteractionText;
    }
    
    private void OnDestroy()
    {
        Interactable.InteractablesInRangeChanged -= UpdateHintTextState;
        Interactable.AnyInteractionComplete -= ShowCompletedInteractionText;
    }

    private void UpdateInteractionText(Interactable interactable)
    {
        if (!interactable)
        {
            _hintText.enabled = false;
        } 
        else
        {
            var interactionType = interactable.InteractionType;
            _completedText.SetText(interactionType.CompletedInteraction);
            _hintText.enabled = true;
        }
    }

    private void ShowCompletedInteractionText(Interactable interactable, string completedInspectionMessage)
    {
        _completedText.SetText(completedInspectionMessage);
        _completedText.gameObject.SetActive(true);
        float messageTime = completedInspectionMessage.Length / 5f;
        messageTime = Mathf.Clamp(messageTime, 2f, 10f);
        StartCoroutine(FadeCompletedText(messageTime));
    }

    private IEnumerator FadeCompletedText(float messageTime)
    {
        _completedText.alpha = 1f;

        while (_completedText.alpha > 0)
        {
            yield return null;
            _completedText.alpha -= Time.deltaTime / messageTime;
        }

        _completedText.gameObject.SetActive(false);
    }
    private void UpdateHintTextState(bool enableHint)
    {
        _hintText.enabled = enableHint;
    }
}
