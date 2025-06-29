using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterInteraction : MonoBehaviour
{
    [SerializeField] RenderTexture renderTexture;
    [SerializeField] Transform interactor;
    [SerializeField] Material _waterMaterial;

    void Awake()
    {
        _waterMaterial.SetTexture("_GlobalEffectRT", renderTexture);
        _waterMaterial.SetFloat("_ProjectionSize", GetComponent<Camera>().orthographicSize);
        //Shader.SetGlobalTexture("_GlobalEffectRT", renderTexture);
        //Shader.SetGlobalFloat("_ProjectionSize", GetComponent<Camera>().orthographicSize);
    }

    private void Update()
    {
        transform.position = new Vector3(interactor.transform.position.x, transform.position.y, interactor.transform.position.z);
        //Shader.SetGlobalVector("_Position", transform.position);
        _waterMaterial.SetVector("_Position", transform.position);
    }

}
