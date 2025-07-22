using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLockMode : MonoBehaviour
{
    private void Awake()
    {
        LockCursor();
    }

    public static void LockCursor()
    {
        Cursor.lockState = UnityEngine.CursorLockMode.Locked;
    }
    public static void UnlockCursor()
    {
        Cursor.lockState = UnityEngine.CursorLockMode.None;
    }
}
