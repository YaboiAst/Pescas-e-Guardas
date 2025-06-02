using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public Transform centerPoint;

    public float forwardForce = 1000f;
    public float rotationSpeed = 90f;

    private void Start()
    {
        rb.drag = 5f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        bool w = Input.GetKey("w");
        bool a = Input.GetKey("a");
        bool d = Input.GetKey("d"); 


        if (w)
        {
            rb.AddForce(transform.forward * forwardForce * Time.fixedDeltaTime);
        }

        if (d)
        {
            transform.RotateAround(centerPoint.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }

        if (a)
        {
            transform.RotateAround(centerPoint.position, Vector3.up, -rotationSpeed * Time.deltaTime);
        }
    }
}
