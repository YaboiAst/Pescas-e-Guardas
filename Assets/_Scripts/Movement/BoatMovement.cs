using UnityEngine;

public class BoatMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 50f;

    private bool _canMove = true;

    private void OnEnable()
    {
        Minigame.OnMinigameUpdated += MinigameUpdated;
    }

    private void OnDestroy()
    {
        Minigame.OnMinigameUpdated -= MinigameUpdated;
    }


    private void MinigameUpdated(bool hasStarted)
    {
        if(hasStarted)
        {
            _canMove = false;
        }else
        {
            _canMove = true;
        }
    }
    
    private void Update()
    {
        if(!_canMove)
            return;

        float moveInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * (moveInput * _moveSpeed * Time.deltaTime));

        float rotationInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, rotationInput * _rotationSpeed * Time.deltaTime);
    }
}