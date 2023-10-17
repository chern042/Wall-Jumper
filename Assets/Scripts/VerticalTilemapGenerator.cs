using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VerticalTilemapGenerator : MonoBehaviour
{


    [SerializeField]public Tilemap backgroundTilemapL1;
    [SerializeField]public Tilemap backgroundTilemapL2;
    [SerializeField]public Tilemap backgroundTilemapR1;
    [SerializeField]public Tilemap backgroundTilemapR2;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {

        // Calculate the camera's position relative to the background Tilemaps
        float cameraBottom = mainCamera.transform.position.y + mainCamera.orthographicSize;

        // Check if the camera has moved enough to swap the Tilemaps
        if (cameraBottom >= (backgroundTilemapL1.transform.position.y + backgroundTilemapL1.size.y))
        {
            Debug.Log("Camera Bottom: " + cameraBottom + " Bottom Tile top Y: " + backgroundTilemapL1.transform.position.y + backgroundTilemapL1.size.y);

            // Move backgroundTilemapL1 on top of backgroundTilemapL2
            Vector3 newPosition1 = backgroundTilemapL2.transform.position + Vector3.up * backgroundTilemapL2.size.y;
            backgroundTilemapL1.transform.position = newPosition1;


            // Move backgroundTilemapR1 on top of backgroundTilemapR2
            Vector3 newPosition2 = backgroundTilemapR2.transform.position + Vector3.up * backgroundTilemapR2.size.y;
            backgroundTilemapR1.transform.position = newPosition2;


            // Swap the left Tilemap references
            Tilemap temp = backgroundTilemapL1;
            backgroundTilemapL1 = backgroundTilemapL2;
            backgroundTilemapL2 = temp;

            // Swap the right Tilemap references
            Tilemap temp2 = backgroundTilemapR1;
            backgroundTilemapR1 = backgroundTilemapR2;
            backgroundTilemapR2 = temp2;
        }


    }



}
