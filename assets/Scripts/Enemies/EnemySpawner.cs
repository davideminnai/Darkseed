using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform[] spawnPoints;
    
    [Header("Enemy Prefabs")]
    public GameObject[] enemyPrefabs; // Array of different enemy types
    
    [Header("Spawn Settings")]
    public int maxEnemies = 10;
    public float spawnInterval = 3f;
    public bool spawnOnStart = true;
    
    [Header("Wave System (Optional)")]
    public bool useWaves = false;
    public int totalWaves = 5;
    public float waveDelay = 5f;
    private int currentWave = 1;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private float nextSpawnTime = 0f;
    private bool isWaveActive = false;

    private void Start()
    {
        if (spawnOnStart)
        {
            SpawnInitialEnemies();
        }
        
        if (useWaves)
        {
            StartCoroutine(WaveSystem());
        }
    }

    private void Update()
    {
        if (!useWaves && Time.time >= nextSpawnTime && activeEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
        
        UpdateActiveEnemies();
    }

    public void SpawnInitialEnemies()
    {
        int initialCount = Mathf.Min(maxEnemies / 2, enemyPrefabs.Length);
        
        for (int i = 0; i < initialCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return;
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        // Find a valid spawn point away from player
        Transform spawnPoint = GetRandomSpawnPoint();
        if (spawnPoint == null) return;

        // Choose random enemy type
        int prefabIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyPrefab = enemyPrefabs[prefabIndex];

        // Instantiate enemy
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        
        if (enemy != null)
        {
            activeEnemies.Add(enemy);
            
            // Setup enemy reference to spawner
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                // Find player and assign to enemy
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    enemyController.playerTransform = player.transform;
                }
            }
        }
    }

    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Length == 0) return null;

        // Try to find a spawn point away from player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPos = player != null ? player.transform.position : Vector2.zero;

        for (int attempts = 0; attempts < 10; attempts++)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            
            if (point == null) continue;
            
            // Check distance from player (minimum 5 units)
            float distance = Vector2.Distance(point.position, playerPos);
            if (distance > 5f)
            {
                return point;
            }
        }

        // If no good spawn point found, return any valid one
        foreach (Transform point in spawnPoints)
        {
            if (point != null) return point;
        }

        return null;
    }

    private void UpdateActiveEnemies()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            GameObject enemy = activeEnemies[i];
            
            if (enemy == null || !enemy.activeInHierarchy)
            {
                activeEnemies.RemoveAt(i);
            }
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }

    private System.Collections.IEnumerator WaveSystem()
    {
        while (currentWave <= totalWaves)
        {
            Debug.Log($"Wave {currentWave} starting!");
            
            int enemiesToSpawn = currentWave * 2; // More enemies each wave
            
            for (int i = 0; i < enemiesToSpawn && activeEnemies.Count < maxEnemies; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval * 0.5f);
            }

            // Wait for all enemies to be defeated or time limit
            float waveTimeLimit = 30f;
            float elapsed = 0f;
            
            while (activeEnemies.Count > 0 && elapsed < waveTimeLimit)
            {
                yield return new WaitForSeconds(1f);
                elapsed += 1f;
            }

            currentWave++;
            
            if (currentWave <= totalWaves)
            {
                Debug.Log($"Wave complete! Starting next wave in {waveDelay} seconds...");
                yield return new WaitForSeconds(waveDelay);
            }
        }

        Debug.Log("All waves complete! Boss should spawn now.");
        // TODO: Spawn boss after all waves
    }

    public int GetActiveEnemyCount()
    {
        return activeEnemies.Count;
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnPoints != null)
        {
            Gizmos.color = Color.green;
            foreach (Transform point in spawnPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawWireSphere(point.position, 0.5f);
                    
                    // Draw line to show spawn radius
                    Gizmos.color = new Color(0, 1, 0, 0.3f);
                    Gizmos.DrawWireSphere(point.position, 5f);
                }
            }
        }
    }
}