using UnityEngine;

public class GameLogicManager : MonoBehaviour
{
    private int score = 0;

    void Start()
    {
        GameStateManager.Instance.SetIdle();
        ResetScore();
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
        ResetScore();
    }

    public void OnIdleButtonPressed()
    {
        GameStateManager.Instance.SetIdle();
    }

    public void OnGameOverButtonPressed()
    {
        GameStateManager.Instance.GameOver(); 
    }


    // 🎯 Робота з очками
    public void AddScore(int value)
    {
        score += value;
        Debug.Log("🟡 Очки: " + score);
    }

    public void ResetScore()
    {
        score = 0;
        Debug.Log("🔁 Очки скинуто");
    }

    public int GetScore()
    {
        return score;
    }
}
