using UnityEngine;
using System.Collections;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;

    private Rigidbody2D playerBody;
    private Animator playerAnim;
    private SpriteRenderer sprite;
    private BoxCollider2D playerCollider;
    private MovementState state;

    private enum MovementState { idle, grabbing, jumping, falling, hit };

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
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {

       if(state != MovementState.hit)
        {
            state = MovementState.idle;
        }


        if (playerBody.velocity.y > 0.1f)
        {
            state = MovementState.jumping;
        }
        else if (playerBody.velocity.y < -0.2f)
        {
            state = MovementState.falling;
        }
        else if(( IsTouchingLeftRight(true) || IsTouchingLeftRight(false) ) && ( !IsGrounded() || (IsGrounded() && HeadCovered()) ) )
        {
            state = MovementState.grabbing;
        }

        if(playerBody.velocity.x > 0.2f)
        {
            sprite.flipX = false;
        }
        else if (playerBody.velocity.x < -0.2f)
        {
            sprite.flipX = true;
        }



        playerAnim.SetInteger("state", (int)state);

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Lava") || collision.gameObject.CompareTag("Obstacle")) )
        {
            state = MovementState.hit;
        }
    }




    private bool IsGrounded()
    {
        return (Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, .1f, interactableLayer));
    }

    private bool HeadCovered()
    {
        return (Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.up, .2f, interactableLayer));
    }

    private bool IsTouchingLeftRight(bool leftRight)
    {
        if (leftRight)
        {
            return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.left, .2f, interactableLayer);
        }
        else
        {
            return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.right, .2f, interactableLayer);
        }

    }
}

