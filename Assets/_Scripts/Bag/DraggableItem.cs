using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemMemento
{
    public Vector3 LastSafePosition;
    public float LastSafeAngle;

    public ItemMemento(Vector3 lastSafePosition, float lastSafeAngle)
    {
        LastSafePosition = lastSafePosition;
        LastSafeAngle = lastSafeAngle;
    }

    public void SetMemento(Vector3 newPos, float newRot)
    {
        this.LastSafePosition = newPos;
        this.LastSafeAngle = newRot;
    }
}

public class DraggableItem : MonoBehaviour, IPointerClickHandler
{
    private bool _isDragging = false;
    private bool _isRotating = false;
    
    private bool _isToggled = false;
    private bool _isOnGrid = false;
    
    private ItemMemento _itemMemento;
    private ItemPlacer _placer;
    private Image _targetVisual;

    public static readonly UnityEvent<ItemPlacer> OnBeginDrag = new();
    public static readonly UnityEvent OnFinishDrag = new();
    
    private void Awake()
    {
        _targetVisual = GetComponentInChildren<Image>();
        _placer = GetComponent<ItemPlacer>();
        _itemMemento = new ItemMemento(this.transform.position, this.transform.eulerAngles.z);
        
        OnBeginDrag.AddListener(SetItemInteraction);
        OnFinishDrag.AddListener(ResetItemInteraction);
    }
    
    private void SetItemInteraction(ItemPlacer activeItem)
    {
        if (this._placer == activeItem)
            return;

        _targetVisual.raycastTarget = false;
    }
    private void ResetItemInteraction()
    {
        _targetVisual.raycastTarget = true;
    }

    public void InitializeDrag()
    {
        _isToggled = true;
        InventoryController.SelectItem(_placer);
        HandleItemGrab();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        if (!_isToggled)
        {
            if (InventoryController.IsItemSelected())
                return;
            
            _isToggled = true;
            InventoryController.SelectItem(_placer);
            HandleItemGrab();
        }
        else
        {
            _isToggled = false;
            InventoryController.DeselectItem();
            HandleItemDrop();
        }
    }
    
    private void HandleItemGrab()
    {
        if (!_isDragging) _isDragging = true;
        
        this.transform.DOMove(Input.mousePosition, .05f)
            .SetEase(Ease.OutBounce);

        _targetVisual.raycastTarget = false;
        OnBeginDrag?.Invoke(this._placer);
    }

    private void HandleItemDrop()
    {
        if (_isDragging) _isDragging = false;
        
        OnFinishDrag?.Invoke();
        if (!InventoryController.Instance.IsPositionValid())
        {
            _placer.PlaceItem(_targetVisual, _itemMemento.LastSafePosition, _itemMemento.LastSafeAngle);
            return;
        }
        
        _placer.SetItemInGrid();
        _placer.PlaceItem(_targetVisual, _placer.GetRoot().position, _targetVisual.transform.eulerAngles.z);
        _itemMemento.SetMemento(this.transform.position, this.transform.eulerAngles.z);
    }

    private void DragItem()
    {
        if (!_isDragging)
            return;
        
        this._targetVisual.transform.position = Input.mousePosition;
        
        if (!_placer.GetPositionStatus())
            _placer.GetRoot().position = _targetVisual.transform.position;
    }
    
    private void Update()
    {
        if (!_isToggled)
            return;
        DragItem();
            
        if (!Input.GetMouseButtonUp(1))
            return;
        if (_isRotating)
            return;
        
        RotateOnce();
    }
    
    private void RotateOnce()
    {
        _isRotating = true;
        this._targetVisual.transform.DORotate(Vector3.forward * (-90), .1f, RotateMode.LocalAxisAdd)
            .OnStart(() => _placer.GetRoot().Rotate(Vector3.forward, -90))
            .OnComplete(() =>
            {
                _isRotating = false;
                InventoryController.CheckGrid(_placer);
            });
    }
}
