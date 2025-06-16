using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            transform.Translate(new Vector3(0, 0, Constants.CAMERA_GAME_LEVEL_SPEED * Time.deltaTime));
        }
    }
}
