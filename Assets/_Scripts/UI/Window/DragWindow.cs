using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragWindow : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform _draggableWindow;
    [SerializeField] private Canvas _canvas;
    private RectTransform _canvasRect;

    private void Awake()
    {
        if (!_draggableWindow)
            _draggableWindow = transform.parent.GetComponent<RectTransform>();

        if (!_canvas)
        {
            Transform testCanvasTransform = transform.parent;

            while (testCanvasTransform)
            {
                _canvas = testCanvasTransform.GetComponent<Canvas>();
                if (_canvas)
                    break;

                testCanvasTransform = testCanvasTransform.parent;
            }
        }
    }

    private void Start() => _canvasRect = _canvas.GetComponent<RectTransform>();

    public void OnDrag(PointerEventData eventData)
    {
        _draggableWindow.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _draggableWindow.anchoredPosition = _canvasRect.FullyContainsMoveInside_SSC(_draggableWindow);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _draggableWindow.SetAsLastSibling();
    }
}