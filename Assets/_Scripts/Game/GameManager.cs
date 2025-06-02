using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.Playing;

    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Paused) return;

        CurrentState = GameState.Paused;
        Time.timeScale = 0f;

        OnGameStateChanged?.Invoke(CurrentState);
    }

    public void ResumeGame()
    {
        if (CurrentState != GameState.Paused) return;

        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
        
        OnGameStateChanged?.Invoke(CurrentState);
    }

    public void TogglePause()
    {
        if (CurrentState == GameState.Paused)
            ResumeGame();
        else
            PauseGame();
    }
}

public enum GameState
{
    Playing,
    Paused,
}