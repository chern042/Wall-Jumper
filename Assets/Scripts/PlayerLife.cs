using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class PlayerLife : MonoBehaviour
{

    private Rigidbody2D playerBody;
    private BoxCollider2D playerCollider;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource lavaHitSound;

    private void Start()
    {
        playerBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        hitSound.volume = PlayerPrefs.GetFloat("Sound FX Slider", 1f);
        lavaHitSound.volume = PlayerPrefs.GetFloat("Sound FX Slider", 1f);
    }





    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ( collision.gameObject.CompareTag("Obstacle"))
        {

            hitSound.Play();
            Die();
 
        }else if (collision.gameObject.CompareTag("Lava"))
        {
            lavaHitSound.Play();
            Die();

        }

    }


    private void Die()
    {

         
        playerBody.constraints = RigidbodyConstraints2D.None;
        playerBody.AddForce(new Vector2(-1f, 4f), ForceMode2D.Impulse);
        playerCollider.enabled = false;
        JumpController.gameStart = false;
        GameEndMenu.gameEnded = true;

    }




}
