using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]public GameObject[] obstaclePrefabs;
    [SerializeField]private int moduloSpawn = 7;

    private List<GameObject> obstaclesSpawned;
    private List<GameObject> obstaclesToDestroy;


    private int lastY;
    private int lastObs;
    private int check = 0;
    private void Start()
    {
        obstaclesSpawned = new List<GameObject>();
        obstaclesToDestroy = new List<GameObject>();
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
            lastObs = randomObstacleIndex;



            Vector3 spawnPosition = new Vector3( -4f,yPoint,0);

            if (Random.Range(0, 100) < 90)
            {
                // Instantiate the obstacle at the random position
                GameObject obstacle = Instantiate(randomObstacle, spawnPosition, Quaternion.identity, GameObject.FindGameObjectWithTag("Obstacles").transform);
                obstaclesSpawned.Add(obstacle);
                Rigidbody2D obBody = obstacle.GetComponent<Rigidbody2D>();
                PolygonCollider2D obCollider = obstacle.GetComponent<PolygonCollider2D>();
                float obstacleWidth = obCollider.bounds.size.x;

                float newXPos = Random.Range(-10f + ((float)Math.Round((decimal)obstacleWidth, 2) / 2), 2f - ((float)Math.Round((decimal)obstacleWidth, 2) / 2));

                bool isStatic = false;
                if (Random.Range(0, 3)==0)
                {
                    isStatic = true;
                }
                if (isStatic)
                {
                    obBody.bodyType = RigidbodyType2D.Static;
                }

                obBody.transform.position = new Vector3(newXPos, obBody.transform.position.y, obBody.transform.position.z);

                if (Random.Range(0, 3) == 0 && !isStatic)
                {
                    if (obstacleWidth >= 7f)
                    {
                        obBody.transform.position = new Vector3(0, obBody.transform.position.y, obBody.transform.position.z);
                        obstacle.GetComponent<IsRotated>().isRotated = true;
                    }
                    else
                    {
                        obstacle.GetComponent<IsRotated>().isRotated = true;
                    }
                }

            }

            lastY = yPoint;
        }
        check++;

        if (check == 1000)
        {
            check = 0;

            foreach (GameObject i in obstaclesSpawned)
            {
                float obstacleHeight = i.GetComponent<Rigidbody2D>().transform.position.y+3;
                float cameraHeight = Camera.main.transform.position.y - Camera.main.orthographicSize;
                if (obstacleHeight < cameraHeight)
                {
                    obstaclesToDestroy.Add(i);
                }
            }
            if (obstaclesToDestroy.Count != 0)
            {
                foreach (GameObject i in obstaclesToDestroy)
                {
                    obstaclesSpawned.Remove(i);
                    Destroy(i);
                }
                obstaclesToDestroy.Clear();
            }

        }
    }



}
