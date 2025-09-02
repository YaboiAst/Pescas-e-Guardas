using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _turnSpeed = 50f;
    [SerializeField] private LayerMask _landLayerMask;

    private bool _canMove = true;
    public void EnableMove(bool set) => _canMove = set;

    public void SetupMovement(BoatTypeData data)
    {
        _moveSpeed = data.MoveSpeed;
        _turnSpeed = data.TurnSpeed;

        CanvasController.OnUIOpen.AddListener(() => _canMove = false);
        CanvasController.OnUIClosed.AddListener(() => _canMove = true);
    }

    // private void OnEnable()
    // {
    //     Minigame.OnMinigameUpdated += MinigameUpdated;
    // }
    //
    // private void OnDestroy()
    // {
    //     Minigame.OnMinigameUpdated -= MinigameUpdated;
    // }
    //
    // private void MinigameUpdated(bool hasStarted)
    // {
    //     if(hasStarted)
    //     {
    //         _canMove = false;
    //     }else
    //     {
    //         _canMove = true;
    //     }
    // }
    
    private void Update()
    {
        if(!_canMove)
            return;

        float moveInput = Input.GetAxis("Vertical");
        float rotationInput = Input.GetAxis("Horizontal");

        float rayDistance = 2f;
        bool canMoveDirection = true;

        if (moveInput > 0f)
        {
            Ray ray = new Ray(transform.position, transform.forward);
            if (Physics.Raycast(ray, rayDistance, _landLayerMask))
                canMoveDirection = false;
        }
        else if (moveInput < 0f)
        {
            Ray ray = new Ray(transform.position, -transform.forward);
            if (Physics.Raycast(ray, rayDistance, _landLayerMask))
                canMoveDirection = false;
        }

        if (canMoveDirection && moveInput != 0f)
            transform.Translate(Vector3.forward * (moveInput * _moveSpeed * Time.deltaTime));

        transform.Rotate(Vector3.up, rotationInput * _turnSpeed * Time.deltaTime);
    }
}