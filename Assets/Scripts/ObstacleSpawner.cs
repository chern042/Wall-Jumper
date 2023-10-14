using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]public GameObject[] obstaclePrefabs;
    [SerializeField] private int obstacleCount = 10;
    [SerializeField] private int moduloSpawn = 15;


    private int lastY;
    private int lastObs;

    private void Start()
    {
        // Initialize the next spawn time
        lastObs = -1;
        lastY = 0;
    }

    private void Update()
    {

        // Get the top Y position of the camera's current world coordinates
        int yPoint = (int)Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y;

        // Check if it's time to spawn an obstacle
        if(yPoint%moduloSpawn == 0 && lastY != yPoint)
        {
            // Randomly select an obstacle prefab
            int randomObstacleIndex = Random.Range(0, obstaclePrefabs.Length);

            while (lastObs == randomObstacleIndex)
            {
                randomObstacleIndex = Random.Range(0, obstaclePrefabs.Length);
            }

            GameObject randomObstacle = obstaclePrefabs[randomObstacleIndex];



            Vector3 spawnPosition = new Vector3( -4f,yPoint,0);

            if (Random.Range(0, 2) == 0)
            {
                // Instantiate the obstacle at the random position
                GameObject obstacle = Instantiate(randomObstacle, spawnPosition, Quaternion.identity, GameObject.FindGameObjectWithTag("Obstacles").transform);

                Rigidbody2D obBody = obstacle.GetComponent<Rigidbody2D>();
                PolygonCollider2D obCollider = obstacle.GetComponent<PolygonCollider2D>();
                float obstacleWidth = obCollider.bounds.size.x;

                float newXPos = Random.Range(-10f + ((float)Math.Round((decimal)obstacleWidth, 2) / 2), 2f - ((float)Math.Round((decimal)obstacleWidth, 2) / 2));


                if (Random.Range(0, 2) == 0)
                {
                    obBody.bodyType = RigidbodyType2D.Static;
                }
                obBody.transform.position = new Vector3(newXPos, obBody.transform.position.y, obBody.transform.position.z);

            }
            lastY = yPoint;


        }
    }



}
