using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModalWindowUI : CanvasController
{
    [Header("Header")] 
    [SerializeField] private Transform _headerArea;
    [Space] 
    [SerializeField] private TMP_Text _headerTextField;


    [Header("Content")] 
    [SerializeField] private Transform _contentArea;
    [Space] 
    [SerializeField] private Transform _verticalLayoutArea;
    [SerializeField] private Image _heroImage;
    [SerializeField] private TMP_Text _heroTextField;
    [Space] 
    [SerializeField] private Transform _horizontalLayoutArea;
    [SerializeField] private Transform _iconContainerImage;
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _iconTextField;

    [Header("Footer")] 
    [SerializeField] private Transform _footerArea;
    [Space] 
    [SerializeField] private Button _confirmButton;
    [SerializeField] private TMP_Text _confirmTextField;
    [SerializeField] private Button _declineButton;
    [SerializeField] private TMP_Text _declineTextField;
    [SerializeField] private Button _alternateButton;
    [SerializeField] private TMP_Text _alternateTextField;

    private Action onConfirmCallback;
    private Action onDeclineCallback;
    private Action onAlternateCallback;

    private void Start() => HideCanvas();

    public void Confirm()
    {
        onConfirmCallback?.Invoke();
        HideCanvas();
    }

    public void Decline()
    {
        onDeclineCallback?.Invoke();
        HideCanvas();
    }

    public void Alternate()
    {
        onAlternateCallback?.Invoke();
        HideCanvas();
    }

    public void ShowAsHero(string header, Sprite imageToShow, string message, string confirmMessage,
        string declineMessage, string alternateMessage, Action confirmAction,
        Action declineAction = null, Action alternateAction = null)
    {
        _verticalLayoutArea.gameObject.SetActive(true);
        _horizontalLayoutArea.gameObject.SetActive(false);

        bool hasHeader = string.IsNullOrEmpty(header);
        _headerArea.gameObject.SetActive(!hasHeader);
        _headerTextField.text = header;

        _heroImage.sprite = imageToShow;
        _heroTextField.text = message;

        onConfirmCallback = confirmAction;
        _confirmTextField.text = confirmMessage;

        bool hasDecline = (declineAction != null);
        _declineTextField.text = declineMessage;
        _declineButton.gameObject.SetActive(hasDecline);
        onDeclineCallback = declineAction;

        bool hasAlternate = (alternateAction != null);
        _alternateTextField.text = alternateMessage;
        _alternateButton.gameObject.SetActive(hasAlternate);
        onAlternateCallback = alternateAction;

        ShowCanvas();
    }

    public void ShowAsHero(string header, Sprite imageToShow, string message, Action confirmAction)
    {
        ShowAsHero(header, imageToShow, message, "Continue", "", "", confirmAction);
    }

    public void ShowAsHero(string header, Sprite imageToShow, string message, Action confirmAction,
        Action declineAction)
    {
        ShowAsHero(header, imageToShow, message, "Continue", "Back", "", confirmAction, declineAction);
    }
    
    
    public void ShowAsPrompt(string header, Sprite imageToShow, string message, string confirmMessage = "OK",
        string declineMessage = "", string alternateMessage = "", Action confirmAction = null,
        Action declineAction = null, Action alternateAction = null)
    {
        _verticalLayoutArea.gameObject.SetActive(false);
        _iconImage.gameObject.SetActive(true);
        _horizontalLayoutArea.gameObject.SetActive(true);

        bool hasHeader = string.IsNullOrEmpty(header);
        _headerArea.gameObject.SetActive(!hasHeader);
        _headerTextField.text = header;

        _iconImage.sprite = imageToShow;
        _iconTextField.text = message;

        onConfirmCallback = confirmAction;
        _confirmTextField.text = confirmMessage;

        bool hasDecline = (declineAction != null);
        _declineTextField.text = declineMessage;
        _declineButton.gameObject.SetActive(hasDecline);
        onDeclineCallback = declineAction;

        bool hasAlternate = (alternateAction != null);
        _alternateTextField.text = alternateMessage;
        _alternateButton.gameObject.SetActive(hasAlternate);
        onAlternateCallback = alternateAction;

        ShowCanvas();
    }
    
    public void ShowAsDialog(string header, Sprite imageToShow, string message, string confirmMessage = "OK",
        string declineMessage = "", string alternateMessage = "", Action confirmAction = null,
        Action declineAction = null, Action alternateAction = null)
    {
        _verticalLayoutArea.gameObject.SetActive(false);
        _iconContainerImage.gameObject.SetActive(false);
        _horizontalLayoutArea.gameObject.SetActive(true);

        bool hasHeader = string.IsNullOrEmpty(header);
        _headerArea.gameObject.SetActive(!hasHeader);
        _headerTextField.text = header;

        _iconImage.sprite = imageToShow;
        _iconTextField.text = message;

        onConfirmCallback = confirmAction;
        _confirmTextField.text = confirmMessage;

        bool hasDecline = (declineAction != null);
        _declineTextField.text = declineMessage;
        _declineButton.gameObject.SetActive(hasDecline);
        onDeclineCallback = declineAction;

        bool hasAlternate = (alternateAction != null);
        _alternateTextField.text = alternateMessage;
        _alternateButton.gameObject.SetActive(hasAlternate);
        onAlternateCallback = alternateAction;

        ShowCanvas();
    }
}