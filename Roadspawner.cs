using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [Header("Lane Setup")]
    public float laneWidth = 3f;
    public float spawnZ = 45f;
    public float destroyZ = -12f;

    [Header("Prefabs")]
    public List<GameObject> obstaclePrefabs = new();
    public GameObject coinPrefab;
    public GameObject factPowerUpPrefab;

    [Header("Spawn Rhythm")]
    public float spawnEvery = 0.65f;
    public float coinSpacing = 2.8f;
    public int minCoinsInLine = 3;
    public int maxCoinsInLine = 5;
    [Range(0f, 1f)] public float factChance = 0.18f;

    float spawnTimer;

    void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsRunning) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnPattern();
            float speedFactor = Mathf.InverseLerp(GameManager.Instance.baseSpeed, GameManager.Instance.maxSpeed, GameManager.Instance.CurrentSpeed);
            spawnTimer = Mathf.Lerp(spawnEvery, spawnEvery * 0.62f, speedFactor);
        }
    }

    void SpawnPattern()
    {
        List<int> lanes = new() { -1, 0, 1 };
        Shuffle(lanes);

        int blockedCount = Random.value < 0.35f ? 2 : 1;
        for (int i = 0; i < blockedCount; i++)
            SpawnObstacle(lanes[i], spawnZ + i * 2f);

        int coinLane = lanes[blockedCount < lanes.Count ? blockedCount : Random.Range(0, lanes.Count)];
        int coinCount = Random.Range(minCoinsInLine, maxCoinsInLine + 1);
        for (int i = 0; i < coinCount; i++)
            Spawn(coinPrefab, coinLane, spawnZ + 5f + i * coinSpacing);

        if (factPowerUpPrefab && Random.value < factChance)
        {
            int factLane = lanes[Random.Range(0, lanes.Count)];
            Spawn(factPowerUpPrefab, factLane, spawnZ + 10f);
        }
    }

    void SpawnObstacle(int lane, float z)
    {
        if (obstaclePrefabs.Count == 0) return;
        Spawn(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)], lane, z);
    }

    void Spawn(GameObject prefab, int lane, float z)
    {
        if (!prefab) return;
        Vector3 position = new(lane * laneWidth, prefab.transform.position.y, z);
        GameObject instance = Instantiate(prefab, position, prefab.transform.rotation);

        if (!instance.TryGetComponent(out RunnerObject runnerObject))
            runnerObject = instance.AddComponent<RunnerObject>();

        runnerObject.destroyZ = destroyZ;
    }

    static void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
