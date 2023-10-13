using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    private Rigidbody2D obstacle;

    [SerializeField] private float speed = 2f;
    [SerializeField] private Transform lava;
    [SerializeField] private GameObject objectSelfReference;

    // Start is called before the first frame update
    void Start()
    {
        obstacle = GetComponent<Rigidbody2D>();
        obstacle.velocity = new Vector2(speed, 0f);
    }


    private void Update()
    {
        if (Camera.main.transform.position.y/2 > transform.position.y)
        {
            Destroy(objectSelfReference);
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
