using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

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

    //singleton implementation
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = new UIManager();
            
            return instance;
        }
    }

    protected UIManager()
    {
    }

    private float score = 0;
    private float bestScore = 0;


    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
    
    public void SetScore(float value)
    {
        score = value;
        UpdateScoreText();
    }

    public void IncreaseScore(float value)
    {
        score += value;
        if (score > bestScore) 
        {
            bestScore = score;
        }
        UpdateScoreText();
    }
    
    private void UpdateScoreText()
    {
        // ScoreText.text = "Score: " + score.ToString();
        ScoreText.text = score.ToString();
        BestScoreText.text = bestScore.ToString();
    }

    public void SetStatus(string text)
    {
        StatusText.text = text;
    }

    public Text ScoreText, StatusText, BestScoreText;

}