using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playericon : MonoBehaviour
{
    public Transform target;
    private void LateUpdate()
    {
        transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        transform.rotation = Quaternion.Euler(90f, target.eulerAngles.y, 0);
    }
}
