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
        GameObstacleRowManager.Instance.TrySpawnScenario(); // Перший спавн
        RoadAnimationManager.Instance.PlayAnimation();
        VideoPlayerManager.Instance.ResetVideo();
    }

    public void OnPauseButtonPressed()
    {
        GameStateManager.Instance.PauseGame();
        VideoPlayerManager.Instance.PauseVideo();
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
        GameScoreManager.Instance.GameOver();
        RoadAnimationManager.Instance.StopAnimation();
        GameObstacleRowManager.Instance.ResetAllObstacles();
    }
}
