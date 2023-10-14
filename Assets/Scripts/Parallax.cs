using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Parallax : MonoBehaviour
{
    private Transform cam;
    private Vector3 lastCamPosition;
    private float textureUnitSizeY;

    [SerializeField] private float parallaxEffect = 0.3f;


    private void Start()
    {
        cam = Camera.main.transform;
        lastCamPosition = cam.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeY = texture.width / sprite.pixelsPerUnit;

    }

    void LateUpdate()
    {

        Vector3 deltaMovement = cam.position - lastCamPosition;

        transform.position += new Vector3(deltaMovement.x * parallaxEffect, deltaMovement.y * parallaxEffect);

        lastCamPosition = cam.position;


        if(Math.Abs(cam.position.y - transform.position.y) >= textureUnitSizeY)
        {
            float offsetPositionY = (cam.position.y - transform.position.y) % textureUnitSizeY;

            transform.position = new Vector3(transform.position.x, cam.position.y + offsetPositionY);
        }
    }



}
