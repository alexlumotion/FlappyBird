using UnityEngine;
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

    [Header("Return to pool")]
    private GameObstacleRowManager poolManager;
    public float returnThresholdZ = 0f;
    private bool canReturnToPool = false;

    [Header("Back to pools")]
    public Vector3 resetScale = new Vector3(1, 0, 1);

    public GameObstacleAnimations animations;

    void Update()
    {
        // 📉 Коли обʼєкт перейшов поріг по Z — запускаємо анімацію зникнення
        if (canReturnToPool && transform.position.z >= returnThresholdZ)
        {
            canReturnToPool = false; // ⛔ щоб більше не викликалося повторно
            transform.localScale = resetScale;
            poolManager.ReturnToPool(gameObject);
        }
    }

    void Start()
    {
        poolManager = GameObstacleRowManager.Instance;
        animations = GetComponent<GameObstacleAnimations>();
    }

    public void Init()
    {
        canReturnToPool = true;
    }

    public void StopAnimations()
    {
        animations.StopAnimations();
    }

    // 1️⃣ Зникнення (scale 1 → 0) з колбеком
    public void PlayDisappearAnimation(Action onComplete = null)
    {
        animations.PlayDisappearAnimation(onComplete);
    }

    // 2️⃣ Поява (scale 0 → 1)
    public void PlayAppearAnimation(Action onComplete = null)
    {
        animations.PlayAppearAnimation(onComplete);
    }

    // ♾️ Idle "дихання"
    public void PlayIdleAnimation()
    {
        animations.PlayIdleAnimation();
    }
    
}