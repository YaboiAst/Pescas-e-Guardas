using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BouyancyObject : MonoBehaviour
{
    public Transform[] floaters;
    
    public float underWaterDrag = 3f;

    public float undwatAngDrag = 1f;

    public float airDrag = 0f;

    public float airangDrag = 0.05f;
    
    public float floatingPower = 15f;

    public float waterheight = 0f;
    
    Rigidbody m_rigidbody;

    int floatersUnderWater;

    bool underwater;
    
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        floatersUnderWater = 0;
        for (int i = 0; i < floaters.Length; i++)
        {
            float difference = floaters[i].position.y - waterheight;
            if (difference < 0)
            {
                m_rigidbody.AddForceAtPosition(Vector3.up * floatingPower * Mathf.Abs(difference), floaters[i].position, ForceMode.Force);
                floatersUnderWater += 1;
                
                if (!underwater)
                {
                    underwater = true;
                    SwitchState(true);
                }
            }
        }
        
        if (underwater && floatersUnderWater == 0)
        {
            underwater = false;
            SwitchState(false);
        }
    }

    void SwitchState(bool isunderwater)
    {
        if (isunderwater)
        {
            m_rigidbody.drag = underWaterDrag;
            m_rigidbody.angularDrag = undwatAngDrag;
        }

        else if (underwater)
        {
            m_rigidbody.drag = airDrag;
            m_rigidbody.angularDrag = airangDrag;
        }
    }
}
