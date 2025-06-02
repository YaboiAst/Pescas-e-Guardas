using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Interactable : MonoBehaviour
{
    public static IReadOnlyCollection<Interactable> InteractablesInRange => s_interactablesInRange;
    private static HashSet<Interactable> s_interactablesInRange = new HashSet<Interactable>();

    [SerializeField] private float _timeToInteract = 0f;
    [SerializeField] private float _interactionRange;
    [Tooltip("Use 0 pra usar infinitamente")] 
    [Min(0)] 
    [SerializeField] private int _maxInteractions = 1;

    [SerializeField] private UnityEvent _onInteractionCompleted;
    [SerializeField] private UnityEvent _onLastInteractionCompleted;
    public Action OnPlayerEnterTrigger;
    public Action OnPlayerExitTrigger;

    // [SerializeField] private bool _requireMinigame;
    // [SerializeField] private MinigameSettings _minigameSettings;
    [SerializeField] protected InteractionType _interactionType;

    private float _timeInteracted = 0f;
    protected int _interactionCount = 0;

    public float InteractionProgress => _timeInteracted / _timeToInteract;
    public bool WasFullyInteracted => InteractionProgress >= 1;

    private IMet[] _allConditions;

    public virtual InteractionType InteractionType => _interactionType;

    public event Action InteractionCompleted;
    public static event Action<bool> InteractablesInRangeChanged;
    public static event Action<Interactable, string> AnyInteractionComplete;

    public string ConditionMessage { get; private set; }

    [Button("[DEBUG] Interact")]
    public void DebugInteract()
    {
        _interactionCount = _maxInteractions;
        InteractionCompleted?.Invoke();
        SendInteractionComplete();
    }

    private void Awake()
    {
        GetComponent<SphereCollider>().radius = _interactionRange;
        _allConditions = GetComponents<IMet>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !WasFullyInteracted)
        {
            s_interactablesInRange.Add(this);
            InteractablesInRangeChanged?.Invoke(true);
            OnPlayerEnterTrigger?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (s_interactablesInRange.Remove(this))
                InteractablesInRangeChanged?.Invoke(s_interactablesInRange.Any());
            
            OnPlayerExitTrigger?.Invoke();
        }
    }

    public void Interact()
    {
        if (WasFullyInteracted)
            return;

        _timeInteracted += Time.deltaTime;

        if (WasFullyInteracted)
        {
            // if (_requireMinigame)
            // {
            //     s_interactablesInRange.Remove(this);
            //     InteractablesInRangeChanged?.Invoke(s_interactablesInRange.Any());
            //     MinigameManager.s_Instance.StartMinigame(_minigameSettings, HandleMinigameCompleted);
            // }
            // else
            CompleteIteraction();
        }
    }
    
    private void CompleteIteraction()
    {
        _interactionCount++;

        if (_maxInteractions == 0)
        {
            _interactionCount = 0;
            _timeInteracted = 0;
        }
        else
        {
            if (_interactionCount < _maxInteractions)
                _timeInteracted = 0f;
            else
                _onLastInteractionCompleted.Invoke();
        }

        InteractionCompleted?.Invoke();

        if (WasFullyInteracted)
            s_interactablesInRange.Remove(this);

        SendInteractionComplete();
    }

    // private void HandleMinigameCompleted(MinigameResult result)
    // {
    //     if (result == MinigameResult.Won)
    //     {
    //         CompleteIteraction();
    //     }
    //     else if (result == MinigameResult.Fail)
    //     {
    //         _timeInteracted = 0f;
    //         s_interactablesInRange.Add(this);
    //         InteractablesInRangeChanged?.Invoke(s_interactablesInRange.Any());
    //     }
    // }

    public bool CheckConditions()
    {
        if (_allConditions == null)
            return false;

        foreach (var condition in _allConditions)
        {
            if (condition.Met() == false)
            {
                ConditionMessage = condition.NotMetMessage;
                return false;
            }

            ConditionMessage = condition.MetMessage;
        }

        return true;
    }

    protected void SendInteractionComplete()
    {
        InteractablesInRangeChanged?.Invoke(s_interactablesInRange.Any());
        _onInteractionCompleted?.Invoke();
        AnyInteractionComplete?.Invoke(this, _interactionType.CompletedInteraction);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _interactionRange);

        Gizmos.color = CheckConditions() ? Color.green : Color.yellow;

        if (WasFullyInteracted)
            Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position + new Vector3(0, 2f, 0), 5f);
    }
}