using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public Text Title;
    public Text Status;
    public Text BestScore;
    public Text Tutorial;

    // Start is called before the first frame update
    void Start()
    {
        Title.text = Constants.TITLE_START;
        //Status.text = GetStatusText();
        // TODO: make best score work
        BestScore.text = GetBestScoreText();
        Tutorial.text = Constants.TUTORIAL;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GameManager.Instance.GameState);
        if (gameObject.activeInHierarchy)
        {
            BestScore.text = GetBestScoreText();
            //Status.text = GetStatusText();
            // Display information based on whether the game is in start or gameover (dead) state
            if (GameManager.Instance.GameState == GameState.Start)
            {
                Title.text = Constants.TITLE_START;
            }
            else if (GameManager.Instance.GameState == GameState.Dead)
            {
                Title.text = Constants.TITLE_GAMEOVER;
            }
        }
    }

    string GetStatusText()
    {
        if (KinectManager.Instance.CalibrationText != null && KinectManager.Instance.CalibrationText.text == Constants.KINECT_WAITING)
        {
            return Constants.KINECT_WAITING;
        } else
        {
            return (GameManager.Instance.GameState == GameState.Start) ? Constants.STATUS_START : Constants.STATUS_DEAD; 
        }
    }

    string GetBestScoreText()
    {
        return "Best Score: " + UIManager.Instance.BestScoreText.text;
    }
}
