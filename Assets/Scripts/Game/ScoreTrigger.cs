using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    [Tooltip("Скільки очок додати при активації")]
    public int scoreAmount = 1;

    [Tooltip("Тип джерела очок")]
    public ScoreSourceType sourceType = ScoreSourceType.type1;

    private GameScoreManager gameScoreManager;
    public bool isTrigger = false;

    void Start()
    {
        gameScoreManager = GameScoreManager.Instance;
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
        }
        else
        {
            Debug.LogWarning("⚠️ GameScoreManager не знайдено!");
        }
    }
}
