using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tab : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup TabGroup;


    [SerializeField] private bool _enableEvents;

    public UnityEvent OnTabSelected;
    public UnityEvent OnTabDeselected;

    [SerializeField] private TabController _tabController;
    private Image _backgroundImage;

    private void Start()
    {
        _backgroundImage = GetComponent<Image>();
        this.TabGroup.Subscribe(this);
        _tabController.Deselect();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.TabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.TabGroup.OnTabExit(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.TabGroup.OnTabSelected(this);
    }


    public void Hover()
    {
        _tabController.Hover();
    }

    public void Deselect()
    {
        _tabController.Deselect();
        OnTabDeselected?.Invoke();
    }

    public void Select()
    {
        _tabController.Select();
        OnTabSelected?.Invoke();
    }
}