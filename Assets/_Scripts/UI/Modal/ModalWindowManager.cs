using UnityEngine;

public class ModalWindowManager : MonoBehaviour
{
    public static ModalWindowManager Instance;

    [SerializeField] private ModalWindowUI _modalWindowUI;

    public ModalWindowUI ModalWindow => _modalWindowUI;
    
    private void Awake()
    {
        Instance = this;
    }
}
