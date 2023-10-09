using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VerticalTilemapGenerator : MonoBehaviour
{


    [SerializeField]public Tilemap backgroundTilemap1;
    [SerializeField]public Tilemap backgroundTilemap2;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {

        // Calculate the camera's position relative to the background Tilemaps
        float cameraBottom = mainCamera.transform.position.y - mainCamera.orthographicSize;

        // Check if the camera has moved enough to swap the Tilemaps
        if (cameraBottom >= backgroundTilemap1.transform.position.y + backgroundTilemap1.size.y)
        {
            // Move backgroundTilemap1 in front of backgroundTilemap2
            Vector3 newPosition = backgroundTilemap2.transform.position + Vector3.up * backgroundTilemap2.size.y;
            backgroundTilemap1.transform.position = newPosition;

            // Swap the Tilemap references
            Tilemap temp = backgroundTilemap1;
            backgroundTilemap1 = backgroundTilemap2;
            backgroundTilemap2 = temp;
        }


    }



}
