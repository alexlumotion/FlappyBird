using UnityEngine;
using System.Collections;

public class ObjectGenerator : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public GameObject[] Collectibles;
    public GameObject[] Obstacles;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
        {
            float placeObject = Random.Range(0.0f, 1.0f);
            if (placeObject < Constants.OBSTACLE_PERCENTAGE) //20% chance of obstacle only
            {
                CreateObject(SpawnPoints[i].position, Obstacles[Random.Range(0, Obstacles.Length)]);
            }
            else if (placeObject < (Constants.OBSTACLE_PERCENTAGE + Constants.COLLECTIBLE_PERCENTAGE)) //50% chance of collectible only
            {
                CreateObject(SpawnPoints[i].position, Collectibles[Random.Range(0, Collectibles.Length)]);
            }
            else //30% chance of obstacle and collectible
            {
                Vector3 obstaclePosition = CreateObject(SpawnPoints[i].position, Obstacles[Random.Range(0, Obstacles.Length)]);
                CreateObject(obstaclePosition, Collectibles[Random.Range(0, Collectibles.Length)]);
            }
        }
    }

    Vector3 CreateObject(Vector3 position, GameObject prefab)
    {
        if (prefab.name.Contains("Sphere")) {
            position += new Vector3(0, Random.Range(Constants.COLLECTIBLE_MIN_Y, Constants.COLLECTIBLE_MAX_Y), 0);
        } 
        else if (prefab.name.Contains("GreenPipe"))
        {
            position += new Vector3(0, Random.Range(Constants.COLLECTIBLE_MIN_Y, Constants.COLLECTIBLE_MAX_Y), 0);
        }
        Instantiate(prefab, position, Quaternion.identity);
        return position;
    }
}