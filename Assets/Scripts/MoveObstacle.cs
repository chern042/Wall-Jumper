using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    private Rigidbody2D obstacle;

    [SerializeField] private float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        obstacle = GetComponent<Rigidbody2D>();
        obstacle.velocity = new Vector2(speed, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("Wall")|| collision.gameObject.CompareTag("Obstacle")){
            speed = -speed;
            obstacle.velocity = new Vector2(speed, 0f);
        }

    }

}
