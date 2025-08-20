using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPoseController : MonoBehaviour
{
    private GameStateManager gameStateManager;
    // Start is called before the first frame update
    void Start()
    {
        gameStateManager = GameStateManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTPoseActivated()
    {
        if (gameStateManager.CurrentState != GameStateMy.Idle) return;

        GameLogicManager.Instance.OnStartButtonPressed();

    }

}
