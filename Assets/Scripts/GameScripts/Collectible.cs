using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
    AudioSource _as;
    public AudioClip soundEffect;
    public int ScorePoints = 1;

    void Start()
    {
        _as = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == Constants.PLAYER_TAG && GameManager.Instance.GameState == GameState.Playing)
        {
            _as.PlayOneShot(soundEffect);
            UIManager.Instance.IncreaseScore(ScorePoints);
            //Destroy(this.gameObject);
            transform.position = new Vector3(0, -100, 0);
        }
    }
}