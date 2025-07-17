using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public Transform birdTransform;

    public float speed = 1f;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            birdTransform.Translate(new Vector3(0, 0, Constants.CAMERA_GAME_LEVEL_SPEED * Time.deltaTime) * speed);
        }
    }
}
