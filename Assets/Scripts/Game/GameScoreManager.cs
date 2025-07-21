using UnityEngine;

public class GameScoreManager : MonoBehaviour
{
    public static GameScoreManager Instance { get; private set; }

    private int score = 0;
    public int CurrentScore => score;

    public delegate void ScoreChanged(int newScore);
    public event ScoreChanged OnScoreChanged;
    public delegate void FinalScoreChanged(int newScore);
    public event FinalScoreChanged OnFinalScoreChanged;
    public delegate void MaxScoreLoaded(int newScore);
    public event MaxScoreLoaded OnMaxScoreLoaded;

    public delegate void ComboMultiplierChanged(int newMultiplier);
    public event ComboMultiplierChanged OnComboMultiplierChanged;

    private ScoreSourceType lastSourceType = ScoreSourceType.type1;
    private int consecutiveSameTypeCount = 0;

    [Tooltip("Скільки однакових підряд потрібно для старту комбо")]
    public int comboThreshold = 3;

    private int currentComboMultiplier = 1;
    public int CurrentComboMultiplier => currentComboMultiplier;

    public int maxPlayerScore = 0;

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
        LoadMaxScore();
    }

    void LoadMaxScore()
    {
        maxPlayerScore = PlayerPrefs.GetInt("player_score", 15);
        OnMaxScoreLoaded?.Invoke(maxPlayerScore);
    }

    void SaveMaxScore()
    {
        if (score > maxPlayerScore)
        {
            PlayerPrefs.SetInt("player_score", score);
            PlayerPrefs.Save(); // не обов’язково, але краще явно викликати
        }
    }

    public void AddScore(int baseAmount, ScoreSourceType sourceType)
    {
        if (sourceType == lastSourceType)
        {
            consecutiveSameTypeCount++;
        }
        else
        {
            lastSourceType = sourceType;
            consecutiveSameTypeCount = 1;
        }

        // 🔢 Динамічний множник: 3 → x2, 6 → x3, 9 → x4, ...
        int calculatedMultiplier = (consecutiveSameTypeCount >= comboThreshold)
            ? 1 + (consecutiveSameTypeCount - comboThreshold + 1) / comboThreshold
            : 1;

        if (calculatedMultiplier != currentComboMultiplier)
        {
            currentComboMultiplier = calculatedMultiplier;
            OnComboMultiplierChanged?.Invoke(currentComboMultiplier);
        }

        int finalScore = baseAmount * currentComboMultiplier;
        score += finalScore;

        //Debug.Log($"🟡 +{finalScore} очок (множник x{currentComboMultiplier}) → загалом: {score}");

        OnScoreChanged?.Invoke(score);
    }

    public void ResetScore()
    {
        score = 0;
        consecutiveSameTypeCount = 0;
        lastSourceType = ScoreSourceType.type1;

        currentComboMultiplier = 1;
        OnComboMultiplierChanged?.Invoke(currentComboMultiplier);

        //Debug.Log("🔁 Очки скинуто");
        OnScoreChanged?.Invoke(score);
    }

    public void ShowFinalScore()
    {
        OnFinalScoreChanged?.Invoke(score);
    }

    public int GetScore()
    {
        return score;
    }

    public void GameOver()
    {
        SaveMaxScore();
        LoadMaxScore();
        ShowFinalScore();
        ResetScore();
    }

}
