using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    private List<Tab> _tabButtons;
    private Tab _selectedTab;
    public List<GameObject> ObjectsToSwap;

    public void Subscribe(Tab tab)
    {
        _tabButtons ??= new List<Tab>();

        _tabButtons.Add(tab);
    }

    public void OnTabEnter(Tab tab)
    {
        ResetTabs();
        if (_selectedTab == null || tab != _selectedTab)
            tab.Hover();
    }

    public void OnTabExit(Tab tab)
    {
        ResetTabs();
    }

    public void OnTabSelected(Tab tab)
    {
        if (_selectedTab != null)
            _selectedTab.Deselect();

        _selectedTab = tab;

        tab.Select();

        ResetTabs();

        var index = tab.transform.GetSiblingIndex();

        for (var i = 0; i < ObjectsToSwap.Count; i++)
        {
            ObjectsToSwap[i].SetActive(i == index);
        }
    }

    private void ResetTabs()
    {
        foreach (var tab in _tabButtons.Where(tab => _selectedTab == null || tab != _selectedTab))
        {
            tab.Deselect();
        }
    }
    
    public void NextTab()
    {
        var currentIndex = _selectedTab.transform.GetSiblingIndex();
        var nextIndex = currentIndex < _tabButtons.Count - 1 ? currentIndex + 1 : _tabButtons.Count - 1;
        OnTabSelected(_tabButtons[nextIndex]);
    }

    public void PreviousTab()
    {
        var currentIndex = _selectedTab.transform.GetSiblingIndex();
        var previousIndex = currentIndex > 0 ? currentIndex - 1 : 0;
        OnTabSelected(_tabButtons[previousIndex]);
    }
}