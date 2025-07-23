using DG.Tweening;
using UnityEngine;

public class GameSpeedController : MonoBehaviour
{

    RoadAnimationManager roadAnimationManager;
    VideoPlayerManager videoPlayerManager;

    public float currentSpeed = 1f; // кешована швидкість


    // Start is called before the first frame update
    void Start()
    {
        GameScoreManager.Instance.OnComboMultiplierChanged += UpdateMultiplier;
        roadAnimationManager = RoadAnimationManager.Instance;
        videoPlayerManager = VideoPlayerManager.Instance;
    }

    public void UpdateMultiplier(int multiplier)
    {
        float newSpeed = Mathf.Clamp(1f + (multiplier - 1) * 0.1f, 0.1f, 5f);

        if (Mathf.Abs(currentSpeed - newSpeed) > 0.3f)
        {
            // Tween зміни швидкості відео
            DOTween.To(() => currentSpeed, x =>
            {
                currentSpeed = x;
                videoPlayerManager.SetSpeed(x);
                roadAnimationManager.SetSpeed(x);
            }, newSpeed, 0.5f); // тривалість Tween — 0.5 сек
        }
        else
        {
            currentSpeed = newSpeed;
            videoPlayerManager.SetSpeed(currentSpeed);
            roadAnimationManager.SetSpeed(currentSpeed);
        }

        //Debug.Log($"🎞️ Швидкість оновлена до: {newSpeed}");
    }

}
