using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class waterManager : MonoBehaviour
{
    public float WaveSpeed = 1f;
    public float WaveFrequency = 0.5f;
    public float WaveAmplitude = 1f;
    public GameObject water;
    
    Material waterMaterial;
    private Texture2D WavesDisplacement;

    private void Start()
    {
        SetVariables();
    }

    void SetVariables()
    {
        waterMaterial = water.GetComponent<Renderer>().sharedMaterial;
        WavesDisplacement = (Texture2D)waterMaterial.GetTexture("_WavesDisplacement");
    }

    public float WaveDepth(Vector3 position)
    {
        
    }
}
