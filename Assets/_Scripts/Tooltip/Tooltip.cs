using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    [SerializeField] private GameObject _headerParent;
    [SerializeField] private Image _headerIcon;
    [SerializeField] private TextMeshProUGUI _headerField;
    [SerializeField] private TextMeshProUGUI _contentField;
    [SerializeField] private Transform _elementsParent;
    [SerializeField] private GameObject _elementPrefab;
    [SerializeField] private LayoutElement _layoutElement;
    [SerializeField] private int _characterWrapLimit;
    [SerializeField] private RectTransform _avoidPanel;
    
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        CanvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start() => CanvasGroup.alpha = 0;

    private void LateUpdate()
    {
        Vector2 mousePosition = Input.mousePosition;
        
        if (IsMouseOverRect(_avoidPanel, mousePosition))
        {
            Vector3[] worldCorners = new Vector3[4];
            _avoidPanel.GetWorldCorners(worldCorners);

            float panelCenterX = (worldCorners[0].x + worldCorners[2].x) * 0.5f;
            float clampedY = Mathf.Clamp(mousePosition.y, worldCorners[0].y, worldCorners[1].y);

            bool panelOnLeft = panelCenterX < Screen.width / 2f;

            if (panelOnLeft)
            {
                _rectTransform.pivot = new Vector2(0, 0.5f); // left center
                _rectTransform.position = new Vector3(worldCorners[2].x, clampedY, 0);
            }
            else
            {
                _rectTransform.pivot = new Vector2(1, 0.5f); // right center
                _rectTransform.position = new Vector3(worldCorners[0].x, clampedY, 0);
            }
        }
        else
        {
            float pivotX = mousePosition.x / Screen.width;
            float pivotY = mousePosition.y / Screen.height;

            _rectTransform.pivot = new Vector2(pivotX < 0.50f ? -0.1f : 1.01f, pivotY < 0.75f ? 0f : 1f);
            _rectTransform.position = mousePosition;
        }
    }

    private bool IsMouseOverRect(RectTransform rect, Vector2 screenPosition)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(rect, screenPosition, null);
    }

    public void SetInfo(TooltipInfo info)
    {
        ReturnElements();
        
        if (string.IsNullOrEmpty(info.Header))
        {
            _headerParent.gameObject.SetActive(false);
        }
        else
        {
            _headerParent.gameObject.SetActive(true);
            _headerField.text = info.Header;
        }

        if (!info.Icon)
        {
            _headerIcon.gameObject.SetActive(false);
        }
        else
        {
            _headerIcon.sprite = info.Icon;
            _headerIcon.gameObject.SetActive(true);
        }
        
        _contentField.text = info.Content;

        int headerLength = _headerField.text.Length;
        int contentLength = _contentField.text.Length;

        if (info.Elements.Count > 0)
        {
            _elementsParent.gameObject.SetActive(true);
            CreateElements(info.Elements);
        }
        else
        {
            _elementsParent.gameObject.SetActive(false);
        }

        _layoutElement.enabled = (headerLength > _characterWrapLimit || contentLength > _characterWrapLimit);
    }

    private void CreateElements(List<TooltipElementInfo> infos)
    {
        foreach (TooltipElementInfo info in infos)
        {
            TooltipElement element = ObjectPoolManager.SpawnGameObject(_elementPrefab, _elementsParent, Quaternion.identity).GetComponent<TooltipElement>();
            element.SetElementInfo(info);
        }
    }

    private void ReturnElements()
    {
        for (int i = _elementsParent.childCount - 1; i >= 0; i--) 
            ObjectPoolManager.ReturnObjectToPool(_elementsParent.GetChild(i).gameObject); 
    }
    
}