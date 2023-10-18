using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveObstacle : MonoBehaviour
{
    private Rigidbody2D obstacle;

    [SerializeField] private float speed = 2f;

    private void Start()
    {
        obstacle = GetComponent<Rigidbody2D>();
        if(obstacle.bodyType == RigidbodyType2D.Dynamic)
        {
            obstacle.velocity = new Vector2(speed, 0f);
        }

    }


    private void Update()
    {

        if(JumpController.gameStart == false)
        {
            if(obstacle.bodyType == RigidbodyType2D.Dynamic)
            {
                obstacle.velocity = new Vector2(0, 0);
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        if ((collision.gameObject.CompareTag("Wall")|| collision.gameObject.CompareTag("Obstacle"))&& obstacle.bodyType == RigidbodyType2D.Dynamic){
            speed = -speed;
            obstacle.velocity = new Vector2(speed, 0f);
        }

    }

}
