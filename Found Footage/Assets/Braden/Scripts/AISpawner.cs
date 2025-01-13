using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform player;

    [Header("Input")]
    public GameObject enemyPrefab;
    public string spawnType = "Farthest";
    public List<Transform> spawns = new();

    protected void Start()
    {
        if (!player)
            player = GameObject.Find("Player").transform;

        if (spawns.Count == 0) // add spawns from transform children
        {
            foreach(Transform spawn in transform.GetComponentsInChildren<Transform>())
                spawns.Add(spawn);
        }
    }

    // Main

    public GameObject SpawnEnemy()
    {
        Transform spawn = PickSpawn();
        GameObject newEnemy = Instantiate(enemyPrefab, spawn.position, spawn.rotation);

        return newEnemy;
    }
    protected Transform PickSpawn()
    {
        // depend on spawnType

        return FarthestFromPlayer();
    }

    // Spawn Types

    Transform FarthestFromPlayer()
    {
        Transform retreatSpot = transform;
        float farthestDistance = -1;

        foreach (Transform spot in spawns)
        {
            float distance = Vector3.Distance(spot.position, player.position);

            if (farthestDistance == -1 || distance > farthestDistance)
            {
                retreatSpot = spot;
                farthestDistance = distance;
            }
        }

        return retreatSpot;
    }
}
