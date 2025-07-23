using UnityEngine;

public class RoadAnimationManager : MonoBehaviour
{
    public static RoadAnimationManager Instance { get; private set; }

    [Header("Animator")]
    public Animator animator;

    [Header("Speed Settings")]
    [Range(0.1f, 5f)]
    public float initialSpeed = 1.0f;
    public float speedStep = 0.1f;

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

    void Start()
    {
        SetSpeed(initialSpeed);
    }

    // void Update()
    // {
    //     // Для тесту: керування з клавіатури
    //     if (Input.GetKeyDown(KeyCode.UpArrow))
    //     {
    //         SetSpeed(animator.speed + speedStep);
    //     }

    //     if (Input.GetKeyDown(KeyCode.DownArrow))
    //     {
    //         SetSpeed(animator.speed - speedStep);
    //     }
    // }

    public void SetSpeed(float speed)
    {
        speed = Mathf.Clamp(speed, 0.1f, 5f);
        animator.speed = speed;
    }

    public void PlayAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Play ON");
        }
    }

    public void StopAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Play OFF");
        }
    }
}
