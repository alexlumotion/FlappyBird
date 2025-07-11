using UnityEngine;
using DG.Tweening;
using System;

public enum ObstacleType
{
    Obstacle,
    Bonus
}

public class GameObstacleBehaviour : MonoBehaviour
{
    [Header("Type Settings")]
    public ObstacleType obstacleType = ObstacleType.Obstacle;

    [Header("Appear Settings")]
    public float appearDurationMin = 0.5f;
    public float appearDurationMax = 1.0f;
    public Ease appearEase = Ease.OutBack;

    [Header("DisAppear Settings")]
    public float disappearDurationMin = 0.25f;
    public float disappearDurationMax = 0.45f;
    public Ease disappearEase = Ease.InBack;

    [Header("Idle Settings")]
    public float idleScaleMin = 0.65f;
    public float idleScaleMax = 1.05f;
    public Ease idleEase = Ease.InOutSine;
    public LoopType idleLoop = LoopType.Yoyo;

    [Header("Return to pool")]
    private GameObstacleRowManager poolManager;
    public float returnThresholdZ = 0f;
    private bool canReturnToPool = false;

    private bool isAnimating = false;
    private Tween currentTween;

    [Header("Dev tools")]
    public bool playAppear = false;
    public bool playDisappear = false;
    public bool playIdle = false;
    public bool playWobble = false;
    public bool stopAnimation = false;

    void Update()
    {
        // 📉 Коли обʼєкт перейшов поріг по Z — запускаємо анімацію зникнення
        if (canReturnToPool && transform.position.z >= returnThresholdZ)
        {
            canReturnToPool = false; // ⛔ щоб більше не викликалося повторно
            PlayDisappearAnimation(() =>
            {
                poolManager.ReturnToPool(gameObject);
            });
        }
        if (stopAnimation)
        {
            stopAnimation = false;
            StopAnimations();
        }
        // Дебагові тригери через інспектор
        if (playAppear)
        {
            playAppear = false;
            PlayAppearAnimation();
        }

        if (playDisappear)
        {
            playDisappear = false;
            PlayDisappearAnimation();
        }

        if (playIdle)
        {
            playIdle = false;
            PlayIdleAnimation();
        }

        if (playWobble)
        {
            playWobble = false;
            PlayIdleWobble();
        }
    }

    void Start()
    {
        poolManager = GameObstacleRowManager.Instance;
    }

    public void Init(GameObstacleRowManager manager, float zThreshold)
    {
        poolManager = GameObstacleRowManager.Instance;
        returnThresholdZ = zThreshold;
        canReturnToPool = true;
    }

    public void StopAnimations()
    {
        transform.DOKill();
        currentTween = null;
        isAnimating = false;
    }

    // 1️⃣ Зникнення (scale 1 → 0) з колбеком
    public void PlayDisappearAnimation(Action onComplete = null)
    {
        if (isAnimating) return;
        isAnimating = true;

        StopAnimations();

        Vector3 currentScale = transform.localScale;

        float randomDuration = UnityEngine.Random.Range(disappearDurationMin, disappearDurationMax);

        currentTween = transform.DOScaleY(0f, randomDuration)
            .SetEase(disappearEase)
            .OnComplete(() =>
            {
                isAnimating = false;
                onComplete?.Invoke();
            });
    }

    // 2️⃣ Поява (scale 0 → 1)
    public void PlayAppearAnimation(Action onComplete = null)
    {
        if (isAnimating) return;
        isAnimating = true;

        StopAnimations();

        Vector3 initialScale = Vector3.one;
        transform.localScale = new Vector3(initialScale.x, 0f, initialScale.z);

        float randomDuration = UnityEngine.Random.Range(appearDurationMin, appearDurationMax);

        currentTween = transform.DOScaleY(initialScale.y, randomDuration)
            .SetEase(appearEase)
            .OnComplete(() =>
            {
                isAnimating = false;
                onComplete?.Invoke();
            });
    }

    // ♾️ Idle "дихання"
    public void PlayIdleAnimation()
    {
        StopAnimations();

        currentTween = transform
            .DOScale(idleScaleMax, idleScaleMin)
            .SetEase(idleEase)
            .SetLoops(-1, idleLoop);
    }

    // ♾️ Idle з покачуванням
    public void PlayIdleWobble()
    {
        StopAnimations();

        Sequence idleSequence = DOTween.Sequence();

        idleSequence
            .Append(transform.DOScale(Vector3.one * 1.05f, 0.6f).SetEase(Ease.InOutSine))
            .Join(transform.DORotate(new Vector3(0, 0, 5f), 0.6f).SetEase(Ease.InOutSine))
            .Append(transform.DOScale(Vector3.one, 0.6f).SetEase(Ease.InOutSine))
            .Join(transform.DORotate(Vector3.zero, 0.6f).SetEase(Ease.InOutSine))
            .SetLoops(-1);

        currentTween = idleSequence;
    }
}