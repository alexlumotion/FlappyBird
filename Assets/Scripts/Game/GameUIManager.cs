using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [Header("UI Панелі")]
    public GameObject idleUI;
    public GameObject gameplayUI;
    public GameObject pauseUI;
    public GameObject gameOverUI;

    void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameStateMy newState)
    {
        // Деактивуємо все
        idleUI?.SetActive(false);
        gameplayUI?.SetActive(false);
        pauseUI?.SetActive(false);
        gameOverUI?.SetActive(false);

        // Активуємо відповідне
        switch (newState)
        {
            case GameStateMy.Idle:
                idleUI?.SetActive(true);
                break;
            case GameStateMy.Playing:
                gameplayUI?.SetActive(true);
                break;
            case GameStateMy.Paused:
                pauseUI?.SetActive(true);
                break;
            case GameStateMy.GameOver:
                gameplayUI?.SetActive(true);
                gameOverUI?.SetActive(true);
                break;
        }

        //Debug.Log($"🧩 UI оновлено для стану: {newState}");
    }
}
