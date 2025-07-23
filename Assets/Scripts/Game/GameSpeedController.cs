using UnityEngine;

public class GameSpeedController : MonoBehaviour
{

    RoadAnimationManager roadAnimationManager;
    VideoPlayerManager videoPlayerManager;

    // Start is called before the first frame update
    void Start()
    {
        GameScoreManager.Instance.OnComboMultiplierChanged += UpdateMultiplier;
        roadAnimationManager = RoadAnimationManager.Instance;
        videoPlayerManager = VideoPlayerManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateMultiplier(int multiplier)
    {
        float speed = 1.0f + (multiplier - 1) * 0.1f;
        roadAnimationManager.SetSpeed(speed);
        videoPlayerManager.SetSpeed(speed);
    }

}
