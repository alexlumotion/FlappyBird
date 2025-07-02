using UnityEngine;
using System;

public enum GameStateMy
{
    Idle,
    Playing,
    Paused,
    GameOver
}

public class GameStateManager : MonoBehaviour
{
    // Singleton
    public static GameStateManager Instance { get; private set; }

    // Поточний стан гри
    private GameStateMy currentState = GameStateMy.Idle;

    // Івент: підписка на зміну стану гри
    public event Action<GameStateMy> OnGameStateChanged;

    public GameStateMy CurrentState => currentState;

    void Awake()
    {
        // Singleton інстанс
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    /// <summary>
    /// Встановлює новий стан гри
    /// </summary>
    public void SetState(GameStateMy newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        //Debug.Log($"🔁 GameState змінено на: {currentState}");

        // Виклик івенту
        OnGameStateChanged?.Invoke(currentState);
    }

    /// <summary>
    /// Утиліти — можна викликати з будь-якого місця
    /// </summary>
    ///  Idle, Playing, Paused, GameOver
    /// 
    public void StartGame() => SetState(GameStateMy.Playing);
    public void PauseGame() => SetState(GameStateMy.Paused);
    public void ResumeGame() => SetState(GameStateMy.Playing);
    public void RestartGame() => SetState(GameStateMy.GameOver);
    public void GameOver() => SetState(GameStateMy.GameOver);
    public void SetIdle() => SetState(GameStateMy.Idle);
}
