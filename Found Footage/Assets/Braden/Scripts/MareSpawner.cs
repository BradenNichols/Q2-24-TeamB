using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SpawnTime {
    public float viewCount;
    public bool hasSpawned;
}

public class MareSpawner : AISpawner
{
    [Header("Mare Spawn Data")]
    ViewCount viewCount;
    public List<Transform> mareRetreatSpots = new();
    public List<SpawnTime> spawnTimings = new();

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

            if (viewCount.viewers >= spawnData.viewCount)
            {
                spawnData.hasSpawned = true;
                SpawnEnemy();
            }
        }
    }

    public new GameObject SpawnEnemy()
    {
        GameObject newEnemy = base.SpawnEnemy();
        newEnemy.GetComponent<TheMare>().retreatSpots = mareRetreatSpots;

        return newEnemy;
    }
}
