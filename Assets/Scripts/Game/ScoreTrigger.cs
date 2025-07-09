using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    [Tooltip("Скільки очок додати при активації")]
    public int scoreAmount = 1;

    [Tooltip("Тип джерела очок")]
    public ScoreSourceType sourceType = ScoreSourceType.type1;

    private GameScoreManager gameScoreManager;
    public bool isTrigger = false;

    private GameObstacleBehaviour obstacleBehaviour;


    void Start()
    {
        gameScoreManager = GameScoreManager.Instance;
        obstacleBehaviour = GetComponent<GameObstacleBehaviour>();
    }

    void Update()
    {
        if (isTrigger)
        {
            isTrigger = false;
            TriggerScore();
        }
    }

    public void TriggerScore()
    {
        if (gameScoreManager != null)
        {
            gameScoreManager.AddScore(scoreAmount, sourceType);
            Debug.Log($"✅ +{scoreAmount} від {sourceType} ({gameObject.name})");
            obstacleBehaviour?.PlayDisappearAnimation();
        }
        else
        {
            Debug.LogWarning("⚠️ GameScoreManager не знайдено!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerScore();
        }
    }
}
