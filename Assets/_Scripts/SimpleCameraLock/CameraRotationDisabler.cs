using UnityEngine;

public class CameraRotationDisabler : MonoBehaviour
{
    public Camera targetCamera;
    
    // The component that handles camera rotation
    private MonoBehaviour rotationScript;

    private void Start()
    {
        if (targetCamera != null)
        {
            rotationScript = targetCamera.GetComponent<MonoBehaviour>();
        }
    }
    
    public void DisableCameraRotation()
    {
        if (rotationScript != null)
        {
            rotationScript.enabled = false;
        }
        else if (targetCamera != null)
        {
            Debug.LogWarning("No camera rotation script found on the target camera");
        }
    }

    // Call this to enable camera rotation again
    public void EnableCameraRotation()
    {
        if (rotationScript != null)
        {
            rotationScript.enabled = true;
        }
    }

    // Toggle rotation on/off
    public void ToggleCameraRotation()
    {
        if (rotationScript != null)
        {
            rotationScript.enabled = !rotationScript.enabled;
        }
    }
}