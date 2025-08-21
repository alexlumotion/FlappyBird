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
        RoadAnimationManager.Instance.PlayAnimation();
        VideoPlayerManager.Instance.ResetVideo();
    }

    // 📦 Методи кнопок
    public void OnStartButtonPressed()
    {
        GameStateManager.Instance.StartGame();
        GameObstacleRowManager.Instance.TrySpawnScenario(); // Перший спавн
        RoadAnimationManager.Instance.PlayAnimation();
        VideoPlayerManager.Instance.ResetVideo();
        RockPoolManager.Instance.SpawnRocks();
        GameScoreManager.Instance.ResetScore();
    }

    public void OnGameOverButtonPressed()
    {
        GameStateManager.Instance.GameOver();
        GameScoreManager.Instance.GameOver();
        RoadAnimationManager.Instance.StopAnimation();
        GameObstacleRowManager.Instance.ResetAllObstacles();
        RockPoolManager.Instance.ResetAllRocks();
        AutoStarterIdleState.Instance.StartTimer();

        VideoPlayerManager.Instance.PauseVideo();
        RoadAnimationManager.Instance.PauseAnimation();
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
        VideoPlayerManager.Instance.PlayVideo();
        RoadAnimationManager.Instance.ResumeAnimation();
    }
    
}
