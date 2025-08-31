using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class TabController
{

    [SerializeField] private List<Graphic> _graphics;
    
    [SerializeField] private List<Color> _activeColors;
    [SerializeField] private List<Color> _inactiveColors;
    [SerializeField] private List<Color> _hoverColors;
    

    public void Select()
    {
        for (int i = 0; i < _graphics.Count; i++) 
            _graphics[i].color = _activeColors[i];
    }
    
    public void Hover()
    {
        for (int i = 0; i < _graphics.Count; i++) 
            _graphics[i].color = _hoverColors[i];
    }
    
    public void Deselect()
    {
        for (int i = 0; i < _graphics.Count; i++) 
            _graphics[i].color = _inactiveColors[i];
    }


}