using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class JumpController : MonoBehaviour
{

    private Rigidbody2D playerBody;
    private BoxCollider2D playerCollider;
    private float referencePoint;
    private float touchMoveDelta;
    private bool touchStart;
    private Camera cam;
    private Vector2 finalVector;



    public static bool gameStart;


    [SerializeField] private ArrowSizeController arrowController;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private Transform arrow;
    [SerializeField] private TextMesh scoreTextMesh;




    // Start is called before the first frame update
    void Start()
    {

        playerBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();

        scoreTextMesh.transform.position = new Vector3(scoreTextMesh.transform.position.x, Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y / 2, scoreTextMesh.transform.position.z);
        cam = Camera.main;
        arrow.gameObject.SetActive(false);
        touchMoveDelta = 1;
        touchStart = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameEndMenu.gameEnded)
        {


            Vector2 playerPos;

            if (IsTouchingLeftRight(true) || IsTouchingLeftRight(false) || IsGrounded())
            {
                if (((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)) && !touchStart)
                {
                    finalVector = new Vector2(0, 0);
                    scoreTextMesh.gameObject.SetActive(false);
                    arrow.gameObject.SetActive(true);
                    touchStart = true;

                    playerPos = playerBody.transform.position;
                    referencePoint = Vector2.Distance(playerPos, GetTouchWorldPosition());
                    // Debug.Log("************ARROW REFERENCE DISTANCE MADE: " + referencePoint);

                    float angle = Mathf.Atan2(GetTouchWorldPosition().y - playerPos.y, GetTouchWorldPosition().x - playerPos.x) * Mathf.Rad2Deg;
                    if ( !( ( (angle >= 80f || angle <= -80f) && !IsGrounded() && IsTouchingLeftRight(true) )  ||
                            ( ( ( angle <= 100f && angle >= 0) || (angle >= -80f && angle <= 0) ) && !IsGrounded() && IsTouchingLeftRight(false) ) ||
                            ( (angle <= 10f  || angle >= 170f) && IsGrounded() ) ) )
                    {
                        arrow.transform.eulerAngles = new Vector3(0, 0, angle - 90f);
                    }
                   
                }
                else if ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved))
                {
                    arrow.gameObject.SetActive(true);



                    playerPos = playerBody.transform.position;

                    float distanceFromPlayer = CalculateResultVector(playerPos, GetTouchWorldPosition()).magnitude;
                    touchMoveDelta = distanceFromPlayer / referencePoint;
                    arrowController.MakeArrowsBigger(touchMoveDelta);
                    //Debug.Log("************ARROW DISTANCE: " + distanceFromPlayer1);
                    //Debug.Log("************ARROW DISTANCE DELTA: " + distanceFromPlayer1 / referencePoint);


                    float angle = Mathf.Atan2(GetTouchWorldPosition().y - playerPos.y, GetTouchWorldPosition().x - playerPos.x) * Mathf.Rad2Deg;
                    Debug.Log(angle);

                    if (( (angle >= 80f) || (angle <= -80f) )&&(!IsGrounded())&&(IsTouchingLeftRight(true)))
                    {
                        arrow.transform.eulerAngles = new Vector3(0, 0, arrow.transform.eulerAngles.z);
                        float radians = (arrow.transform.eulerAngles.z + 90f) * Mathf.Deg2Rad;
                        finalVector = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));


                    }
                    else if (( (angle <= 100f && angle >=0) || (angle >= -80f && angle <= 0) ) && (!IsGrounded()) && (IsTouchingLeftRight(false)))
                    {
                        arrow.transform.eulerAngles = new Vector3(0, 0, arrow.transform.eulerAngles.z);
                        float radians = (arrow.transform.eulerAngles.z + 90f) * Mathf.Deg2Rad;

                        finalVector = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
                    }
                    else if ((angle <= 10f || angle >= 170) && IsGrounded() )
                    {
                        arrow.transform.eulerAngles = new Vector3(0, 0, arrow.transform.eulerAngles.z);
                        float radians = (arrow.transform.eulerAngles.z + 90f) * Mathf.Deg2Rad;
                        finalVector = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
                    }
                    else
                    {
                        arrow.transform.eulerAngles = new Vector3(0, 0, angle - 90f);
                    }

                }


                    if ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended))
                    {

                        gameStart = true;
                        touchStart = false;
                        arrow.gameObject.SetActive(false);
                        JumpPlayerTowardsTouch();

                   
                }



            }
        }
        


    }


    private Vector2 GetTouchWorldPosition()
    {
        Vector2 touchPos = Input.GetTouch(0).position;
        Vector2 touchPosWorld = ConvertScreenToWorldPoint(touchPos);
        return touchPosWorld;
    }


    private void JumpPlayerTowardsTouch()
    {
        playerBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        Vector2 touchPosWorld = GetTouchWorldPosition();
        Vector2 playerPos = transform.position;

        Vector2 resultVector = CalculateResultVector(playerPos, touchPosWorld);

        float angle = Mathf.Atan2(resultVector.y,resultVector.x) * Mathf.Rad2Deg;




        float distanceFromPlayer = resultVector.magnitude;


        Debug.Log("Touch World X: " + touchPosWorld.x + " Touch World y: " + touchPosWorld.y);
      //  Debug.Log("Touch Distance: " + distanceFromPlayer1);


      //  Debug.Log("Player X: " + playerPos.x + "Player y: " + playerPos.y);


        if(finalVector.x == 0 && finalVector.y == 0)
        {
            if (touchMoveDelta >= 0.6f && touchMoveDelta <= 1.65f)
            {
                playerBody.velocity = new Vector2(resultVector.x * touchMoveDelta * jumpForce, resultVector.y * touchMoveDelta * jumpForce);
            }
            else if (touchMoveDelta < 0.6f)
            {
                playerBody.velocity = new Vector2(resultVector.x * 0.6f * jumpForce, resultVector.y * 0.6f * jumpForce);
            }
            else if (touchMoveDelta >= 1.65f)
            {
                playerBody.velocity = new Vector2(resultVector.x * 1.65f * jumpForce, resultVector.y * 1.65f * jumpForce);
            }
            else
            {
                playerBody.velocity = new Vector2(resultVector.x * jumpForce, resultVector.y * jumpForce);
            }
        }
        else
        {
            finalVector = finalVector * (resultVector / resultVector.normalized);

            if (touchMoveDelta >= 0.6f && touchMoveDelta <= 1.65f)
            {
                playerBody.velocity = new Vector2(finalVector.x * touchMoveDelta * jumpForce, finalVector.y * touchMoveDelta * jumpForce);
            }
            else if (touchMoveDelta < 0.6f)
            {
                playerBody.velocity = new Vector2(finalVector.x * 0.6f * jumpForce, finalVector.y * 0.6f * jumpForce);
            }
            else if (touchMoveDelta >= 1.65f)
            {
                playerBody.velocity = new Vector2(finalVector.x * 1.65f * jumpForce, finalVector.y * 1.65f * jumpForce);
            }
            else
            {
                playerBody.velocity = new Vector2(finalVector.x * jumpForce, finalVector.y * jumpForce);
            }
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
        return (Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround))||(Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.up, .1f, jumpableGround));
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
