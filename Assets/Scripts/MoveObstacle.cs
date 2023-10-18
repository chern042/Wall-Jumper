using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveObstacle : MonoBehaviour
{
    private Rigidbody2D obstacle;
    private bool isRotating;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float rotateSpeedConstant = 0.5f;


    private void Start()
    {
        obstacle = GetComponent<Rigidbody2D>();
        isRotating = GetComponent<IsRotated>().isRotated;
        if(obstacle.bodyType == RigidbodyType2D.Dynamic && !isRotating)
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
        }else if (obstacle.bodyType == RigidbodyType2D.Dynamic && isRotating)
        {
            PolygonCollider2D obstacleCollider = GetComponent<PolygonCollider2D>();
            Vector3 centre = obstacleCollider.bounds.center;
            obstacle.transform.RotateAround(centre, Vector3.forward, 360 * rotateSpeedConstant * Time.deltaTime);
            if (obstacleCollider.bounds.size.x >= 6)
            {
                obstacle.velocity = new Vector2(0f, 0f);
            }
            else
            {
                obstacle.velocity = new Vector2(speed, 0f);
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
