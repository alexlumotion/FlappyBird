using UnityEngine;
using System.Collections.Generic;

//public enum ObstacleType { Obstacle, Bonus }

public class GameObstacleRowManager : MonoBehaviour
{
    public static GameObstacleRowManager Instance { get; private set; }

    [Header("Spawn Settings")]
    public GameObject[] obstaclePrefabs; // 9 штук
    public GameObject[] bonusPrefabs;    // 3 штуки
    public Transform[] spawnPoints;      // 3 точки
    public Transform obstacleParent;

    [Header("Spawn Trigger")]
    public Transform watchedTransform;
    public float angleStep = 1.91f;
    public int angleMultiplier = 1;

    private float lastTriggerAngle = 0f;

    private Queue<GameObject> obstaclePool = new Queue<GameObject>();
    private Queue<GameObject> bonusPool = new Queue<GameObject>();
    private List<GameObject> activeObstacles = new List<GameObject>();

    private enum SpawnPattern
    {
        OneObstacle,
        OneBonus,
        TwoObstacles,
        ObstacleAndBonus
    }


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
        InitializePools();
    }

    void Update()
    {
        float currentAngle = watchedTransform.rotation.eulerAngles.x;
        float deltaAngle = Mathf.DeltaAngle(lastTriggerAngle, currentAngle);

        float triggerStep = angleStep * angleMultiplier;

        if (Mathf.Abs(deltaAngle) >= triggerStep)
        {
            lastTriggerAngle = currentAngle;
            TrySpawnScenario();
        }
    }

    void InitializePools()
    {
        foreach (var prefab in obstaclePrefabs)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            obstaclePool.Enqueue(obj);
        }

        foreach (var prefab in bonusPrefabs)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            bonusPool.Enqueue(obj);
        }
    }

    public void TrySpawnScenario()
    {
        List<SpawnPattern> patterns = new List<SpawnPattern> { 
            SpawnPattern.OneObstacle, 
            SpawnPattern.OneBonus, 
            SpawnPattern.TwoObstacles, 
            SpawnPattern.ObstacleAndBonus 
        };

        while (patterns.Count > 0)
        {
            SpawnPattern selected = patterns[Random.Range(0, patterns.Count)];

            if (CanSpawn(selected))
            {
                ExecuteSpawn(selected);
                return;
            }

            patterns.Remove(selected);
        }

        Debug.LogWarning("❌ Немає достатньо обʼєктів у пулах для жодного варіанту спавну.");
    }

    bool CanSpawn(SpawnPattern pattern)
    {
        switch (pattern)
        {
            case SpawnPattern.OneObstacle: return obstaclePool.Count >= 1;
            case SpawnPattern.OneBonus: return bonusPool.Count >= 1;
            case SpawnPattern.TwoObstacles: return obstaclePool.Count >= 2;
            case SpawnPattern.ObstacleAndBonus: return obstaclePool.Count >= 1 && bonusPool.Count >= 1;
            default: return false;
        }
    }

    void ExecuteSpawn(SpawnPattern pattern)
    {
        List<int> availableIndexes = new List<int> { 0, 1, 2 };
        Shuffle(availableIndexes);

        switch (pattern)
        {
            case SpawnPattern.OneObstacle:
                SpawnFromPool(obstaclePool, availableIndexes[0]);
                break;
            case SpawnPattern.OneBonus:
                SpawnFromPool(bonusPool, availableIndexes[0]);
                break;
            case SpawnPattern.TwoObstacles:
                SpawnFromPool(obstaclePool, availableIndexes[0]);
                SpawnFromPool(obstaclePool, availableIndexes[1]);
                break;
            case SpawnPattern.ObstacleAndBonus:
                SpawnFromPool(obstaclePool, availableIndexes[0]);
                SpawnFromPool(bonusPool, availableIndexes[1]);
                break;
        }
    }

    void SpawnFromPool(Queue<GameObject> pool, int spawnPointIndex)
    {
        if (pool.Count == 0 || spawnPointIndex >= spawnPoints.Length) return;

        GameObject obj = pool.Dequeue();
        obj.transform.position = spawnPoints[spawnPointIndex].position;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.SetParent(obstacleParent);
        obj.SetActive(true);

        var behaviour = obj.GetComponent<GameObstacleBehaviour>();
        if (behaviour != null)
        {
            behaviour.Init();
            behaviour.PlayAppearAnimation();
        }

        activeObstacles.Add(obj);
    }

    public void ReturnToPool(GameObject obj)
    {
        var behaviour = obj.GetComponent<GameObstacleBehaviour>();
        if (behaviour != null)
        {
            behaviour.StopAnimations();
        }

        obj.SetActive(false);
        obj.transform.SetParent(obstacleParent);

        // Визначаємо тип і повертаємо у відповідний пул
        var type = obj.GetComponent<GameObstacleBehaviour>()?.obstacleType ?? ObstacleType.Obstacle;
        if (type == ObstacleType.Bonus)
            bonusPool.Enqueue(obj);
        else
            obstaclePool.Enqueue(obj);

        activeObstacles.Remove(obj);
    }

    public void ResetAllObstacles()
    {
        foreach (var obj in new List<GameObject>(activeObstacles))
        {
            ReturnToPool(obj);
        }

        activeObstacles.Clear();
        //Debug.Log("🔁 Усі обʼєкти повернуто в пул.");
    }

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