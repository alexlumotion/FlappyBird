using UnityEngine;

public class GameLogicManager : MonoBehaviour
{

    void Start()
    {
        GameStateManager.Instance.SetIdle();
        GameScoreManager.Instance.ResetScore();
    }

    // 📦 Методи кнопок
    public void OnStartButtonPressed()
    {
        GameStateManager.Instance.StartGame();
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
        GameScoreManager.Instance.ResetScore();
    }
}
