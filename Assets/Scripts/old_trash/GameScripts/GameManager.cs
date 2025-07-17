using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool startGame = false;
    public bool startPlaying = false;
    public bool die = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            DestroyImmediate(this);
        }
    }

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (startGame)
        {
            StartGame();
            startGame = false;
        }

        if (startPlaying)
        {
            StartPlaying();
            startPlaying = false;
        }

        if (die)
        {
            Die();
            die = false;
        }
    }

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    protected GameManager()
    {
        GameState = GameState.Start;
    }

    public GameState GameState { get; set; }

    public void StartGame()
    {
        // Transitions to start state
        UIManager.Instance.SetStatus(Constants.STATUS_START);
        UIManager.Instance.ResetScore();
        this.GameState = GameState.Start;
    }

    public void StartPlaying()
    {
        UIManager.Instance.ResetScore();
        UIManager.Instance.SetStatus(Constants.STATUS_PLAYING);
        this.GameState = GameState.Playing;
    }

    public void Die()
    {
        UIManager.Instance.SetStatus(Constants.STATUS_DEAD);
        this.GameState = GameState.Dead; 
    }
}
