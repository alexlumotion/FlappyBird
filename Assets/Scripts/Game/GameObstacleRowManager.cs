using System.Collections.Generic;
using UnityEngine;

public class GameObstacleRowManager : MonoBehaviour
{
    [Header("Global Settings")]
    public GameObject[] obstaclePrefabs;
    public int poolSizePerRow = 21;
    public int rowsCount = 3;
    public float zSpacing = 10f;
    public float baseSpawnZ = 44f;
    public float despawnZ = -2f;
    public float moveSpeed = 10f;

    [Header("Row Settings")]
    public int minObstacleCount = 3;
    public int maxObstacleCount = 7;
    public int xMin = -4;
    public int xMax = 4;

    private class ObstacleRow
    {
        public float spawnZ;
        public Transform container;
        public List<GameObject> pool = new();
        public List<GameObject> active = new();
    }

    private List<ObstacleRow> rows = new();
    private bool isActive = false;

    void Start()
    {
        GameStateManager.Instance.OnGameStateChanged += HandleGameStateChanged;
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
                GenerateObstacleRow(row);
            }
        }
    }

    void InitializeRows()
    {
        rows.Clear();

        for (int i = 0; i < rowsCount; i++)
        {
            float rowZ = baseSpawnZ + i * zSpacing;

            GameObject rowGO = new GameObject($"Row_{i + 1}");
            rowGO.transform.parent = transform;
            rowGO.transform.position = new Vector3(0, 0, rowZ);

            var row = new ObstacleRow
            {
                spawnZ = rowZ,
                container = rowGO.transform
            };

            for (int j = 0; j < poolSizePerRow; j++)
            {
                int prefabIndex = j % obstaclePrefabs.Length;
                GameObject prefab = obstaclePrefabs[prefabIndex];
                GameObject obj = Instantiate(prefab, row.container);
                obj.SetActive(false);
                row.pool.Add(obj);
            }

            GenerateObstacleRow(row);
            rows.Add(row);
        }
    }

    void ClearRow(ObstacleRow row)
    {
        foreach (var obj in row.active)
        {
            obj.SetActive(false);
        }
        row.active.Clear();
    }

    void ClearAllRows()
    {
        foreach (var row in rows)
        {
            row.container.position = new Vector3(0, 0, row.spawnZ);
            ClearRow(row);
        }
    }

    void GenerateObstacleRow(ObstacleRow row)
    {
        List<int> availableX = new();
        for (int x = xMin; x <= xMax; x++)
            availableX.Add(x);

        int count = Random.Range(minObstacleCount, maxObstacleCount + 1);
        List<int> chosenX = new();

        while (chosenX.Count < count && availableX.Count > 0)
        {
            int index = Random.Range(0, availableX.Count);
            chosenX.Add(availableX[index]);
            availableX.RemoveAt(index);
        }

        if ((xMax - xMin + 1) - chosenX.Count < 1)
        {
            int removeIndex = Random.Range(0, chosenX.Count);
            chosenX.RemoveAt(removeIndex);
        }

        foreach (int x in chosenX)
        {
            GameObject obj = GetPooledObstacle(row);
            if (obj != null)
            {
                obj.transform.localPosition = new Vector3(x, 0.5f, 0);
                obj.SetActive(true);
                row.active.Add(obj);
            }
        }
    }

    GameObject GetPooledObstacle(ObstacleRow row)
    {
        foreach (var obj in row.pool)
        {
            if (!obj.activeInHierarchy)
                return obj;
        }

        Debug.LogWarning("⚠️ Pool закінчився — збільши poolSizePerRow.");
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
                isActive = false;     // зупиняємо рух, поки не стартануть
                ClearAllRows();       // очищаємо перед запуском
                foreach (var row in rows)
                    GenerateObstacleRow(row);       // повне очищення
                break;

            case GameStateMy.Idle:
                isActive = false;
                break;
            case GameStateMy.Paused:
                isActive = false;
                break;
        }
    }

    public HashSet<int> GetObstacleXCoordinatesAtZ(float z)
    {
        const float epsilon = 0.1f; // точність для порівняння координат Z
        foreach (var row in rows)
        {
            if (Mathf.Abs(row.container.position.z - z) < epsilon)
            {
                HashSet<int> xPositions = new();
                foreach (var obj in row.active)
                {
                    int x = Mathf.RoundToInt(obj.transform.localPosition.x);
                    xPositions.Add(x);
                }
                return xPositions;
            }
        }
        return new HashSet<int>();
    }


    void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }
    }
}
