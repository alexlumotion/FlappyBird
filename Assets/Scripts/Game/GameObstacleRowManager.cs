using System.Collections.Generic;
using UnityEngine;

public class GameObstacleRowManager : MonoBehaviour
{
    public static GameObstacleRowManager Instance { get; private set; }

    [Header("Spawn Settings")]
    public GameObject[] obstaclePrefabs;
    public int poolSize = 50;
    public Transform obstacleParent;


    [Header("Spawn Points")]
    public Transform[] spawnPoints;
    public int spawnMin = 0;
    public int spawnMax = 4;

    [Header("Rotation Trigger")]
    public Transform watchedTransform;
    public float angleStep = 1.91f;
    public int angleMultiplier = 1;
    private float lastTriggerAngle = 0f;

    [Header("Return Settings")]
    public float zReturnThreshold = 0f;

    private Queue<GameObject> obstaclePool = new Queue<GameObject>();
    private List<GameObject> activeObstacles = new List<GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InitializePool();
    }

    void Update()
    {
        float currentAngle = watchedTransform.rotation.eulerAngles.x;
        float deltaAngle = Mathf.DeltaAngle(lastTriggerAngle, currentAngle);

        float triggerStep = angleStep * angleMultiplier;

        if (Mathf.Abs(deltaAngle) >= triggerStep)
        {
            lastTriggerAngle = currentAngle;
            SpawnObstacles();
        }
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject prefab = GetRandomPrefab();
            GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obstaclePool.Enqueue(obj);
        }
    }

    GameObject GetRandomPrefab()
    {
        if (obstaclePrefabs.Length == 0)
        {
            Debug.LogError("❌ Немає префабів у obstaclePrefabs!");
            return null;
        }

        int index = Random.Range(0, obstaclePrefabs.Length);
        return obstaclePrefabs[index];
    }

    public void SpawnObstacles()
    {
        int count = Random.Range(spawnMin, spawnMax + 1);
        List<int> indices = new List<int> { 0, 1, 2, 3, 4 };
        Shuffle(indices);

        for (int i = 0; i < count; i++)
        {
            int spawnIndex = indices[i];
            if (spawnIndex >= spawnPoints.Length) continue;

            Transform spawnPoint = spawnPoints[spawnIndex];
            GameObject obj = GetPooledObject();

            if (obj != null)
            {
                obj.transform.position = spawnPoint.position;
                obj.transform.rotation = Quaternion.identity;
                obj.transform.SetParent(obstacleParent);

                var behaviour = obj.GetComponent<GameObstacleBehaviour>();
                if (behaviour != null)
                {
                    behaviour.PlayAppearAnimation(); // ✅ Анімація появи
                    behaviour.Init(this, zReturnThreshold);
                }

                activeObstacles.Add(obj);
            }
        }
    }

    GameObject GetPooledObject()
    {
        if (obstaclePool.Count > 0)
        {
            return obstaclePool.Dequeue();
        }

        Debug.LogWarning("⚠️ Пул об’єктів вичерпано.");
        return null;
    }

    public void ReturnToPool(GameObject obj)
    {
        //var behaviour = obj.GetComponent<GameObstacleBehaviour>();
        //behaviour.PlayDisappearAnimation(() =>
        //{
            obj.transform.SetParent(obstacleParent);
            obstaclePool.Enqueue(obj);
            activeObstacles.Remove(obj);
        //});
    }

    //for Game Over
    public void ResetAllObstacles()
    {
        foreach (var obj in activeObstacles)
        {
            var behaviour = obj.GetComponent<GameObstacleBehaviour>();
            behaviour.PlayDisappearAnimation(() =>
            {
                obj.transform.SetParent(obstacleParent);
                obstaclePool.Enqueue(obj);
            });
        }

        activeObstacles.Clear();
        Debug.Log("🔁 Усі перешкоди очищено та повернуто до пулу.");
    }

    //for SpawnObstacles
    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}