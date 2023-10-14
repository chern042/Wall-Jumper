using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MoveObstacle : MonoBehaviour
{
    private Rigidbody2D obstacle;

    [SerializeField] private float speed = 2f;
    [SerializeField] private GameObject objectSelfReference;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        obstacle = GetComponent<Rigidbody2D>();
        obstacle.velocity = new Vector2(speed, 0f);
        cam = Camera.main;

    }


    private void Update()
    {
        Debug.Log("CAM Y: " + (cam.transform.position.y-cam.orthographicSize) + "      OBS " + objectSelfReference.name + " Y: " + transform.position.y);
        if ((cam.transform.position.y-cam.orthographicSize)-5 > transform.position.y)
        {
            Destroy(objectSelfReference);
            //StartCoroutine(DestroyGameObject(objectSelfReference));

        }
    }

    IEnumerator DestroyGameObject(GameObject obstacle)
    {
        yield return new WaitForSeconds(1);
        Destroy(obstacle);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        if ((collision.gameObject.CompareTag("Wall")|| collision.gameObject.CompareTag("Obstacle"))&& obstacle.bodyType == RigidbodyType2D.Dynamic){
            speed = -speed;
            obstacle.velocity = new Vector2(speed, 0f);
        }

    }

}
