using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool _isDragging = false;
    private bool _isRotating = false;

    private ItemPlacer _placer;
    private Image _targetVisual;
    
    private void Awake()
    {
        _targetVisual = GetComponentInChildren<Image>();
        _placer = GetComponent<ItemPlacer>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (!_isDragging) _isDragging = true;
            
            this.transform.DOMove(Input.mousePosition, .05f)
                .SetEase(Ease.OutBounce);

            _targetVisual.raycastTarget = false;
        }
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_isDragging) _isDragging = false;

            _targetVisual.raycastTarget = true;
            if (eventData.pointerEnter is null) return;
            if (eventData.pointerEnter.gameObject.CompareTag("Grid"))
            {
                transform.position = _placer.GetRoot().position;
                _placer.GetRoot().position = transform.position;
                _targetVisual.transform.position = transform.position;
            }
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging)
            return;
        
        this._targetVisual.transform.position = Input.mousePosition;
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
                InventoryController.CheckOverlap?.Invoke(_placer);
            });
        _placer.GetRoot().Rotate(Vector3.forward, -90);
    }
}
