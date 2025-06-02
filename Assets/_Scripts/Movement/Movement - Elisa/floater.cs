using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floater : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public int floaterCount = 1;
    

    private void FixedUpdate()
    {
        rigidBody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
        float WaveHeight = Wave.instance.GetWaveHeight(transform.position.x);
        
        if (transform.position.y < WaveHeight)
        {
            float displacementMultiplier = Mathf.Clamp01(WaveHeight - transform.position.y / depthBeforeSubmerged) * displacementAmount;
            rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position,ForceMode.Acceleration);
            
        }
    }
}
