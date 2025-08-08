using DG.Tweening;
using UnityEngine;
using System;

public class GameObstacleAnimations : MonoBehaviour
{
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

    [Header("Dev tools")]
    public bool playAppear = false;
    public bool playDisappear = false;
    public bool playIdle = false;
    public bool stopAnimation = false;

    private bool isAnimating = false;
    private Tween currentTween;

    void Update()
    {
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

        float randomDuration = UnityEngine.Random.Range(appearDurationMin, appearDurationMax);

        currentTween = transform.DOScaleY(1f, randomDuration)
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

    
}
