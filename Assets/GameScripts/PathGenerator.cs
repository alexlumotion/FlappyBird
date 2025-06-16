using UnityEngine;
using System.Collections;

public class PathGenerator : MonoBehaviour {

    public Transform[] PathSpawnPoints;
    public GameObject Path;

    void Start()
    {

    }

    void OnTriggerEnter(Collider hit)
    {
        //player has hit the collider
        if (hit.gameObject.tag == Constants.PLAYER_TAG)
        {
            Instantiate(Path, PathSpawnPoints[0].position, PathSpawnPoints[0].rotation);
        }
    }

}
