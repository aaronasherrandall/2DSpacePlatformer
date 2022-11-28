using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerMovement : MonoBehaviour
{
    MakeChildOfPlanet makeChildOfPlanet;
    Shooter shooter;
    //Weapon weapon;

    public SpriteRenderer attackPointSR;
    public Transform attackPointTransform;
    
    //Navi
    public Transform weaponAttachPoint;
    private float attachXValue;

    private Rigidbody2D myRigidBody2D;
    private SpriteRenderer mySpriteRenderer;
    private Animator playerAnimator;
    public float horizontalMovement, verticalMovement;


    //Inventory
    private Inventory inventory;

    public bool isInLowestOrbit;

    [SerializeField] private float moveSpeed;

    //Handle Jumping
    private float jumpPower = 12.2f;
    public bool playerJustJumped;
    public bool onSurface;

    //Jumping
    public bool isJumping;
    
    //Grounded Check
    public Transform groundCheckPoint;
    public bool isGrounded;

    //Handle Off Planet Movement
    public Vector3 offPlanetVector;
    public float offPlanetVelocity = 1.005f;
    public float moveSpeedOffPlanet;
    public float jetPackBoostSpeed;
    public float jetPackBoostOffSet;

    //Reference for Off Planet 
    Vector2 offPlanetMovement;
    public Vector3 changeInMovementOffPlanet;

    //Handle Rotation on JetPack oldControls
    Quaternion playerLerpStartRotation; //4 dimensional math; must use when rotating objects so we don't have "gimble lock"; Euler are 3D math
    float playerLerpTimer;

    [Header("Feedbacks")]
    //FEEL Feedbacks 
    public MMFeedbacks JumpFeedback;
    public MMFeedbacks LandingFeedback;

    //Variables to Calculate Landing
    public float velocityLastFrame;
    private const float _lowVelocity = 0.1f;

    //Other Variables to calculate Landing
    public LayerMask whatisGround;

    //Store state as a member variable
    public PlayerState playerState;
    public enum PlayerState
	{
        OnSurface,
        EnteringPlanet,
        LeavingPlanet,
        OffPlanet,
        //DialogueIsActive
    }

    private void Awake()
	{
        //Set the below active after states are complete
        //state = State.OnSurface;
        myRigidBody2D = GetComponent<Rigidbody2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        inventory = new Inventory();
        shooter = GetComponentInChildren<Shooter>();
        //weapon = GetComponentInChildren<Weapon>();
        attachXValue = weaponAttachPoint.localPosition.x;

    }

	// Start is called before the first frame update
	void Start()
    {
        isInLowestOrbit = false;
        playerLerpTimer = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        //if (DialogueManager.dialougeIsActive)
        //{
        //    playerState = PlayerState.DialogueIsActive;
        //    Debug.Log("Dialogue Is Active State ON");
        //    return;
        //}

        switch (playerState)
		{
			case PlayerState.OnSurface:
				HandleSurfaceMovement();
				HandleJumping();
				break;
            case PlayerState.OffPlanet:
                HandleOffPlanetMovement();
                break;
            case PlayerState.EnteringPlanet:
				HandleEnteringPlater();
				break;
            //case PlayerState.DialogueIsActive:
            //    break;
		}


        //if(isJumping && (velocityLastFrame < 0) && (MathF.Abs(myRigidBody2D.velocity.x) < _lowVelocity))
        if (isJumping && (velocityLastFrame < 0) && (MathF.Abs(myRigidBody2D.velocity.y) < _lowVelocity))  
		{
            isJumping = false;
            LandingFeedback?.PlayFeedbacks();
            Debug.Log("Playing Landing Feedback");

		}
        velocityLastFrame = myRigidBody2D.velocity.y;

  //      if(Input.GetMouseButton(0))
		//{
  //          Debug.Log("Pressed Left Mouse Button");
  //          //weapon.isFiring = true;
		//}
  //      else
		//{
  //          //weapon.isFiring = false;
		//}
        
	}

    private void FixedUpdate()
    {
        //if (DialogueManager.dialougeIsActive)
        //{
        //    playerState = PlayerState.DialogueIsActive;
        //    Debug.Log("Dialogue Is Active State ON");
        //    return;
        //}


        if (playerState == PlayerState.OnSurface)
        {
            //Apply a force to player Rigid Body2D in direction of player's right axis
            myRigidBody2D.AddForce(transform.right * horizontalMovement * moveSpeed);
        }
        if (playerState == PlayerState.OffPlanet)
        {
            myRigidBody2D.AddForce(changeInMovementOffPlanet * jetPackBoostSpeed * Time.deltaTime * jetPackBoostOffSet, ForceMode2D.Force);
            transform.Rotate(new Vector3(0, 0, 1f) * 50f * Time.deltaTime);
            //transform.localEulerAngles = new Vector3(0, 0, 90);
        }

    }

    private void HandleSurfaceMovement()
    {
        //Handle Movement
        //horizontalMovement will return a float between -1 to 1
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        mySpriteRenderer.flipX = horizontalMovement > 0 ? true : (horizontalMovement < 0 ? false : mySpriteRenderer.flipX);

     
        var currentPosition = weaponAttachPoint.localPosition;
        currentPosition.x = Mathf.MoveTowards(currentPosition.x, Mathf.Abs(attachXValue) * (mySpriteRenderer.flipX ? -1 : 1), 1.5f * Time.deltaTime);
        weaponAttachPoint.localPosition = currentPosition;
        //weapon.ChangeWeaponFlip(mySpriteRenderer.flipX);

        //attackPointSR.flipX = mySpriteRenderer.flipX;

        //if (!mySpriteRenderer.flipX)
        //{
        //    attackPointTransform.localPosition = new Vector3(-0.43f, 0.09f, 1f);
        //}
        //else attackPointTransform.localPosition = new Vector3(0.43f, 0.09f, 1f);

        //attackPoint.gameObject.GetComponent<SpriteRenderer>().flipX = mySpriteRenderer.flipX; 

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatisGround);
    }
    private void HandleJumping()
    {
        //Behavior for this state
        if(Input.GetKeyDown("space"))
		{
            //isInLowOrbit is basically isGrounded
            //Calculated by isInLowestOrbit = distance < 0.5f; in OnTriggerStay2D
            if (isInLowestOrbit)
			{
                myRigidBody2D.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
                Debug.Log("Jump Pressed");
                playerJustJumped = true;
                isJumping = true;
                JumpFeedback?.PlayFeedbacks();
                Debug.Log("Player Jump Feedback");
            }
		}
    }

    private void HandleOffPlanetMovement()
    {
        //NormalizeRotation();
        changeInMovementOffPlanet = Vector3.zero;
        changeInMovementOffPlanet.x = Input.GetAxisRaw("Horizontal");
        changeInMovementOffPlanet.y = Input.GetAxisRaw("Vertical");

        //Turn this into a coroutine?

        //myRigidBody2D.AddForce(changeInMovement * jetPackBoostSpeed * Time.deltaTime * jetPackBoostOffSet, ForceMode2D.Force);
        //myRigidBody2D.velocity = myRigidBody2D.velocity / offPlanetVelocity;
        //Spinning animation
    }

    private void OffPlanetControlsOn()
	{
        

    }
    private void HandleLeavingPlanet()
    {
        //Behavior for this state
        throw new NotImplementedException();
    }

    private void HandleEnteringPlater()
    {
        //Behavior for this state
        throw new NotImplementedException();
    }

    public void NormalizeRotation()
    {
        if (playerLerpTimer < 1 && !isInLowestOrbit) //can check !isInLowestOrbit multiple times
        {
            playerLerpTimer += Time.deltaTime; // Time.deltaTime / 2 -- or other offset to see Slerp better 
            gameObject.transform.rotation = Quaternion.Slerp(playerLerpStartRotation, Quaternion.identity, playerLerpTimer); // Quaternion.identity (0,0,0,0)

        }
    }






	public void OnTriggerStay2D(Collider2D obj)
    {
        if (obj.CompareTag("Planet") /*&& DialogueManager.dialougeIsActive == false*/)
        {
            playerState = PlayerState.OnSurface;
            isInLowestOrbit = true;
            myRigidBody2D.drag = 3f;
            
            float distance = Mathf.Abs(obj.GetComponent<GravityPoint>().planetRadius - Vector2.Distance(transform.position, obj.transform.position));
            //Debug.Log("On Planet " + distance);
            if (distance < 1f)
            {
                isInLowestOrbit = distance < 0.5f;
            }
        }
    }

	public void OnTriggerExit2D(Collider2D collision)
	{
        if(collision.CompareTag("Planet"))
		{
            playerState = PlayerState.OffPlanet;
            isInLowestOrbit = false;
            myRigidBody2D.drag = 0.5f;

            //offPlanetVector takes current jump velociy and adds jump power so when we leave collider, we have force applied
            offPlanetVector = transform.up * jumpPower;
            //Gives us push once off planet; starts to drift and uses drag to slow down
            myRigidBody2D.AddForce(offPlanetVector);
            playerJustJumped = false;
            playerLerpTimer = 0;
            playerLerpStartRotation = gameObject.transform.rotation;
        }
        if(collision.CompareTag("OuterLayer"))
		{
            playerState = PlayerState.OffPlanet;
            myRigidBody2D.drag = .1f;
            isInLowestOrbit = false;

            //offPlanetVector takes current jump velociy and adds jump power so when we leave collider, we have force applied
            offPlanetVector = transform.up * jumpPower;
            //Gives us push once off planet; starts to drift and uses drag to slow down
            myRigidBody2D.AddForce(offPlanetVector);
            playerJustJumped = false;
            playerLerpTimer = 0;
            playerLerpStartRotation = gameObject.transform.rotation;

        }
        
    }
    

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Surface")
        {
            onSurface = true;
            playerAnimator.SetBool("onSurface", true);
            //Debug.Log("ON SURFACE");

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Surface")
        {
            onSurface = false;
            playerAnimator.SetBool("onSurface", false);
            //Debug.Log("OFF SURFACE");
            playerJustJumped = false;
        }
    }
}


