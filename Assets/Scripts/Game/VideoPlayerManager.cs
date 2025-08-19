using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    public static VideoPlayerManager Instance { get; private set; }

    [Header("Video Settings")]
    public VideoPlayer videoPlayerForeground;
    public VideoPlayer videoPlayerBackground;
    public bool playOnStart = false;

    [Header("Speed Settings")]
    [Range(0.1f, 5f)]
    public float initialSpeed = 1.0f;
    public float speedStep = 0.1f;

    public bool speedUp, speedDown, test = false;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //videoPlayer = GetComponent<VideoPlayer>();

        //videoPlayerBackground.sort

        SetSpeed(initialSpeed);

        if (playOnStart && videoPlayerForeground != null && videoPlayerBackground != null)
        {
            PlayVideo();
        }
    }

    // void Update()
    // {
    //     // Для тесту — зміна швидкості з клавіатури
    //     if (Input.GetKeyDown(KeyCode.UpArrow) || speedUp)
    //     {
    //         speedUp = false;
    //         SetSpeed(videoPlayer.playbackSpeed + speedStep);
    //     }

    //     if (Input.GetKeyDown(KeyCode.DownArrow) || speedDown)
    //     {
    //         speedDown = false;
    //         SetSpeed(videoPlayer.playbackSpeed - speedStep);
    //     }
    //     if (test)
    //     {
    //         test = false;
    //         videoPlayer.playbackSpeed = initialSpeed;
    //     }
    // }

    /// <summary>
    /// ▶️ Запускає відтворення відео
    /// </summary>
    public void PlayVideo()
    {
        if (videoPlayerForeground != null && !videoPlayerForeground.isPlaying)
        {
            videoPlayerForeground.Play();
        }
        if (videoPlayerBackground != null && !videoPlayerBackground.isPlaying)
        {
            videoPlayerBackground.Play();
        }
    }

    /// <summary>
    /// ⏸️ Ставить відео на паузу
    /// </summary>
    public void PauseVideo()
    {
        if (videoPlayerForeground != null && videoPlayerForeground.isPlaying)
        {
            videoPlayerForeground.Pause();
        }
        if (videoPlayerBackground != null && videoPlayerBackground.isPlaying)
        {
            videoPlayerBackground.Pause();
        }
    }

    /// <summary>
    /// 🔁 Скидає відео до початку і запускає
    /// </summary>
    public void ResetVideo()
    {
        if (videoPlayerForeground != null)
        {
            videoPlayerForeground.Stop();            // Зупиняємо
            videoPlayerForeground.time = 0f;         // Скидаємо час
            videoPlayerForeground.Play();            // Запускаємо заново
        }
        if (videoPlayerBackground != null)
        {
            videoPlayerBackground.Stop();            // Зупиняємо
            videoPlayerBackground.time = 0f;         // Скидаємо час
            videoPlayerBackground.Play();            // Запускаємо заново
        }
    }
    public void SetSpeed(float speed)
    {
        speed = Mathf.Clamp(speed, 0.1f, 5f);
        videoPlayerForeground.playbackSpeed = speed;
        videoPlayerBackground.playbackSpeed = speed;
        //Debug.Log($"🎬 Встановлено швидкість відео: {speed}");
    }
}
