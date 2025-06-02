using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMotor : MonoBehaviour
{
    public float turnSpeed = 1000f;
    public float accellerateSpeed = 1010f;
    public float drag;
    
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        handleDrag();
        
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        rb.AddTorque(0f, h * (turnSpeed * Time.deltaTime), 0f);
        rb.AddForce(transform.right * v * (accellerateSpeed * Time.deltaTime));
    }

    void handleDrag()
    {
        Vector3 hDrag = new Vector3(rb.velocity.x, 0f, rb.velocity.z) / ((1 + drag)/100);
    }
}
