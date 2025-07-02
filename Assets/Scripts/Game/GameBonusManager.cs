using System.Collections.Generic;
using UnityEngine;

public class GameBonusManager : MonoBehaviour
{
    [Header("Bonus Settings")]
    public GameObject[] bonusPrefabs;
    public int poolSizePerRow = 10;
    public int rowsCount = 3;
    public float zSpacing = 10f;
    public float baseSpawnZ = 44f;
    public float despawnZ = -2f;
    public float moveSpeed = 10f;

    [Range(0f, 1f)]
    public float spawnChance = 0.5f; // Ймовірність появи бонусу в рядку

    [Header("Position Range")] 
    public int xMin = -4;
    public int xMax = 4;

    private class BonusRow
    {
        public float spawnZ;
        public Transform container;
        public List<GameObject> pool = new();
        public List<GameObject> active = new();
    }

    private List<BonusRow> rows = new();
    private bool isActive = false;
    private GameObstacleRowManager obstacleManager;

    void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
        obstacleManager = FindObjectOfType<GameObstacleRowManager>();
        InitializeRows();
    }

    void Update()
    {
        if (!isActive) return;

        foreach (var row in rows)
        {
            row.container.position += Vector3.back * moveSpeed * Time.deltaTime;

            if (row.container.position.z <= despawnZ)
            {
                row.container.position = new Vector3(0, 0, row.spawnZ);
                ClearRow(row);
                GenerateBonusRow(row);
            }
        }
    }

    void InitializeRows()
    {
        rows.Clear();

        for (int i = 0; i < rowsCount; i++)
        {
            float rowZ = baseSpawnZ + i * zSpacing;

            GameObject rowGO = new GameObject($"BonusRow_{i + 1}");
            rowGO.transform.parent = transform;
            rowGO.transform.position = new Vector3(0, 0, rowZ);

            var row = new BonusRow
            {
                spawnZ = rowZ,
                container = rowGO.transform
            };

            for (int j = 0; j < poolSizePerRow; j++)
            {
                int prefabIndex = j % bonusPrefabs.Length;
                GameObject prefab = bonusPrefabs[prefabIndex];
                GameObject obj = Instantiate(prefab, row.container);
                obj.SetActive(false);
                row.pool.Add(obj);
            }

            GenerateBonusRow(row);
            rows.Add(row);
        }
    }

    void ClearRow(BonusRow row)
    {
        foreach (var obj in row.active)
        {
            obj.SetActive(false);
        }
        row.active.Clear();
    }

    void GenerateBonusRow(BonusRow row)
    {
        if (Random.value > spawnChance) return; // не спавнимо цього разу

        float currentZ = row.container.position.z;
        HashSet<int> obstacleX = GetObstacleXCoordinatesAtZ(currentZ);

        List<int> freePositions = new();
        for (int x = xMin; x <= xMax; x++)
        {
            if (obstacleX == null || !obstacleX.Contains(x))
                freePositions.Add(x);
        }

        if (freePositions.Count == 0) return;

        int spawnX = freePositions[Random.Range(0, freePositions.Count)];

        GameObject obj = GetPooledBonus(row);
        if (obj != null)
        {
            obj.transform.localPosition = new Vector3(spawnX, 0.5f, 0);
            obj.SetActive(true);
            row.active.Add(obj);
        }
    }

    GameObject GetPooledBonus(BonusRow row)
    {
        foreach (var obj in row.pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }
        return null;
    }

    void HandleGameStateChanged(GameStateMy newState)
    {
        switch (newState)
        {
            case GameStateMy.Playing:
                isActive = true;
                break;

            case GameStateMy.GameOver:
                isActive = false;
                ClearAllRows();
                foreach (var row in rows)
                    GenerateBonusRow(row);       // повне очищення
                break;

            case GameStateMy.Idle:
                isActive = false;
                break;
            case GameStateMy.Paused:
                isActive = false;
                break;
        }
    }

    void ClearAllRows()
    {
        foreach (var row in rows)
        {
            row.container.position = new Vector3(0, 0, row.spawnZ);
            ClearRow(row);
        }
    }

    public HashSet<int> GetObstacleXCoordinatesAtZ(float z)
    {
        if (obstacleManager != null)
            return obstacleManager.GetObstacleXCoordinatesAtZ(z);

        return new HashSet<int>();
    }

    void OnDestroy()
    {
        if (GameStateManager.Instance != null)
            GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
    }
}
