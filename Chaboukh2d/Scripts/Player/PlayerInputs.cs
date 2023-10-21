using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// Deals with All the Inputs
/// </summary>
public class PlayerInputs : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;
    internal float horizontal;
    internal float Vertical;
    internal bool isJumpPressed;
    internal bool isJumping;
    internal bool InteractPressed;
    internal bool SpellPressed;
    internal bool SlideRequested;
    internal bool FallFromPlatform;

    //Counters=========================
    internal float jumpTimeCounter;
    internal float hangTimeCounter;

    internal bool keepPushingJump;

    internal bool umbrellaIsOpen;
    internal bool RopeJumpRequested;


    //public Joystick joystick;

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        if (keepPushingJump)
        {
            playerScript.PlayerRigideBody.velocity = new Vector2(playerScript.PlayerRigideBody.velocity.x, 0f);
            playerScript.PlayerRigideBody.AddForce(Vector2.up * playerScript.playerController.PlayerJumpForce, ForceMode2D.Impulse);

            jumpTimeCounter -= Time.fixedDeltaTime;
        }
    }
    void Update()
    {
        if (!PlayerScript.Instance.OpenedTheDoor)
        {
            //horizontal = joystick.Horizontal;
            //Vertical = joystick.Vertical;
            horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            Vertical = CrossPlatformInputManager.GetAxis("Vertical");
        }


        //FallingFromAPlatform
        if (CrossPlatformInputManager.GetButtonDown("Jump") && playerScript.playerAnimation.isCrouching)
        {
            FallFromPlatform = true;
        }


        //Jump

        playerScript.playerController.jumpPressedRememberTimer -= Time.deltaTime;
        if (CrossPlatformInputManager.GetButtonDown("Jump") && !playerScript.playerInputs.SpellPressed)
        {
            playerScript.playerController.jumpPressedRememberTimer = playerScript.playerController.jumpPressedRememberTime;
            RopeJumpRequested = true;
        }
        if (playerScript.playerController.jumpPressedRememberTimer > 0 && hangTimeCounter > 0)
        {
            isJumpPressed = true;
        }
        else
        {
            isJumpPressed = false;
        }
        //if (playerScript.weapon != PlayerScript.Weapon.unarmed)
        //{
        //    playerScript.playerController.jumpPressedRememberTimer = 0;
        //}
        if (CrossPlatformInputManager.GetButton("Jump") && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                keepPushingJump = true;
            }
            else
            {
                keepPushingJump = false;
                isJumping = false;
            }
        }
        else if (CrossPlatformInputManager.GetButtonUp("Jump"))
        {
            isJumping = false;
            keepPushingJump = false;

        }

        // Interact
        if (CrossPlatformInputManager.GetButtonDown("Action"))
        {
            if (playerScript.weapon != PlayerScript.Weapon.unarmed)
            {
                InteractPressed = true;
            }
        }
        if (CrossPlatformInputManager.GetButtonDown("Skill") && !CrossPlatformInputManager.GetButton("Jump"))
        {
            if (playerScript.HoldingUmbrella)
            {
                if (!playerScript.playerController.IsGrounded() && !playerScript.playerController.IsLadder && playerScript.weapon == PlayerScript.Weapon.unarmed)
                {
                    umbrellaIsOpen = !umbrellaIsOpen;
                }
            }
        }
        if (Vertical < -0.5f && playerScript.playerController.IsGrounded() && playerScript.weapon == PlayerScript.Weapon.unarmed && playerScript.CanSlide)
        {
            SlideRequested = true;
        }

        if (CrossPlatformInputManager.GetButtonDown("Spell")
            && playerScript.HasMagicPower
            && playerScript.weapon == PlayerScript.Weapon.unarmed
            && playerScript.playerController.IsGrounded()
            && playerScript.playerAnimation.PlayerCurrentAnimationState != "MagicSpell"
            && !playerScript.playerController.IsClimbing
            && !playerScript.playerController.isSliding)
        {
            SpellPressed = true;
        }


    }




    }

//    horizontal = Input.GetAxisRaw("Horizontal");
//        Vertical = Input.GetAxisRaw("Vertical");
//        playerScript.playerController.jumpPressedRememberTimer -= Time.deltaTime;
//        //Jump
//        if (Input.GetKeyDown(KeyCode.Space) && !playerScript.HoldingTheAxe && !playerScript.HoldingTheSpear)
//        {
//            playerScript.playerController.jumpPressedRememberTimer = playerScript.playerController.jumpPressedRememberTime;
//        }
//if (playerScript.playerController.jumpPressedRememberTimer > 0 && hangTimeCounter > 0)
//{
//    isJumpPressed = true;


//}
//if (Input.GetKey(KeyCode.Space) && isJumping == true)
//{
//    if (jumpTimeCounter > 0)
//    {
//        playerScript.PlayerRigideBody.velocity = new Vector2(playerScript.PlayerRigideBody.velocity.x, 0f);
//        playerScript.PlayerRigideBody.AddForce(Vector2.up * playerScript.playerController.PlayerJumpForce, ForceMode2D.Impulse);

//        jumpTimeCounter -= Time.deltaTime;
//    }
//    else
//    {
//        isJumping = false;
//    }
//}
//else if (Input.GetKeyUp(KeyCode.Space))
//{
//    isJumping = false;
//}

//// Interact
//if (Input.GetKeyDown(KeyCode.X) && (playerScript.HoldingTheAxe || playerScript.HoldingTheSpear))
//{
//    InteractPressed = true;
//}

        
//    }


