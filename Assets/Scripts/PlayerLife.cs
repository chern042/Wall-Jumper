using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{

    private Rigidbody2D playerBody;
    private BoxCollider2D playerCollider;
    [SerializeField] private AudioSource hitSound;

    // Start is called before the first frame update
    private void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
    }





    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Lava") || collision.gameObject.CompareTag("Obstacle"))
        {

            hitSound.Play();
            Die();
 
        }

    }


    private void Die()
    {
        //deathSound.Play();
        playerBody.constraints = RigidbodyConstraints2D.None;

        playerBody.AddForce(new Vector2(-1f, 4f), ForceMode2D.Impulse);
        playerCollider.enabled = false;
        JumpController.gameStart = false;
        GameEndMenu.gameEnded = true;

    }




}
