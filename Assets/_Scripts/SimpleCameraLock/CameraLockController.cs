using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class CameraLockController : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook mainCamera;
    [SerializeField] private CinemachineVirtualCamera lockedCamera;

    private Transform _freelookFollow, _freelookLookAt;
    
    private void Awake()
    {
        mainCamera.Priority = 10;
        lockedCamera.Priority = 5;
    }

    private void Start()
    {
        CanvasController.OnUIOpen.AddListener(LockCamera);
        CanvasController.OnUIClosed.AddListener(UnlockButton);
    }

    private void LockCamera()
    {
        _freelookFollow ??= mainCamera.Follow;
        _freelookLookAt ??= mainCamera.LookAt;

        mainCamera.Follow = null;
        mainCamera.LookAt = null;

        mainCamera.Priority = 5;
        lockedCamera.Priority = 10;

        Cursor.lockState = UnityEngine.CursorLockMode.None;
    }

    private void UnlockCamera()
    {
        mainCamera.Follow ??= _freelookFollow;
        mainCamera.LookAt ??= _freelookLookAt;

        mainCamera.Priority = 10;
        lockedCamera.Priority = 5;
        
        Cursor.lockState = UnityEngine.CursorLockMode.Locked;
    }

    [Button("Lock")]
    public void LockButton() => LockCamera();
    [Button("Unlock")]
    public void UnlockButton() => UnlockCamera();
}
