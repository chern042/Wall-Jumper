using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LavaMoving : MonoBehaviour
{
    private float moveSpeed = 2f;
    private Rigidbody2D rb;
    [SerializeField] private LayerMask wallLayer;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float cameraBottom = Camera.main.transform.position.y - Camera.main.orthographicSize;

        if (JumpController.gameStart == true)
        {
            rb.velocity = new Vector2(0f, moveSpeed);

            if ( cameraBottom > rb.position.y)
            {
                rb.position = new Vector2(rb.position.x,cameraBottom);
                rb.velocity = new Vector2(0f, moveSpeed);

            }

        }
        else
        {
            rb.velocity = new Vector2(0f, 0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(2f, 0f);
            StartCoroutine(DestroyGameObject(collision.gameObject));
        }
    }


    IEnumerator DestroyGameObject(GameObject obstacle)
    {        
        yield return new WaitForSeconds(1);
        Destroy(obstacle);

    }
}
