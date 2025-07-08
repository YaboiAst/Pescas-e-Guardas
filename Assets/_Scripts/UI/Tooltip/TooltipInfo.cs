using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[Serializable]
public struct TooltipInfo
{
    public Sprite Icon;
    public string Header;
    [ReorderableList]
    public List<TooltipElementInfo> Elements;
    [ReorderableList]
    public List<TooltipActionInfo> Actions;
    [TextArea]
    public string Content;
}

[Serializable]
public struct TooltipElementInfo
{
    public string Name;
    public string Value;
}

[Serializable]
public struct TooltipActionInfo
{
    public string Action;
    public Sprite Prompt;
}