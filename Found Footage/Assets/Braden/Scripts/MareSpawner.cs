using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SpawnTime {
    public float atViewCount;
    public bool isFinalSpawn;
    public bool hasSpawned;
}

public class MareSpawner : AISpawner
{
    [Header("Mare Spawning")]
    public List<Transform> retreatSpots = new();
    public List<SpawnTime> spawnTimings = new();

    ViewCount viewCount;

    new void Start()
    {
        base.Start();
        viewCount = player.GetComponent<ViewCount>();
    }

    // Main

    void FixedUpdate() // dont need it to run that often
    {
        for (int i = 0; i < spawnTimings.Count; i++)
        {
            SpawnTime spawnData = spawnTimings[i];
            if (spawnData.hasSpawned) continue;

            if (viewCount.viewers >= spawnData.atViewCount)
            {
                spawnData.hasSpawned = true;
                SpawnEnemy(spawnData.isFinalSpawn);
            }
        }
    }

    public GameObject SpawnEnemy(bool isFinalSpawn)
    {
        GameObject newEnemy = base.SpawnEnemy();

        TheMare mareClass = newEnemy.GetComponent<TheMare>();
        mareClass.retreatSpots = retreatSpots;
        mareClass.isFinalSpawn = isFinalSpawn;
        mareClass.target = player.transform;

        return newEnemy;
    }
}
