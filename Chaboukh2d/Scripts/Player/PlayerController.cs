using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// Player movement scripts
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Connection to the playerScript")]
    [SerializeField] PlayerScript playerScript;

    [Header("Movement Variables")]
    [SerializeField] internal float PlayerAcceleration;
    [SerializeField] float PlayerRunSpeed;
    [SerializeField] private float groundLinearDrag;
    [SerializeField] private float airlinearDrag;

    [Header("Jump Variables")]
    [SerializeField] internal float PlayerJumpForce;
    [SerializeField] private float jumpTime;
    [SerializeField] private float fallMultipier;
    [SerializeField] private float lowJumpMultipier;
    [SerializeField] private float hangTime;
    [SerializeField]
    private float _minSpeedtoFall;
    [SerializeField] internal float jumpPressedRememberTime;
    internal float jumpPressedRememberTimer;
    [SerializeField]
    float PlayerAirAcceleration;
    [SerializeField]
    float PlayerAirMovementSpeed;

    [Header("Ground Variables")]
    [SerializeField] private LayerMask whatisGround;
    [SerializeField] Transform[] groundCheck;
    [SerializeField] Transform playerLegs;

    [SerializeField] private LayerMask whatIsObstacle;
    [SerializeField] Transform[] ObstacleCheck_Spear;
    [SerializeField] Transform[] ObstacleCheck_Axe;



    internal bool changingDir => (playerScript.PlayerRigideBody.velocity.x > 0f && playerScript.playerInputs.horizontal < 0f ||
    playerScript.PlayerRigideBody.velocity.x < 0f && playerScript.playerInputs.horizontal > 0f);
    public bool Player_Locking_Right = true;

    bool wasLockingRight = true;
    

    private Vector2 playerScale;

    bool landed;

    [Header("Ladder")]
    internal bool IsLadder = false;
    internal bool IsClimbing = false;
    internal bool topOfLadder;
    internal bool LadderToRight = false;
    [SerializeField] float ladderClimbingSpeed;
    [SerializeField] private float ladderLinearDrag;

    [Header("Axe")]
    [SerializeField] internal GameObject axePrefab;
    [SerializeField] internal Transform axePos;
    [SerializeField] internal Transform axePos2;
    [SerializeField] internal GameObject axePrefabDropped;
    internal bool axeThrown;

    [Header("Spear")]
    [SerializeField] internal GameObject spearPrefab;
    [SerializeField] internal Transform spearPos;
    [SerializeField] internal Transform dropSpearPos;
    [SerializeField] internal GameObject spearPrefabDrop;

    [Header("Magic")]
    [SerializeField] internal GameObject magicArrow;
    [SerializeField] internal Transform magicArrowpos;



    [Header("Particles")]
    [SerializeField] internal ParticleSystem ChangingDirDust;
    [SerializeField] internal ParticleSystem BloodExplosionPrefab;

    [Header("Umbrella")]
    [SerializeField] float gravityScaleWhenUmbrellaIsOpen;
    [SerializeField] GameObject umbrellaPrefab;
    [SerializeField] float umbrellaLinearDrag;
    internal bool DontThrow;

    [Header("Slide")]
    [SerializeField] float slideForce;
    [SerializeField] float slideTime;
    [SerializeField] float timeToSlideAgainAfterSlide;
    [SerializeField] float slideLinearDrag;
    internal bool isSliding;
    internal bool youCanSlideAgain;

    [Header("RopeHanging")]
    [SerializeField] internal bool RopeHanging;

    internal bool killBounce;
    [SerializeField] internal float killBounceForce;
    bool isbouncingFromKill;

    internal bool charachterFlipped;
    void CharachterFlipped()
    {

        if (Player_Locking_Right)
        {
            if (!wasLockingRight)
            {
                wasLockingRight = true;
                if (IsGrounded())
                {
                    charachterFlipped = true;
                }

            }

        }
        else if (!Player_Locking_Right)
        {
            if (wasLockingRight)
            {
                wasLockingRight = false;
                if (IsGrounded())
                {
                    charachterFlipped = true;
                }
            }
        }
        if (!IsGrounded())
        {
            charachterFlipped = false;
        }
    }

    void Start()
    {
        playerScale = transform.localScale;
        Player_Locking_Right = true;
        landed = true;
        youCanSlideAgain = true;
    }

    // Update is called once per frame
    void Update()
    {
        CharachterFlipped();

        //Umbrella Open to false if not holding it
        if (!playerScript.HoldingUmbrella)
        {
            playerScript.playerInputs.umbrellaIsOpen = false;
        }
        if (IsGrounded() || RopeHanging)
        {
            playerScript.playerInputs.umbrellaIsOpen = false;
            isbouncingFromKill = false; 
        }
        if (IsGrounded()) 
        {
            if (changingDir || !landed)
            {
                ChangingDirDust.Play();
                if (!landed)
                {
                    landed = true;
                }
            }
        }
        else
        {
            landed = false;

            // 
            if (!playerScript.playerInputs.umbrellaIsOpen)
            {
                FallMultiplier();
            }

        }
        if (IsLadder && playerScript.weapon == PlayerScript.Weapon.unarmed && !IsGrounded())
        {
            playerScript.playerInputs.umbrellaIsOpen = false;
            IsClimbing = true;
        }
    }

    private void FixedUpdate()
    {
        if (!playerScript.PlayerIsDead)
        {
            if (killBounce)
            {
                killBounce = false;
                isbouncingFromKill = true;
                playerScript.PlayerRigideBody.velocity = new Vector2(playerScript.PlayerRigideBody.velocity.x, 0f);
                playerScript.PlayerRigideBody.AddForce(Vector2.up * killBounceForce, ForceMode2D.Impulse);

            }
            if (!playerScript.playerInputs.SpellPressed && !isSliding && !playerScript.playerAnimation.isCrouching)
            {
                Flip();
            }
            switch (playerScript.weapon)
            {
                case PlayerScript.Weapon.axe:
                    isSliding = false;
                    playerScript.PlayerRigideBody.gravityScale = 1;
                    if (!playerScript.playerAnimation.throwingAxeAnimationµIsPlaying)
                    {
                        Run(1.4f);
                        playerScript.playerInputs.umbrellaIsOpen = false;
                    }
                    break;

                case PlayerScript.Weapon.spear:
                    isSliding = false;
                    playerScript.PlayerRigideBody.gravityScale = 1;
                    if (!playerScript.playerAnimation.throwingSpearAnimationµIsPlaying)
                    {
                        Run(0);
                        playerScript.playerInputs.umbrellaIsOpen = false;
                    }
                    break;

                case PlayerScript.Weapon.unarmed:
                    if (!playerScript.playerInputs.SpellPressed)
                    {
                        Run(0);
                    }
                    else
                    {
                        playerScript.PlayerRigideBody.velocity = Vector2.zero;
                    }
                    if (IsClimbing)
                    {
                        isSliding = false;
                        Climbing();
                    }
                    else
                    {
                        playerScript.PlayerRigideBody.gravityScale = 1;
                        //playerScript.playerInputs.joystick.SnapY = false;

                    }
                    break;
            }
            if (IsGrounded())
            {
                //Can Jump
                playerScript.playerInputs.hangTimeCounter = hangTime;

                //Sliding
                 Slide();
                if (isSliding)
                {
                    playerScript.PlayerRigideBody.drag = slideLinearDrag;
                }
                else if (playerScript.playerAnimation.isCrouching && playerScript.playerAnimation.PlayerCurrentAnimationState != "Run")
                {
                    playerScript.PlayerRigideBody.drag = groundLinearDrag;
                }
                else
                {
                    GroundLinearDrag();
                }

            }
            else if (playerScript.HoldingUmbrella && playerScript.playerInputs.umbrellaIsOpen)
            {
                playerScript.PlayerRigideBody.gravityScale = gravityScaleWhenUmbrellaIsOpen;
                AirControl(0);
                playerScript.PlayerRigideBody.drag = umbrellaLinearDrag;
                isSliding = false;
            }
            else if (!IsClimbing)
            {
                if (RopeHanging)
                {
                    playerScript.PlayerRigideBody.drag = 0.1f;
                    //if (!plzhangingBackward)
                    if (!playerScript.playerAnimation.HangingBackward)
                    {
                        AirControl(-2f);

                    }


                }
                else
                {
                    AirControl(0);
                    AirLinearDrag();
                }

                playerScript.playerInputs.hangTimeCounter -= Time.fixedDeltaTime;
                isSliding = false;


            }
            if (playerScript.playerInputs.isJumpPressed && !isSliding)
            {
                // is jump Pressed is activated in playerInput when the the key is clicked, hangtime > 0 and the remember jump pressed time > 0
                // hang time depends on isgrounded


                Jump();

                switch (playerScript.weapon)
                {
                    case PlayerScript.Weapon.unarmed:
                        ChangingDirDust.Play();
                        break;

                    case PlayerScript.Weapon.axe:
                        if (!playerScript.playerAnimation.throwingAxeAnimationµIsPlaying)
                        {
                            DropAxe();
                        }
                        break;

                    case PlayerScript.Weapon.spear:
                        ChangingDirDust.Play();
                        break;
                } 
            }


        }

    }


    private void Run(float substract)
    {
        if (!isSliding && playerScript.playerAnimation.PlayerCurrentAnimationState != "Crouch" && playerScript.playerAnimation.PlayerCurrentAnimationState != "ThrowAxe")
        {
            playerScript.PlayerRigideBody.AddForce(new Vector2(playerScript.playerInputs.horizontal, 0f) * PlayerAcceleration);
            if (Mathf.Abs(playerScript.PlayerRigideBody.velocity.x) > PlayerRunSpeed - substract)
            {
                playerScript.PlayerRigideBody.velocity = new Vector2(Mathf.Sign(playerScript.PlayerRigideBody.velocity.x)
                    * (PlayerRunSpeed - substract), playerScript.PlayerRigideBody.velocity.y);
            }
        }
        
    }

    private void AirControl(float substract)
    {

        playerScript.PlayerRigideBody.AddForce(new Vector2(playerScript.playerInputs.horizontal, 0f) * PlayerAirAcceleration);
        if (Mathf.Abs(playerScript.PlayerRigideBody.velocity.x) > PlayerAirMovementSpeed - substract)
        {
            playerScript.PlayerRigideBody.velocity = new Vector2(Mathf.Sign(playerScript.PlayerRigideBody.velocity.x)
                * (PlayerAirMovementSpeed - substract), playerScript.PlayerRigideBody.velocity.y);
        }
    }
    private void Slide()
    {
        if (playerScript.playerInputs.SlideRequested
    && Mathf.Abs(playerScript.PlayerRigideBody.velocity.x) >= PlayerRunSpeed
    && !isSliding
    && youCanSlideAgain && IsGrounded())
        {
            isSliding = true;
            playerScript.playerInputs.SlideRequested = false;
            youCanSlideAgain = false;
            playerScript.PlayerRigideBody.velocity = Vector2.zero;
            if (playerScript.playerController.Player_Locking_Right)
            {
                playerScript.PlayerRigideBody.AddForce(Vector2.right * slideForce);
            }
            else
            {
                playerScript.PlayerRigideBody.AddForce(Vector2.left * slideForce);
            }
            StartCoroutine(StopSlide());
            StartCoroutine(SlideAgain());
        }
        else
        {
            playerScript.playerInputs.SlideRequested = false;
        }
        //if (playerScript.playerInputs.Vertical >= 0)
        //{
        //    isSliding = false;

        //}

    }
    IEnumerator StopSlide()
    {
        yield return new WaitForSeconds(slideTime);
        isSliding = false;
    }    
    IEnumerator SlideAgain()
    {
        yield return new WaitForSeconds(timeToSlideAgainAfterSlide);
        youCanSlideAgain = true;
    }

    private void GroundLinearDrag()
    {
        if ((Mathf.Abs(playerScript.playerInputs.horizontal) < 0.4f || changingDir) && !playerScript.playerInputs.isJumpPressed )
        {
            playerScript.PlayerRigideBody.drag = groundLinearDrag;
        }
        else
        {
            playerScript.PlayerRigideBody.drag = 0f;
        }
    }
    private void LadderLinearDrag()
    {
        playerScript.PlayerRigideBody.drag = ladderLinearDrag;
    }

    private void AirLinearDrag()
    {
        playerScript.PlayerRigideBody.drag = airlinearDrag;
    }

    internal bool IsGrounded()
    {
        foreach (Transform groundcheck in groundCheck)
        {
            if (Physics2D.Linecast(playerLegs.position, groundcheck.position, whatisGround))
            {
                return true;

            }
        }
        return false;
    }
    internal bool IsObstacleInFrontOfHimSpear()
    {
        foreach (Transform obstacle in ObstacleCheck_Spear)
        {
            if (Physics2D.Linecast(transform.position, obstacle.position, whatIsObstacle))
            {
                return true;

            }
        }
        return false;
    }
    internal bool IsObstacleInFrontOfHimAxe()
    {
        foreach (Transform obstacle in ObstacleCheck_Axe)
        {
            if (Physics2D.Linecast(transform.position, obstacle.position, whatIsObstacle))
            {
                return true;

            }
        }
        return false;
    }

    internal Vector2 PlayerDirection()
    {
        if (Player_Locking_Right)
        {
            return Vector2.right;
        }
        return Vector2.left;
    }
    private void Jump()
    {
        playerScript.playerInputs.isJumpPressed = false;
        isSliding = false;


        playerScript.PlayerRigideBody.velocity = new Vector2(playerScript.PlayerRigideBody.velocity.x, 0f);
        playerScript.PlayerRigideBody.AddForce(Vector2.up * PlayerJumpForce, ForceMode2D.Impulse);

        playerScript.playerInputs.hangTimeCounter = 0;
        playerScript.playerInputs.jumpTimeCounter = 0;

        playerScript.playerInputs.isJumping = true;
        playerScript.playerInputs.jumpTimeCounter = jumpTime;
        AudioManager.instance.Play("Jump");

    }

    private void FallMultiplier()
    {
        if (playerScript.PlayerRigideBody.velocity.y < _minSpeedtoFall)
        {
            playerScript.PlayerRigideBody.velocity += (fallMultipier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
        }
        //!CrossPlatformInputManager.GetButton("Jump" && !Input.GetKey(KeyCode.Space) && !Input.GetButton("Fire1")))
        //else if (playerScript.PlayerRigideBody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        else if (playerScript.PlayerRigideBody.velocity.y > 0 && !CrossPlatformInputManager.GetButton("Jump") && !isbouncingFromKill)
        {
            playerScript.PlayerRigideBody.velocity += (lowJumpMultipier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
        }
    }

    private void Flip()
    {
        if (playerScript.playerInputs.horizontal < 0)
        {
            Player_Locking_Right = false;
            if (playerScript.playerAnimation.PlayerCurrentAnimationState != "Idle" && playerScript.playerAnimation.PlayerCurrentAnimationState != "Run")
            {
                transform.localScale = new Vector2(-playerScale.x, playerScale.y);
            }
        }
        else if (playerScript.playerInputs.horizontal > 0)
        {
            Player_Locking_Right = true;
            if (playerScript.playerAnimation.PlayerCurrentAnimationState != "Idle" && playerScript.playerAnimation.PlayerCurrentAnimationState != "Run")
            {
                transform.localScale = new Vector2(playerScale.x, playerScale.y);
            }

        }
    }









    private void Climbing()
    {
        playerScript.PlayerRigideBody.gravityScale = 0;
        //playerScript.playerInputs.joystick.SnapY = true;


        // climbing The ladder normally
        if (!topOfLadder)
        {
            playerScript.PlayerRigideBody.velocity = new Vector2(ladderClimbingSpeed * playerScript.playerInputs.horizontal, ladderClimbingSpeed * playerScript.playerInputs.Vertical);
            LadderLinearDrag();

        }
        else
        {
            if (playerScript.playerInputs.Vertical >= 0)
            {
                if (!IsGrounded())
                {
                    LadderLinearDrag();
                    playerScript.PlayerRigideBody.velocity = new Vector2(ladderClimbingSpeed * playerScript.playerInputs.horizontal, 0);
                }
                else
                {
                    playerScript.PlayerRigideBody.velocity = new Vector2(playerScript.PlayerRigideBody.velocity.x, 0);

                }

            }
            else
            {
                playerScript.PlayerRigideBody.velocity = new Vector2(ladderClimbingSpeed * playerScript.playerInputs.horizontal, ladderClimbingSpeed * playerScript.playerInputs.Vertical);

            }

        }
        //// if the player reach the top of the ladder
        //else
        //{
        //    // if the player is moving upword
        //    if (playerScript.playerInputs.Vertical > 0)
        //    {
        //        // the location of the plateform next to the ladder
        //        if (LadderToRight)
        //        {
        //            playerScript.PlayerRigideBody.velocity = new Vector2(ladderClimbingSpeed * playerScript.playerInputs.Vertical, 0);
        //        }
        //        else
        //        {
        //            playerScript.PlayerRigideBody.velocity = new Vector2(-ladderClimbingSpeed * playerScript.playerInputs.Vertical, 0);

        //        }
        //    }
        //    // if the player is moving downword
        //    else if (playerScript.playerInputs.Vertical < 0)
        //    {
        //        playerScript.PlayerRigideBody.velocity = new Vector2(playerScript.PlayerRigideBody.velocity.x, ladderClimbingSpeed * playerScript.playerInputs.Vertical);
        //    }

        //}
    }

    private void DropAxe()
    {
        if (!playerScript.playerController.axeThrown)
        {
            playerScript.weapon = PlayerScript.Weapon.unarmed;
            Instantiate(axePrefabDropped, axePos2.position, Quaternion.identity);
        }

    }

    private void DropSpear()
    {
        playerScript.weapon = PlayerScript.Weapon.unarmed;
        Instantiate(spearPrefabDrop, dropSpearPos.position, Quaternion.identity);
    }













}
