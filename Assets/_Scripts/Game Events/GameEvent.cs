using System.Collections.Generic;
using PedronsaDev.Console;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Event/New Game Event")]
public class GameEvent : ScriptableObject
{
    private static HashSet<GameEvent> _listenedEvents = new HashSet<GameEvent>();

    private HashSet<GameEventListener> _gameEventListeners = new HashSet<GameEventListener>();

    public void Register(GameEventListener gameEventListener)
    {
        _gameEventListeners.Add(gameEventListener);
        _listenedEvents.Add(this);
    }

    public void Deregister(GameEventListener gameEventListener)
    {
        _gameEventListeners.Remove(gameEventListener);
        if (_gameEventListeners.Count == 0)
            _listenedEvents.Remove(this);
    }

    public void Invoke()
    {
        foreach (var gameEventListener in _gameEventListeners) 
            gameEventListener.RaiseEvent();
    }

    [Command("raise_event","Triggers a game event if found")]
    public static void RaiseEvent(string eventName)
    {
        bool eventFound = false;
        
        foreach (GameEvent gameEvent in _listenedEvents)
        {
            if (gameEvent.name == eventName)
            {
                gameEvent.Invoke();
                eventFound = true;
            } 
        }

        if (!eventFound) 
            Console.LogWarning($"Event '{eventName}' not found");
    }
}