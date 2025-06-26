using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour {

    AudioSource _as;
    public AudioClip soundEffect;

    void Start()
    {
        _as = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == Constants.PLAYER_TAG && GameManager.Instance.GameState == GameState.Playing)
        {
            _as.PlayOneShot(soundEffect);
            GameManager.Instance.Die();
        }
    }
}