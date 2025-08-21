using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [Header("UI Панелі")]
    public GameObject idleUI;
    public GameObject gameplayUI;
    public GameObject pauseUI;
    public GameObject gameOverUI;

    private UIAnimations uiAnimations;

    void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        uiAnimations = UIAnimations.Instance;
        uiAnimations.ShowLetsStart();
    }

    private void HandleGameStateChanged(GameStateMy newState)
    {
        // Деактивуємо все
        //idleUI?.SetActive(false);
        //gameplayUI?.SetActive(false);
        //pauseUI?.SetActive(false);
        //gameOverUI?.SetActive(false);

        // Активуємо відповідне
        switch (newState)
        {
            case GameStateMy.Idle:
                //idleUI?.SetActive(true);
                uiAnimations.HideBest();
                uiAnimations.HideYour();
                uiAnimations.HideGameOver();
                uiAnimations.ShowLetsStart();
                break;
            case GameStateMy.Playing:
                //gameplayUI?.SetActive(true);
                uiAnimations.ShowBest();
                uiAnimations.ShowYour();
                uiAnimations.HideGameOver();
                uiAnimations.HideLetsStart();
                break;
            case GameStateMy.Paused:
                //pauseUI?.SetActive(true);
                //uiAnimations.ShowGameOver();
                break;
            case GameStateMy.GameOver:
                //gameplayUI?.SetActive(true);
                //gameOverUI?.SetActive(true);
                uiAnimations.ShowGameOver();
                break;
        }

        //Debug.Log($"🧩 UI оновлено для стану: {newState}");
    }
}
