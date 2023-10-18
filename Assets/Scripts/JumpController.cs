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
    private bool offRange = false;



    public static bool gameStart;


    [SerializeField] private ArrowSizeController arrowController;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private Transform arrow;
    [SerializeField] private TextMesh scoreTextMesh;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource landSound;






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

            if (IsTouchingLeftRight(true) || IsTouchingLeftRight(false) || IsGrounded() )
            {
                if (((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)) && !touchStart)
                {
                    finalVector = new Vector2(0, 0);
                    scoreTextMesh.gameObject.SetActive(false);
                    touchStart = true;

                    playerPos = playerBody.transform.position;
                    referencePoint = Vector2.Distance(playerPos, GetTouchWorldPosition());


                    float angle = Mathf.Atan2(GetTouchWorldPosition().y - playerPos.y, GetTouchWorldPosition().x - playerPos.x) * Mathf.Rad2Deg;


                    if ( IsTouchingLeftRight(true))
                    {
                        if (angle >= 80f)
                        {
                            arrow.transform.eulerAngles = new Vector3(0, 0, 80f - 90f);
                            finalVector = DegreeToVectorAngle(80f - 90f);
                            offRange = true;
                        }
                        else if (angle <= -80f)
                        {
                            arrow.transform.eulerAngles = new Vector3(0, 0, -80f - 90f);
                            finalVector = DegreeToVectorAngle(-80f - 90f);
                            offRange = true;
                        }
                        else
                        {
                            offRange = false;
                            arrow.transform.eulerAngles = new Vector3(0, 0, angle - 90f);
                        }

                    }
                    else if ( IsTouchingLeftRight(false) )
                    {
                        if (angle <= 100f && angle >= 0f)
                        {
                            arrow.transform.eulerAngles = new Vector3(0, 0, 100f - 90f);
                            finalVector = DegreeToVectorAngle(100f - 90f);
                            offRange = true;
                        }
                        else if (angle >= -100f && angle <= 0f)
                        {
                            arrow.transform.eulerAngles = new Vector3(0, 0, -100f - 90f);
                            finalVector = DegreeToVectorAngle(-100f - 90f);
                            offRange = true;
                        }
                        else
                        {
                            arrow.transform.eulerAngles = new Vector3(0, 0, angle - 90f);
                            offRange = false;
                        }
                    }
                    else if (IsGrounded() && !IsTouchingLeftRight(true) && !IsTouchingLeftRight(false) )
                    {
                        if (angle <= 10f && angle >= -90f)
                        {
                            arrow.transform.eulerAngles = new Vector3(0, 0, 10f - 90f);
                            finalVector = DegreeToVectorAngle(10f - 90f);
                            offRange = true;
                        }
                        else if (angle >= 170f || (angle >= -180f && angle <= -90f))
                        {
                            arrow.transform.eulerAngles = new Vector3(0, 0, 170f - 90f);
                            finalVector = DegreeToVectorAngle(170f - 90f);
                            offRange = true;
                        }
                        else
                        {
                            arrow.transform.eulerAngles = new Vector3(0, 0, angle - 90f);
                            offRange = false;
                        }
                    }

                    if (offRange)
                    {
                        touchStart = false;
                    }

                    arrow.gameObject.SetActive(true);
                    arrowController.MakeArrowsBigger(1);


                }
                else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)  
                {
                    playerPos = playerBody.transform.position;

                    float distanceFromPlayer = CalculateResultVector(playerPos, GetTouchWorldPosition()).magnitude;
                    touchMoveDelta = distanceFromPlayer / referencePoint;
                    arrowController.MakeArrowsBigger(touchMoveDelta);



                    float angle = Mathf.Atan2(GetTouchWorldPosition().y - playerPos.y, GetTouchWorldPosition().x - playerPos.x) * Mathf.Rad2Deg;

                    if( IsTouchingLeftRight(true)  )
                    {
                        if (angle >= 80f) {finalVector = DegreeToVectorAngle(80f-90f);}
                        else if (angle <= -80f) {finalVector = DegreeToVectorAngle(-80f-90f);}
                        else {arrow.transform.eulerAngles = new Vector3(0, 0, angle - 90f);finalVector = new Vector2(0, 0);}
                    }
                    else if ( IsTouchingLeftRight(false) )
                    {
                        if (angle <= 100f && angle >= 0f) {finalVector = DegreeToVectorAngle(100f-90f);}
                        else if (angle >= -100f && angle <= 0f) {finalVector = DegreeToVectorAngle(-100f-90f);}
                        else {arrow.transform.eulerAngles = new Vector3(0, 0, angle - 90f);finalVector = new Vector2(0, 0);}
                    }
                    else if (IsGrounded() && !IsTouchingLeftRight(true) && !IsTouchingLeftRight(false))
                    {
                        if (angle <= 10f && angle >= -90f){finalVector = DegreeToVectorAngle(10f-90f);}
                        else if(angle >= 170f || (angle >= -180f && angle <= -90f)){finalVector = DegreeToVectorAngle(170f-90f);}
                        else{arrow.transform.eulerAngles = new Vector3(0, 0, angle - 90f);finalVector = new Vector2(0, 0);}
                    }

                }
                  if ((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended) && arrow.gameObject.activeSelf)
                    {

                        gameStart = true;
                        touchStart = false;
                        arrow.gameObject.SetActive(false);
                        JumpPlayerTowardsTouch();
                    jumpSound.Play();
                }



            }
        }
        


    }
    private Vector2 DegreeToVectorAngle(float angle)
    {
        float radians = (angle + 90f) * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
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

        Vector2 resultVector = CalculateResultVector(playerPos, touchPosWorld).normalized;

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
          //  finalVector = finalVector * (resultVector / resultVector.normalized);

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
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground"))
        {
            landSound.Play();
        }
    }




    private bool IsGrounded()
    {
        return (Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround));
    }

    private bool HeadCovered()
    {
        return (Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.up, .2f, jumpableGround));
    }

    private bool IsTouchingLeftRight(bool leftRight)
    {
        if (leftRight)
        {
            return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.left, .2f, jumpableGround);
        }
        else
        {
            return Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0f, Vector2.right, .2f, jumpableGround);
        }

    }
}
