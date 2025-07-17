using UnityEngine;

public class RoadAnimationManager : MonoBehaviour
{
    public static RoadAnimationManager Instance { get; private set; }

    [Header("Animator")]
    public Animator animator;

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
