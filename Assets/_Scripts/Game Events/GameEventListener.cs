using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private UnityEvent _unityEvent;
    [SerializeField] private GameEvent _gameEvent;

    private void Awake() => _gameEvent.Register(this);
    private void OnDisable() => _gameEvent.Deregister(this);

    public void RaiseEvent() => _unityEvent.Invoke();
}