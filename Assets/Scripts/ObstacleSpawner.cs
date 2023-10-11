using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]public GameObject[] obstaclePrefabs;
    [SerializeField]public Tilemap[] walls;
    [SerializeField]public Transform spawnPoint;
    [SerializeField]public float minSpawnDelay = 1.0f;
    [SerializeField]public float maxSpawnDelay = 3.0f;

    private float nextSpawnTime;

    private void Start()
    {
        // Initialize the next spawn time
        nextSpawnTime = Time.time + Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    private void Update()
    {
        // Check if it's time to spawn an obstacle
        if (Time.time >= nextSpawnTime)
        {
            Debug.Log(spawnPoint.position.y+"+LS: "+spawnPoint.localScale.y);
            // Randomly select an obstacle prefab
            GameObject randomObstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

            // Calculate a random spawn position within the boundaries
            Vector3 randomSpawnPosition = new Vector3(
                0f,
                Random.Range(spawnPoint.position.y, spawnPoint.position.y *2),
                0
            );
            // Instantiate the obstacle at the random position
            Instantiate(randomObstacle, randomSpawnPosition, Quaternion.identity, GameObject.FindGameObjectWithTag("Obstacles").transform);

            // Update the next spawn time
            nextSpawnTime = Time.time + Random.Range(minSpawnDelay, maxSpawnDelay);
        }
    }
}
