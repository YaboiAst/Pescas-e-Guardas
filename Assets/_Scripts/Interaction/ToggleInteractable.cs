using System;
using UnityEngine;

public class ToggleInteractable : Interactable
{
    [SerializeField] private InteractionType _toggledInteractionType;
    
    private bool _toggleState => _interactionCount % 2 == 1;

    public override InteractionType InteractionType => _toggleState ? _toggledInteractionType : _interactionType;
}