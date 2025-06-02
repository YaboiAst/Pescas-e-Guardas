using UnityEngine;

public class Bussola : MonoBehaviour
{
    [SerializeField] private RectTransform _compassDial;
    [SerializeField] private Transform _playerCamera;

    [Header("Wobble Settings")]
    [SerializeField] private float _wobbleSpeed = 4f;
    [SerializeField] private float _wobbleAmount = 5f;

    private float _currentRotation;
    private float _velocity;

    void Update()
    {
        float targetRotation = _playerCamera.eulerAngles.y;

        if (targetRotation > 180f) targetRotation -= 360f;

        _currentRotation = Mathf.SmoothDampAngle(_currentRotation, -targetRotation, ref _velocity, 1f / _wobbleSpeed);

        float wobble = Mathf.Sin(Time.time * _wobbleSpeed) * _wobbleAmount;

        _compassDial.localEulerAngles = new Vector3(0f, 0f, _currentRotation + wobble);

    }
}
