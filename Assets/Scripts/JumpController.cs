using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class JumpController : MonoBehaviour
{

    private Rigidbody2D playerBody;
    private Animator playerAnim;
    private SpriteRenderer sprite;
    private BoxCollider2D playerCollider;
    private float referencePoint;
    private float touchMoveDelta;
    private bool touchStart;
    public static bool gameStart;


    [SerializeField] private ArrowSizeController arrowController;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private Camera cam;
    [SerializeField] private Transform arrow;



    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("Cam: " + cam.transform.position.ToString());
        playerBody = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
        cam = Camera.main;
        arrow.gameObject.SetActive(false);
        touchMoveDelta = 1;
        touchStart = false;
        gameStart = false;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerPos;

        if (IsTouchingLeftRight(true) || IsTouchingLeftRight(false) || IsGrounded())
        {
            if ((Input.GetMouseButtonDown(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)) && !touchStart)
            {
                arrow.gameObject.SetActive(true);
                touchStart = true;


                playerPos = playerBody.transform.position;
                referencePoint = Vector2.Distance(playerPos, GetTouchWorldPosition());
                Debug.Log("************ARROW REFERENCE DISTANCE MADE: " + referencePoint);
              
                float angle = Mathf.Atan2(GetTouchWorldPosition().y - playerPos.y, GetTouchWorldPosition().x - playerPos.x) * Mathf.Rad2Deg;
                arrow.transform.eulerAngles = new Vector3(0, 0, angle - 90f);
            }
             else if ((Input.GetAxisRaw("Mouse X") != 0 || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)) )
            {
                arrow.gameObject.SetActive(true);



                playerPos = playerBody.transform.position;
                float distanceFromPlayer1 = CalculateResultVector(playerPos, GetTouchWorldPosition()).magnitude;
                touchMoveDelta = distanceFromPlayer1 / referencePoint;
                arrowController.MakeArrowsBigger(touchMoveDelta);
                Debug.Log("************ARROW DISTANCE: " + distanceFromPlayer1);
                Debug.Log("************ARROW DISTANCE DELTA: " + distanceFromPlayer1 / referencePoint);


                float angle = Mathf.Atan2(GetTouchWorldPosition().y - playerPos.y, GetTouchWorldPosition().x - playerPos.x) * Mathf.Rad2Deg;
                arrow.transform.eulerAngles = new Vector3(0, 0, angle - 90f);

            }
            if ((Input.GetMouseButtonUp(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)) )
            {
                gameStart = true;
                touchStart = false;
                arrow.gameObject.SetActive(false);
                JumpPlayerTowardsTouch();

            }


        }


    }


    private Vector2 GetTouchWorldPosition()
    {
        Vector2 touchPos = Input.GetTouch(0).position;
        Vector2 touchPosWorld = ConvertScreenToWorldPoint(touchPos);
        return touchPosWorld;
    }

    private Vector2 GetClickWorldPosition()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector2 mousePosWorld = ConvertScreenToWorldPoint(mousePos);
        return mousePosWorld;
    }

    private void JumpPlayerTowardsTouch()
    {
        playerBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        //Vector2 mousePosWorld = GetClickWorldPosition();
        Vector2 touchPosWorld = GetTouchWorldPosition();
        Vector2 playerPos = transform.position;

        Vector2 resultVector1 = CalculateResultVector(playerPos, touchPosWorld);
        //Vector2 resultVector2 = CalculateResultVector(playerPos, mousePosWorld);

        //float distanceFromPlayer1 = Vector2.Distance(resultVector1.normalized, playerPos.normalized);
        float distanceFromPlayer1 = resultVector1.magnitude;
        //float distanceFromPlayer2 = Vector2.Distance(resultVector2.normalized, playerPos.normalized);


        Debug.Log("Touch World X: " + touchPosWorld.x + " Touch World y: " + touchPosWorld.y);
        Debug.Log("Touch Distance: " + distanceFromPlayer1);


        Debug.Log("Player X: " + playerPos.x + "Player y: " + playerPos.y);


        //playerBody.velocity = new Vector2(resultVector2.x * distanceFromPlayer2 * jumpForce, resultVector2.y * distanceFromPlayer2 * jumpForce);

        if (touchMoveDelta >= 0.6f && touchMoveDelta <= 1.65f)
        {
            playerBody.velocity = new Vector2(resultVector1.x * touchMoveDelta * jumpForce, resultVector1.y * touchMoveDelta * jumpForce);
        }
        else if( touchMoveDelta < 0.6f)
        {
            playerBody.velocity = new Vector2(resultVector1.x * 0.6f * jumpForce, resultVector1.y * 0.6f * jumpForce);
        }
        else if( touchMoveDelta >= 1.65f)
        {
            playerBody.velocity = new Vector2(resultVector1.x * 1.65f * jumpForce, resultVector1.y * 1.65f * jumpForce);
        }
        else
        {
            playerBody.velocity = new Vector2(resultVector1.x * jumpForce, resultVector1.y * jumpForce);
        }

    }

    private Vector2 ConvertScreenToWorldPoint(Vector2 screenPoint)
    {
        Vector2 worldPoint = cam.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, cam.nearClipPlane));

        return worldPoint;
    }

    private Vector2 CalculateResultVector(Vector2 startPos, Vector2 targetPos)
    {
        Vector2 resultVector = targetPos - startPos;
        return resultVector;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            playerBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }

    }
 
    


    private bool IsGrounded()
    {
        return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }


    private bool IsTouchingLeftRight(bool leftRight)
    {
        if (leftRight)
        {
            return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.left, .1f, jumpableGround);
        }
        else
        {
            return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.right, .1f, jumpableGround);
        }

    }
}
