using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMouseLook : MonoBehaviour
{
    [Header("Configurações da Câmera")]
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    
    [Header("Configurações de UI")]
    public Canvas[] uiCanvases; // Array de Canvases para verificar
    
    private float xRotation = 0f;
    private bool isUIActive = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        
        // Se não houver Canvases atribuídos, encontrar todos automaticamente
        if (uiCanvases == null || uiCanvases.Length == 0)
        {
            uiCanvases = FindObjectsOfType<Canvas>();
        }
    }

    void Update()
    {
        CheckUIActive();
        
        if (!isUIActive)
        {
            HandleCameraRotation();
        }
        
        HandleCursorLock();
    }

    void CheckUIActive()
    {
        isUIActive = false;
        
        // Verifica se o EventSystem está com algum objeto selecionado
        if (EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null)
        {
            isUIActive = true;
            return;
        }
        
        // Verifica se o cursor está sobre UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            isUIActive = true;
            return;
        }
        
        // Verifica se qualquer Canvas está ativo e interativo
        foreach (Canvas canvas in uiCanvases)
        {
            if (canvas != null && canvas.gameObject.activeInHierarchy && canvas.enabled)
            {
                // Verifica se o Canvas é do tipo World Space (não bloqueia movimento)
                if (canvas.renderMode == RenderMode.ScreenSpaceOverlay || 
                    canvas.renderMode == RenderMode.ScreenSpaceCamera)
                {
                    isUIActive = true;
                    return;
                }
            }
        }
    }

    void HandleCameraRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
        if (playerBody != null)
        {
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    void HandleCursorLock()
    {
        if (isUIActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Método para adicionar Canvas manualmente (útil para UI dinâmica)
    public void AddUICanvas(Canvas canvas)
    {
        if (uiCanvases == null)
        {
            uiCanvases = new Canvas[] { canvas };
        }
        else
        {
            Canvas[] newArray = new Canvas[uiCanvases.Length + 1];
            uiCanvases.CopyTo(newArray, 0);
            newArray[uiCanvases.Length] = canvas;
            uiCanvases = newArray;
        }
    }

    // Método para remover Canvas
    public void RemoveUICanvas(Canvas canvas)
    {
        if (uiCanvases != null && uiCanvases.Length > 0)
        {
            System.Collections.Generic.List<Canvas> canvasList = new System.Collections.Generic.List<Canvas>(uiCanvases);
            canvasList.Remove(canvas);
            uiCanvases = canvasList.ToArray();
        }
    }
}