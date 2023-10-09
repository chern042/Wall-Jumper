using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void Update()
    {
        if(player.position.y > 13f && player.gameObject.GetComponent<Rigidbody2D>().velocity.y > 0.1f && player.position.y >= Camera.main.transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, player.position.y, transform.position.z);

        }
        
    }
}
