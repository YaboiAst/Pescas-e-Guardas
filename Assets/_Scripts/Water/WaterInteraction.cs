using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterInteraction : MonoBehaviour
{
    [SerializeField] RenderTexture _renderTexture;
    [SerializeField] Transform _interactor;
    [SerializeField] Material _waterMaterial;

    void Awake()
    {
        _waterMaterial.SetTexture("_GlobalEffectRT", _renderTexture);
        _waterMaterial.SetFloat("_ProjectionSize", GetComponent<Camera>().orthographicSize);
    }

    private void Update()
    {
        transform.position = new Vector3(_interactor.transform.position.x, transform.position.y, _interactor.transform.position.z);
        _waterMaterial.SetVector("_Position", transform.position);
    }

}
