using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject mySphere; // The enemy prefab to spawn
    public int maxEnemies = 10; // Maximum number of enemies allowed
    private List<GameObject> activeEnemies = new List<GameObject>(); // List to track active enemies

    void Start()
    {
        // Start spawning enemies
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Check if we can spawn more enemies
            if (activeEnemies.Count < maxEnemies)
            {
                SpawnSphere();
            }
            yield return new WaitForSeconds(2f); // Wait time between spawns
        }
    }

    void SpawnSphere()
    {
        // Define spawn position (you can customize this)
        int spawnPointX = Random.Range(-10, 10);
        int spawnPointY = Random.Range(10, 20);
        int spawnPointZ = Random.Range(-10, 10);

        Vector3 spawnPosition = new Vector3(spawnPointX, spawnPointY, spawnPointZ);

        GameObject newSphere = Instantiate(mySphere, spawnPosition, Quaternion.identity);
        activeEnemies.Add(newSphere);

        
        newSphere.GetComponent<LadyInRed>().onDeath += HandleEnemyDeath;
    }

    void HandleEnemyDeath(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        Destroy(enemy); // Remove the enemy from the scene
        StartCoroutine(RespawnEnemy());
    }

    IEnumerator RespawnEnemy()
    {
        yield return new WaitForSeconds(2f); // Delay before respawning
        SpawnSphere(); // Spawn a new enemy
    }
}
