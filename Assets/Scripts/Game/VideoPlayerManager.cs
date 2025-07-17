using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerManager : MonoBehaviour
{
    public static VideoPlayerManager Instance { get; private set; }

    [Header("Video Settings")]
    public VideoPlayer videoPlayer;
    public bool playOnStart = false;

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
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (playOnStart && videoPlayer != null)
        {
            PlayVideo();
        }
    }

    /// <summary>
    /// ▶️ Запускає відтворення відео
    /// </summary>
    public void PlayVideo()
    {
        if (videoPlayer != null && !videoPlayer.isPlaying)
        {
            videoPlayer.Play();
        }
    }

    /// <summary>
    /// ⏸️ Ставить відео на паузу
    /// </summary>
    public void PauseVideo()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
    }

    /// <summary>
    /// 🔁 Скидає відео до початку і запускає
    /// </summary>
    public void ResetVideo()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();            // Зупиняємо
            videoPlayer.time = 0f;         // Скидаємо час
            videoPlayer.Play();            // Запускаємо заново
        }
    }
}
