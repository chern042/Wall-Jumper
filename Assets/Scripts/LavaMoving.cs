using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMoving : MonoBehaviour
{
    private float moveSpeed = 200f;
    private Rigidbody2D rb;
    private CompositeCollider2D lavaCollider;
    [SerializeField] private LayerMask jumpableGround;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lavaCollider = GetComponent<CompositeCollider2D>();

        // Set the velocity to move upward at a constant speed
    }

    private void Update()
    {
        if(JumpController.gameStart == true)
        {
            if (!IsTouchingLeftRight(true) && !IsTouchingLeftRight(false))
            {
                moveSpeed = 2000f;
            }
            else
            {
                moveSpeed = 200f;
            }
            rb.velocity = new Vector2(0f, moveSpeed * Time.deltaTime);

        }
        else
        {
            rb.velocity = new Vector2(0f, 0f);
        }



    }



    private bool IsTouchingLeftRight(bool leftRight)
    {
        if (leftRight)
        {
            return Physics2D.BoxCast(lavaCollider.bounds.center, lavaCollider.bounds.size, 0f, Vector2.left, .1f, jumpableGround);
        }
        else
        {
            return Physics2D.BoxCast(lavaCollider.bounds.center, lavaCollider.bounds.size, 0f, Vector2.right, .1f, jumpableGround);
        }

    }
}
