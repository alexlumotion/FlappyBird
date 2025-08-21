using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UIAnimations : MonoBehaviour
{

    public static UIAnimations Instance { get; private set; }


    [Header("UI Elements")]
    public RectTransform letsStart;
    public RectTransform gameOver;
    public RectTransform bestScore;
    public RectTransform yourScore;

    [Header("LetsStart Animation Settings")]
    public float minStartMoveDuration = 0.5f;
    public float maxStartMoveDuration = 1f;
    public Ease startMoveEase = Ease.OutBack;
    public float startUpPosition = -86f;
    public float startDownPosition = -860f;
    public bool showLetsStart = false;
    public bool hideLetsStart = false;

    [Header("Your Animation Settings")]
    public float minYourMoveDuration = 0.5f;
    public float maxYourMoveDuration = 1f;
    public Ease yourMoveEase = Ease.OutBack;
    public float yourUpPosition = 434f;
    public float yourDownPosition = 700f;
    public bool showYour = false;
    public bool hideYour = false;

    [Header("Best Animation Settings")]
    public float minBestMoveDuration = 0.5f;
    public float maxBestMoveDuration = 1f;
    public Ease bestMoveEase = Ease.OutBack;
    public float bestUpPosition = 434f;
    public float bestDownPosition = 700f;
    public bool showBest = false;
    public bool hideBest = false;

    [Header("GameOver Animation Settings")]
    public float minGameOverMoveDuration = 0.5f;
    public float maxGameOverMoveDuration = 1f;
    public Ease gameMoveEase = Ease.OutBack;
    public float gameUpPosition = 0f;
    public float gameDownPosition = -800f;
    public bool showGame = false;
    public bool hideGame = false;

    void Awake()
    {
        Instance = this;
    }

    public bool test1, test2 = false;
    void Update()
    {
        if (showLetsStart) { showLetsStart = false; ShowLetsStart(); }
        if (hideLetsStart) { hideLetsStart = false; HideLetsStart(); }
        if (showYour) { showYour = false; ShowYour(); }
        if (hideYour) { hideYour = false; HideYour(); }
        if (showBest) { showBest = false; ShowBest(); }
        if (hideBest) { hideBest = false; HideBest(); }
        if (showGame) { showGame = false; ShowGameOver(); }
        if (hideGame) { hideGame = false; HideGameOver(); }

        if (test1) { test1 = false; HideLetsStart(); ShowBest(); ShowYour(); }
        if (test2) { test2 = false; HideGameOver(); HideBest(); HideYour(); }
    }

    public void ShowLetsStart(Action onComplete = null)
    {
         if (letsStart.anchoredPosition.y == startUpPosition) return;

        // Рандомна тривалість анімації
        float duration = UnityEngine.Random.Range(minStartMoveDuration, maxStartMoveDuration);

        // Початкова позиція (внизу, поза екраном)
        letsStart.anchoredPosition = new Vector2(letsStart.anchoredPosition.x, startDownPosition);

        // Анімація вгору до видимої позиції
        letsStart.DOAnchorPosY(startUpPosition, duration)
            .SetEase(startMoveEase)
            .OnComplete(() => onComplete?.Invoke());
    }

    public void HideLetsStart(Action onComplete = null)
    {
        if (letsStart.anchoredPosition.y == startDownPosition) return;

        // Рандомна тривалість анімації
        float duration = UnityEngine.Random.Range(minStartMoveDuration, maxStartMoveDuration);

        // Початкова позиція (внизу, поза екраном)
        letsStart.anchoredPosition = new Vector2(letsStart.anchoredPosition.x, startUpPosition);

        // Анімація вгору до видимої позиції
        letsStart.DOAnchorPosY(startDownPosition, duration)
            .SetEase(startMoveEase)
            .OnComplete(() => onComplete?.Invoke());
    }

    public void ShowYour(Action onComplete = null)
    {
        if (yourScore.anchoredPosition.y == yourUpPosition) return;

        // Рандомна тривалість анімації
        float duration = UnityEngine.Random.Range(minYourMoveDuration, maxYourMoveDuration);

        // Початкова позиція (внизу, поза екраном)
        yourScore.anchoredPosition = new Vector2(yourScore.anchoredPosition.x, yourDownPosition);

        // Анімація вгору до видимої позиції
        yourScore.DOAnchorPosY(yourUpPosition, duration)
            .SetEase(yourMoveEase)
            .OnComplete(() => onComplete?.Invoke());
    }

    public void HideYour(Action onComplete = null)
    {
        if (yourScore.anchoredPosition.y == yourDownPosition) return;

        // Рандомна тривалість анімації
        float duration = UnityEngine.Random.Range(minYourMoveDuration, maxYourMoveDuration);

        // Початкова позиція (внизу, поза екраном)
        yourScore.anchoredPosition = new Vector2(yourScore.anchoredPosition.x, yourUpPosition);

        // Анімація вгору до видимої позиції
        yourScore.DOAnchorPosY(yourDownPosition, duration)
            .SetEase(yourMoveEase)
            .OnComplete(() => onComplete?.Invoke());
    }

    public void ShowBest(Action onComplete = null)
    {
        if (bestScore.anchoredPosition.y == bestUpPosition) return;

        // Рандомна тривалість анімації
        float duration = UnityEngine.Random.Range(minBestMoveDuration, maxBestMoveDuration);

        // Початкова позиція (внизу, поза екраном)
        bestScore.anchoredPosition = new Vector2(bestScore.anchoredPosition.x, bestDownPosition);

        // Анімація вгору до видимої позиції
        bestScore.DOAnchorPosY(bestUpPosition, duration)
            .SetEase(bestMoveEase)
            .OnComplete(() => onComplete?.Invoke());
    }

    public void HideBest(Action onComplete = null)
    {
        if (bestScore.anchoredPosition.y == bestDownPosition) return;

        // Рандомна тривалість анімації
        float duration = UnityEngine.Random.Range(minBestMoveDuration, maxBestMoveDuration);

        // Початкова позиція (внизу, поза екраном)
        bestScore.anchoredPosition = new Vector2(bestScore.anchoredPosition.x, bestUpPosition);

        // Анімація вгору до видимої позиції
        bestScore.DOAnchorPosY(bestDownPosition, duration)
            .SetEase(bestMoveEase)
            .OnComplete(() => onComplete?.Invoke());
    }

    public void ShowGameOver(Action onComplete = null)
    {
        if (gameOver.anchoredPosition.y == gameUpPosition) return;

        // Рандомна тривалість анімації
        float duration = UnityEngine.Random.Range(minGameOverMoveDuration, maxGameOverMoveDuration);

        // Початкова позиція (внизу, поза екраном)
        gameOver.anchoredPosition = new Vector2(gameOver.anchoredPosition.x, gameDownPosition);

        // Анімація вгору до видимої позиції
        gameOver.DOAnchorPosY(gameUpPosition, duration)
            .SetEase(gameMoveEase)
            .OnComplete(() => onComplete?.Invoke());
    }

    public void HideGameOver(Action onComplete = null)
    {
        if (gameOver.anchoredPosition.y == gameDownPosition) return;

        // Рандомна тривалість анімації
        float duration = UnityEngine.Random.Range(minGameOverMoveDuration, maxGameOverMoveDuration);

        // Початкова позиція (внизу, поза екраном)
        gameOver.anchoredPosition = new Vector2(gameOver.anchoredPosition.x, gameUpPosition);

        // Анімація вгору до видимої позиції
        gameOver.DOAnchorPosY(gameDownPosition, duration)
            .SetEase(gameMoveEase)
            .OnComplete(() => onComplete?.Invoke());
    }

}