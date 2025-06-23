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

public class DraggableItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool _isDragging = false;
    private bool _isRotating = false;

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
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!_isDragging) _isDragging = true;
            
            this.transform.DOMove(Input.mousePosition, .05f)
                .SetEase(Ease.OutBounce);

            _targetVisual.raycastTarget = false;
            OnBeginDrag?.Invoke(this._placer);
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_isDragging) _isDragging = false;
            
            OnFinishDrag?.Invoke();
            if (eventData.pointerEnter is null ||
                !eventData.pointerEnter.gameObject.CompareTag("Grid") ||
                !InventoryController.Instance.IsPositionValid())
            {
                _placer.PlaceItem(_targetVisual, _itemMemento.LastSafePosition, _itemMemento.LastSafeAngle);
                return;
            }

            _placer.SetItemInGrid();
            _placer.PlaceItem(_targetVisual, _placer.GetRoot().position, _targetVisual.transform.eulerAngles.z);
            _itemMemento.SetMemento(this.transform.position, this.transform.eulerAngles.z);
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging)
            return;

        var isOnGrid = (eventData.pointerEnter is not null) && (eventData.pointerEnter.CompareTag("Grid"));
        _placer.SetPositionStatus(isOnGrid);

        this._targetVisual.transform.position = Input.mousePosition;
        
        if (!isOnGrid)
        {
            _placer.GetRoot().position = _targetVisual.transform.position;
        }
        else
        {
            InventoryController.CheckGrid(_placer);
        }
    }
    
    private void Update()
    {
        if (!_isDragging)
            return;
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
            .OnComplete(() =>
            {
                _isRotating = false;
                InventoryController.CheckGrid(_placer);
            });
        _placer.GetRoot().Rotate(Vector3.forward, -90);
    }
}
