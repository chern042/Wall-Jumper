﻿using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;

    private Rigidbody2D playerBody;
    private Animator playerAnim;
    private SpriteRenderer sprite;
    private BoxCollider2D playerCollider;

    private float dirX = 0f;
    private enum MovementState { idle, running, jumping, falling };

    // Use this for initialization
    void Start()
	{
        playerAnim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerBody = GetComponent<Rigidbody2D>();

    }


    // Update is called once per frame
    void Update()
    {

    }

    private void UpdateAnimationState()
    {
        MovementState state;

       state = MovementState.idle;
       //playerAnim.SetBool("running", false);


        if (playerBody.velocity.y > 0.1f)
        {
            state = MovementState.jumping;

        }
        else if (playerBody.velocity.y < -0.2f)
        {
            state = MovementState.falling;
        }



        playerAnim.SetInteger("state", (int)state);

    }




    private bool IsGrounded()
    {
        return (Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, .1f, interactableLayer)) || (Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.up, .1f, interactableLayer));
    }

    private bool IsTouchingLeftRight(bool leftRight)
    {
        if (leftRight)
        {
            return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.left, .1f, interactableLayer);
        }
        else
        {
            return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.right, .1f, interactableLayer);
        }

    }
}
