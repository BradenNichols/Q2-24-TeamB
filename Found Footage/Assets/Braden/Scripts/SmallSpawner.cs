using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSpawner : AISpawner
{
    [Header("Small Spawning")]
    public int maxEnemies = 5;
    public float randomMinSpawnTime = 4;
    public float randomMaxSpawnTime = 9;
    public List<Transform> patrolPoints = new();

    float currentTimeAmount = 0;
    float timeToSpawn = 0;

    [Header("Internal")]
    [SerializeField] int currentlySpawnedAmount = 0;

    new void Start()
    {
        base.Start();
        timeToSpawn = Random.Range(randomMinSpawnTime, randomMaxSpawnTime);
    }

    void FixedUpdate() // fixed update because we dont need to run it that much
    {
        if (currentlySpawnedAmount >= maxEnemies) return;

        currentTimeAmount += Time.fixedDeltaTime;

        if (currentTimeAmount >= timeToSpawn)
        {
            // spawn
            currentTimeAmount = 0;
            timeToSpawn = Random.Range(randomMinSpawnTime, randomMaxSpawnTime);

            SpawnEnemy();
        }
    }

    // Inherited

    public new GameObject SpawnEnemy()
    {
        GameObject newEnemy = base.SpawnEnemy();
        GeneralStats enemyStats = newEnemy.GetComponent<GeneralStats>();
        SmallChild enemyClass = newEnemy.GetComponent<SmallChild>();

        enemyClass.patrolPoints = patrolPoints;

        currentlySpawnedAmount++;
        enemyStats.deathEvent.AddListener(OnDespawn);

        return newEnemy;
    }

    void OnDespawn()
    {
        currentlySpawnedAmount--;
    }
}
