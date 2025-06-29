using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    public static readonly string MAP_KEY = "Open Map";
    public static readonly string BAG_KEY = "Open Bag";
    public static readonly string QUEST_KEY = "Open Quest";
    private readonly string[] _keyPool =
    {
        MAP_KEY,
        BAG_KEY,
        QUEST_KEY,
    };

    [SerializeField] private float debounceTime;
    private float _debounceTimer = -1f;
    
    // TODO - Chamar overlays aqui ou descentralizar
    // [SerializeField] private GameObject mapRoot;
    // [SerializeField] private GameObject bagRoot;
    // [SerializeField] private GameObject questRoot;
    // public Action<string> OnKeyPressed;
    
    private void Update()
    {
        if (_debounceTimer > 0f)
        {
            _debounceTimer -= Time.deltaTime;
            return;
        }
        
        for (var i = 0; i < _keyPool.Length; i++)
        {
            if (Input.GetButtonUp(_keyPool[i]))
            {
                Debug.Log($"Calling: {_keyPool[i]}");
                _debounceTimer = debounceTime;
                return;
            }
        }
    }
}
