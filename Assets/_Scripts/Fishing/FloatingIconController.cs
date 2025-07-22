using System;
using DG.Tweening;
using UnityEngine;

public class FloatingIconController : MonoBehaviour
{
    [Header("Distance Settings")]
    public float _activationDistance = 15f;

    [SerializeField] private GameObject _iconInstance;
    private Transform _playerTransform;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
        GameObject player = GameObject.FindWithTag("Player");

        if (!player)
        {
            Debug.LogError("Player object not found! Make sure your player has the 'Player' tag.");
            return;
        }
        _playerTransform = player.transform;

        Canvas canvas = FindObjectOfType<Canvas>();
        if (!canvas)
        {
            Debug.LogError("No Canvas found in the scene. Please add a UI Canvas.");
            return;
        }

        _iconInstance.SetActive(false);
    }

    void Update()
    {
        if (!_playerTransform || !_iconInstance) return;

        float distance = Vector3.Distance(transform.position, _playerTransform.position);

        if (distance <= _activationDistance)
        {
            if (!_iconInstance.activeInHierarchy)
            {
                _iconInstance.SetActive(true);
                _iconInstance.transform.DOScale(new Vector3(0.05f, 0.05f, 0.05f), 0.3f).SetEase(Ease.OutQuad);
            }
        }
        else
        {
            if (_iconInstance.activeInHierarchy)
            {
                _iconInstance.transform.DOScale(new Vector3(0f, 0f, 0f), 0.3f).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    _iconInstance.SetActive(false);
                });
            }
        }
    }

    void OnDestroy()
    {
        if (_iconInstance)
            Destroy(_iconInstance);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _activationDistance);
    }
  #endif

}
