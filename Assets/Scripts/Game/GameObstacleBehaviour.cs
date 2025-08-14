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
    public bool canReturnToPool = false;

    [Header("Back to pools")]
    public Vector3 resetScale = new Vector3(1, 0, 1);

    public GameObstacleAnimations animations;
    public BubbleEmitterController bubbleEmitterController;

    public float testZ = 0f;

    void Update()
    {
        testZ = transform.position.z;
        // 📉 Коли обʼєкт перейшов поріг по Z — запускаємо анімацію зникнення
        if (canReturnToPool && transform.position.z >= returnThresholdZ)
        {
            canReturnToPool = false; // ⛔ щоб більше не викликалося повторно
                                     //transform.localScale = resetScale;
                                     //poolManager.ReturnToPool(gameObject);
                                     //PlayDisappearAnimation();
            if (obstacleType == ObstacleType.Obstacle)
                {
                    bubbleEmitterController.StopEmitting();
                }
            PlayDisappearAnimation(() =>
                {
                    poolManager.ReturnToPool(gameObject);
                });
        }
    }

    void Start()
    {
        poolManager = GameObstacleRowManager.Instance;
        animations = GetComponent<GameObstacleAnimations>();
        if (obstacleType == ObstacleType.Obstacle)
        {
            bubbleEmitterController = GetComponent<BubbleEmitterController>();
        }
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
        animations.PlayDisappearAnimation(() =>
            {
                poolManager.ReturnToPool(gameObject);
                if (obstacleType == ObstacleType.Obstacle)
                {
                    bubbleEmitterController.StopEmitting();
                }
            });
    }

    // 2️⃣ Поява (scale 0 → 1)
    public void PlayAppearAnimation(Action onComplete = null)
    {
        animations.PlayAppearAnimation(() =>
            {
                if (GameStateManager.Instance.CurrentState == GameStateMy.GameOver)
                {
                    PlayDisappearAnimation();
                }
                else
                {
                    PlayIdleAnimation();
                    if (obstacleType == ObstacleType.Obstacle)
                    {
                        bubbleEmitterController.StartEmitting();
                    }
                }
                
            });
    }

    // ♾️ Idle "дихання"
    public void PlayIdleAnimation()
    {
        animations.PlayIdleAnimation();
    }
    
}