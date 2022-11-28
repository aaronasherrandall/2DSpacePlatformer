using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    PlayerMovement playerMovement;
    Animator playerAnimator;

    //Animation State References
    const string PLAYER_INSPACE = "Player_InSpace";
    const string PLAYER_IDLE = "Player_Idle";

    [SerializeField]
    private string currentState;

    private void Awake()
	{
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<Animator>();
	}
    // Update is called once per frame
    void Update()
    {
        //Want to wrap this in bools to check that we are in low orbit / on surface
        //This sets our "moveX" float in the animator = to our horizontalMovement; only needs to be greater than -0.1 for trigger;
        if (playerMovement.playerState == PlayerMovement.PlayerState.OnSurface)
		{
            playerAnimator.SetFloat("moveX", Mathf.Abs(playerMovement.horizontalMovement));
        }
        if (playerMovement.playerState == PlayerMovement.PlayerState.OffPlanet)
		{
            playerAnimator.SetBool("offPlanet", true);
            playerAnimator.SetBool("isJumping", false);
            ChangeAnimationState(PLAYER_INSPACE);
            //Add spin freak out animation
        }
        if(playerMovement.playerJustJumped)
		{
            playerAnimator.SetBool("isJumping", true);
        }
        if (playerMovement.isInLowestOrbit)
        {
            SetAnimToIdle();
            //Debug.Log("Anim being set to idle");
        }
    }

	private void FixedUpdate()
	{
        if (playerAnimator.GetBool("isJumping") && (playerMovement.onSurface) && (!playerMovement.playerJustJumped)) //pull isJumping from animator
        {
            playerAnimator.SetBool("isJumping", false);
        }
    }

    //Code to tell how long current animation is
    private void ChangeAnimationState(string newState)
    {
        //stop the same animation from interrupting itself
        if (currentState == newState) return;

        //play the animation
        //_animator.CrossFade(newState, 1f);
        playerAnimator.Play(newState);

        //reassign the current state
        currentState = newState;
    }

    public void SetAnimToIdle()
    {
        playerMovement.playerState = PlayerMovement.PlayerState.OnSurface;
        ChangeAnimationState(PLAYER_IDLE);
        playerAnimator.SetBool("isInLowOrbit", true);
        //Debug.Log("Setting Bool in anim to true");
    }
}
