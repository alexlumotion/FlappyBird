using UnityEngine;

public class GameLogicManager : MonoBehaviour
{

    public static GameLogicManager Instance { get; private set; }

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

    void Start()
    {
        GameStateManager.Instance.SetIdle();
        GameScoreManager.Instance.ResetScore();
    }

    // 📦 Методи кнопок
    public void OnStartButtonPressed()
    {
        GameStateManager.Instance.StartGame();
        GameObstacleRowManager.Instance.SpawnObstacles(); // Перший спавн
        GameObstacleRowManager.Instance.PlayAnimation();
    }

    public void OnPauseButtonPressed()
    {
        GameStateManager.Instance.PauseGame();
    }

    public void OnRestartButtonPressed()
    {
        GameStateManager.Instance.RestartGame();
        GameScoreManager.Instance.ResetScore();
    }

    public void OnIdleButtonPressed()
    {
        GameStateManager.Instance.SetIdle();
    }

    public void OnGameOverButtonPressed()
    {
        GameStateManager.Instance.GameOver();
        GameScoreManager.Instance.ShowFinalScore();
        GameScoreManager.Instance.ResetScore();
        GameObstacleRowManager.Instance.StopAnimation();
        GameObstacleRowManager.Instance.ResetAllObstacles();
    }
}
