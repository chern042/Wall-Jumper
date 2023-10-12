using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]public GameObject[] obstaclePrefabs;
    [SerializeField]public Tilemap[] walls;
    [SerializeField]public Transform spawnPoint;
    [SerializeField]private LayerMask wall;
    [SerializeField]public float minSpawnDelay = 1.0f;
    [SerializeField]public float maxSpawnDelay = 3.0f;
    [SerializeField] private int obstacleCount = 10;


    private int[] yCoordsSpawned;
    private bool foundYPos;
    private float nextSpawnTime;
    private int lastY;

    private void Start()
    {
        // Initialize the next spawn time
        nextSpawnTime = Time.time + Random.Range(minSpawnDelay, maxSpawnDelay);
        yCoordsSpawned = new int[1];
        foundYPos = false;
        lastY = 0;
    }

    private void Update()
    {

        // Check if it's time to spawn an obstacle
        Debug.Log(GameObject.FindGameObjectsWithTag("Obstacle").Length);
        if (GameObject.FindGameObjectsWithTag("Obstacle").Length <=obstacleCount)
        {
            // Randomly select an obstacle prefab
            GameObject randomObstacle = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

            int yPoint = (int)Random.Range((spawnPoint.position.y*1.5f)+lastY, spawnPoint.position.y * 4);
            if (yCoordsSpawned.Length == 1)
            {
                yCoordsSpawned[0] = yPoint;
                // Calculate a random spawn position within the boundaries
                Vector3 randomSpawnPosition = new Vector3(-2f,yPoint,0);

                PlaceObstacles(randomObstacle, randomSpawnPosition, yPoint);

            }
            else
            {
                foreach (int point in yCoordsSpawned)
                {
                    if (yPoint == point || yPoint == point - 1 || yPoint == point + 1 || yPoint == point - 2 || yPoint == point + 2)
                    {
                        foundYPos = true;
                    }
                }

                if (!foundYPos)
                {
                    //Debug.Log(yCoordsSpawned.Length);
                    yCoordsSpawned[yCoordsSpawned.Length-1] = yPoint;

                    // Calculate a random spawn position within the boundaries
                    Vector3 randomSpawnPosition = new Vector3( -2f,yPoint,0);

                    PlaceObstacles(randomObstacle, randomSpawnPosition, yPoint);


                }
            }
            foundYPos = false;
            if (yCoordsSpawned.Length >= 50)
            {
                Array.Resize(ref yCoordsSpawned, 1);

            }




        }
    }

    private void PlaceObstacles(GameObject randomObstacle, Vector3 randomSpawnPosition, int yPoint)
    {
        // Instantiate the obstacle at the random position
        GameObject obstacle = Instantiate(randomObstacle, randomSpawnPosition, Quaternion.identity, GameObject.FindGameObjectWithTag("Obstacles").transform);
        Rigidbody2D obBody = obstacle.GetComponent<Rigidbody2D>();
        PolygonCollider2D obCollider = obstacle.GetComponent<PolygonCollider2D>();

        if (Random.Range(0, 2) == 0)
        {
            obBody.bodyType = RigidbodyType2D.Static;
        }

        if (IsTouchingLeftRight(true, obCollider))
        {
            obBody.transform.position = new Vector3(obBody.transform.position.x - 2, obBody.transform.position.y, obBody.transform.position.z);
        }else if (IsTouchingLeftRight(false, obCollider))
        {

            obBody.transform.position = new Vector3(obBody.transform.position.x + 2, obBody.transform.position.y, obBody.transform.position.z);

        }

        // Update the next spawn time
        nextSpawnTime = Time.time + Random.Range(minSpawnDelay, maxSpawnDelay);
        lastY = yPoint;
        Array.Resize(ref yCoordsSpawned, yCoordsSpawned.Length + 1);
    }


    private bool IsTouchingLeftRight(bool leftRight, PolygonCollider2D collider)
    {
        if (leftRight)
        {
            return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.left, .2f, wall);
        }
        else
        {
            return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.right, .2f, wall);
        }

    }
}
