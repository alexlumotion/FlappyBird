using UnityEngine;

public class GameLevelCamera : MonoBehaviour
{
    public Transform cameraTransform;

    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            cameraTransform.Translate(new Vector3(0, 0, Constants.CAMERA_GAME_LEVEL_SPEED * Time.deltaTime * speed));
        }
    }
}
