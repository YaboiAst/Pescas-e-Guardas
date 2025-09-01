using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour

{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float deceleration = 3f;

    private float currentSpeed = 0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.drag = 1f;
            rb.angularDrag = 1f;
        }
    }

    void Update()
    {
        HandleMovementInput();
    }

    void HandleMovementInput()
    {
        // Rotação (A/D)
        float rotationInput = Input.GetAxis("Horizontal");
        transform.Rotate(0f, rotationInput * rotationSpeed * Time.deltaTime, 0f);

        // Movimento para frente/trás (W/S)
        float moveInput = Input.GetAxis("Vertical");

        if (moveInput != 0f)
        {
            // Aceleração
            currentSpeed += moveInput * acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed/2, maxSpeed);
        }
        else
        {
            // Desaceleração gradual
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }

        // Aplica o movimento
        Vector3 moveDirection = transform.forward * currentSpeed;
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
    }
}
