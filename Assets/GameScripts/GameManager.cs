using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class GameManager : MonoBehaviour
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

    void Start()
    {
        StartGame();
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
