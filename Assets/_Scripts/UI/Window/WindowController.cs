using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class WindowController : MonoBehaviour
{
   public UnityEvent OnOpen;
   public UnityEvent OnClose;

    [SerializeField] private Vector3 _targetOpenScale = Vector3.one;
    [SerializeField] private Vector3 _targetCloseScale = Vector3.zero;

    private CanvasGroup _canvasGroup;
    private bool _isOpen = false;

    private void Awake() => _canvasGroup = GetComponent<CanvasGroup>();

    private void Start()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        transform.localScale = _targetCloseScale;
        _isOpen = false;
    }

    public void ToggleWindow()
    {
        if (_isOpen)
            CloseWindow();
        else
            OpenWindow();
    }

    [ContextMenu("[DEBUG] Open Window")]
    public void OpenWindow()
    {
        if (_isOpen)
            return;
        
        WindowManager.Instance.AddWindow(this);
        
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        transform.DOScaleY(_targetOpenScale.y, 0.15f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            _isOpen = true;
        });

        OnOpen?.Invoke();
    }
    
    [ContextMenu("[DEBUG] Close Window")]
    public void CloseWindow()
    {
        if (!_isOpen)
            return;
        
        StartCoroutine(WindowManager.Instance.RemoveWindow(this));
        
        transform.DOScaleY(_targetCloseScale.y, 0.15f).SetEase(Ease.InBack).OnComplete(() =>
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            _isOpen = false;
        
            OnClose?.Invoke();
        });
    }
}