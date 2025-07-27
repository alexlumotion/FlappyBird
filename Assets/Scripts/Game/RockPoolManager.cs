using UnityEngine;
using System.Collections.Generic;

public class RockPoolManager : MonoBehaviour
{
    public static RockPoolManager Instance { get; private set; }

    [Header("Rock Settings")]
    public GameObject[] rockPrefabs;  // 6 унікальних префабів
    public int copiesPerPrefab = 4;   // 3 або 4 копії на кожен

    [Header("Spawn Range Settings")]
    public int minSpawnCount = 3;
    public int maxSpawnCount = 6;

    [Header("Position Settings")]
    public Transform spawnReference;              // обʼєкт для Y та Z
    public float xSpawnRange = 3f;                // -xSpawnRange до +xSpawnRange
    public float zSpawnRange = 1f;

    [Header("Parent Reference")]
    public Transform rockParent;

    [Header("Recycle Settings")]
    public float recycleZThreshold = 0.5f;

    private Queue<GameObject> rockPool = new Queue<GameObject>();
    private List<GameObject> activeRocks = new List<GameObject>();
    private Dictionary<GameObject, Renderer> renderers = new Dictionary<GameObject, Renderer>();

    public bool spawn = false;

    [Header("Spawn Trigger")]
    public Transform watchedTransform;
    public float angleStep = 1.91f;
    public float angleMultiplier = 1;
    private float lastTriggerAngle = 0f;

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
        for (int i = activeRocks.Count - 1; i >= 0; i--)
        {
            GameObject rock = activeRocks[i];
            if (rock.transform.position.z >= recycleZThreshold)
            {
                ReturnToPool(rock);
            }
        }

        float currentAngle = watchedTransform.rotation.eulerAngles.x;
        float deltaAngle = Mathf.DeltaAngle(lastTriggerAngle, currentAngle);
        float triggerStep = angleStep * angleMultiplier;

        if (Mathf.Abs(deltaAngle) >= triggerStep)
        {
            lastTriggerAngle = currentAngle;
            SpawnRocks();
        }

        if (spawn)
        {
            spawn = false;
            SpawnRocks();
        }
    }

    void InitializePool()
    {
        foreach (var prefab in rockPrefabs)
        {
            for (int i = 0; i < copiesPerPrefab; i++)
            {
                GameObject rock = Instantiate(prefab);

                // Кешуємо MeshRenderer
                Renderer rend = rock.GetComponentInChildren<Renderer>();
                if (rend != null)
                {
                    rend.enabled = false;
                    renderers[rock] = rend;
                }

                // Random scale
                float scale = Random.Range(0.25f, 0.35f);
                rock.transform.localScale = new Vector3(scale, scale, scale);

                // Random rotation
                rock.transform.rotation = Random.rotation;

                rockPool.Enqueue(rock);
            }
        }
    }

    public void SpawnRocks()
    {
        int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            if (rockPool.Count == 0) return;

            GameObject rock = rockPool.Dequeue();

            Vector3 spawnPos = new Vector3(
                Random.Range(-xSpawnRange, xSpawnRange),
                spawnReference.position.y,
                Random.Range(spawnReference.position.z - zSpawnRange, spawnReference.position.z + zSpawnRange)
            );

            rock.transform.SetParent(rockParent);
            rock.transform.position = spawnPos;

            if (renderers.TryGetValue(rock, out Renderer rend))
            {
                rend.enabled = true;
            }

            activeRocks.Add(rock);
        }
    }

    void ReturnToPool(GameObject rock)
    {
        if (renderers.TryGetValue(rock, out Renderer rend))
        {
            rend.enabled = false;
        }

        rock.transform.SetParent(transform); // Назад до менеджера
        activeRocks.Remove(rock);
        rockPool.Enqueue(rock);
    }

    public void ResetAllRocks()
    {
        foreach (var rock in new List<GameObject>(activeRocks))
        {
            ReturnToPool(rock);
        }
        activeRocks.Clear();
    }
}