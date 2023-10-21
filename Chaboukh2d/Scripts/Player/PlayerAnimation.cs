using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] internal PlayerScript playerScript;
    internal string PlayerCurrentAnimationState;
    [SerializeField] Animator pAnim;

    [Header("Box Checking")]
    [SerializeField]
    Transform[] BoxChecks;
    [SerializeField] private LayerMask whatIsBox;

    float runTimer = 0;


    internal bool hitGroundAnimationHadBeenPlayed = true;
    internal bool stoppingAnimationHadBeenPlayed = true;
    internal bool openingUmbrellaAnimationHadBeenPlayed = true;
    internal bool closingUmbrellaAnimationHadBeenPlayed = true;
    internal bool throwingSpearAnimationµIsPlaying = false;
    internal bool throwingAxeAnimationµIsPlaying = false;
    internal bool landEntryPlayed = false;
    internal bool jumpingAnimationIsplaying = false;
    internal bool FlipAnimationPlayed = false;
    internal bool isCrouching = false;



    //
    const string PLAYER_IDLE = "Idle";
    const string PLAYER_CROUCH = "Crouch";
    const string PLAYER_SLIDE = "Slide";
    const string PLAYER_RUN = "Run";
    const string PLAYER_STOP = "Stop";
    const string PLAYER_JUMP = "Jump";
    const string PLAYER_JUMP2 = "Jump2";
    const string PLAYER_FLIP = "ChangingDirection";
    const string PLAYER_LAND_ENTRY = "LandEntry";
    const string PLAYER_LAND = "Land";
    const string PLAYER_HITGROUND = "HitGround";
    const string PLAYER_PUSH = "Push";
    const string PLAYER_CLIMB = "Climb";
    const string PLAYER_HANGING_ENTRY = "HangingEntry";
    //
    const string PLAYER_WALK_AXE = "WalkWithAxe";
    const string PLAYER_IDLE_AXE = "IdleWithAxe";
    const string PLAYER_THROW_AXE = "ThrowAxe";
    const string PLAYER_IDLE_SPEAR = "IdleWithSpear";
    const string PLAYER_WALK_SPEAR = "WalkWithSpear";
    const string PLAYER_JUMP_SPEAR = "JumpWithSpear";
    const string PLAYER_LAND_SPEAR_ENTRY = "LandWithSpearEntry";
    const string PLAYER_LAND_SPEAR = "LandWithSpear";
    const string PLAYER_HITGROUND_SPEAR = "HitGroundWithSpear";

    const string PLAYER_THROW_SPEAR = "ThrowSpear";

    const string PLAYER_MAGIC_SPELL = "MagicSpell";
    //
    const string OPEN_UMBRELLA = "OpenUmbrella";
    const string GLIDE_UMBRELLA = "GlideUmbrella";
    const string GLIDE_UMBRELLA_ON_FAN = "GlideUmbrellaOnFan";
    const string GLIDE_UMBRELLA_ON_FAN_ENTRY = "GlideUmbrellaOnFanEntry";
    const string CLOSE_UMBRELLA = "LandingWithClosedUmbrella";
    //
    const string PLAYER_DEATH_ROLLINGSPIKES = "RollingSpikesDeath";
    const string PLAYER_DEATH_SPIKES = "DeathFromSpikes";

    [SerializeField] float CrtiticalXVelocity;
    float stilnessTimer;
    [SerializeField] float timeToBeCalledStill;

    internal bool HangingForward => ((playerScript.playerController.Player_Locking_Right && playerScript.PlayerRigideBody.velocity.x > 0) || (!playerScript.playerController.Player_Locking_Right && playerScript.PlayerRigideBody.velocity.x < 0));
    internal bool HangingBackward => ((playerScript.playerController.Player_Locking_Right && playerScript.PlayerRigideBody.velocity.x < 0) || (!playerScript.playerController.Player_Locking_Right && playerScript.PlayerRigideBody.velocity.x > 0));
    internal enum PlayerState
    {
       idle,
       running,
       Stopping,
       jumping,
       landing,
       pushing,
       climbing,
       hitGround,
       umbrella,
       shootingMagic,
       slide
    }

    internal PlayerState playerState;



    // Start is called before the first frame update
    void Start()
    {
        playerState = PlayerState.idle;
    }

    void HangingAnimationsManagement()
    {
        ChangeAnimationState(PLAYER_HANGING_ENTRY);
        if (Mathf.Abs(playerScript.PlayerRigideBody.velocity.x) <= CrtiticalXVelocity)
        {
            stilnessTimer += Time.deltaTime;
            if (stilnessTimer > timeToBeCalledStill)
            {
                //new
                pAnim.SetBool("Forward", false);
                pAnim.SetBool("Still", true);

                ////old
                //if (playerScript.playerInputs.horizontal == 0)
                //{
                //    pAnim.SetBool("Forward", false);
                //    pAnim.SetBool("Still", true);
                //}
                //else if (playerScript.playerInputs.horizontal > 0)
                //{
                //    pAnim.SetBool("Forward", true);
                //    pAnim.SetBool("Still", false);
                //}
                //else
                //{
                //    pAnim.SetBool("Forward", false);
                //    pAnim.SetBool("Still", false);
                //}
            }
        }
        else
        {
            stilnessTimer = 0;
            if (HangingForward)
            {
                pAnim.SetBool("Forward", true);
                pAnim.SetBool("Still", false);

            }
            else if (HangingBackward)
            {
                pAnim.SetBool("Forward", false);
                pAnim.SetBool("Still", false);
            }
        }
    }

    void Update()
    {
        pAnim.SetFloat("horizontal", Mathf.Abs(playerScript.playerInputs.horizontal));

        if (playerScript.playerController.IsGrounded() && playerScript.playerInputs.Vertical < -0.7f && playerScript.weapon == PlayerScript.Weapon.unarmed)
        {
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }

        
        ResetingBooleans();
        if (!playerScript.PlayerIsDead)
        {
            if (playerScript.playerController.RopeHanging)
            {
                HangingAnimationsManagement();
                pAnim.SetBool("JumpingFromRope", false);
            }
            else
            {
                pAnim.SetBool("JumpingFromRope", true);
                switch (playerState)
                {

                    case PlayerState.idle:
                        stoppingAnimationHadBeenPlayed = true;

                        runTimer = 0;

                        //Transitions : 
                        if (playerScript.playerController.IsGrounded())
                        {

                                if (playerScript.playerInputs.SpellPressed)
                                {
                                    playerState = PlayerState.shootingMagic;
                                }
                                else if (playerScript.playerController.isSliding)
                                {
                                    playerState = PlayerState.slide;
                                }
                                else
                                {
                                    if (playerScript.playerInputs.horizontal != 0)
                                    {

                                        if (!isCrouching)
                                        {
                                            playerState = PlayerState.running;
                                        }
                                    }
                                    else if (playerScript.playerController.IsClimbing)
                                    {
                                        playerState = PlayerState.climbing;
                                    }
                                    else
                                    {
                                        if (playerScript.playerController.charachterFlipped && !playerScript.playerInputs.InteractPressed)
                                        {
                                            ChangeAnimationState(PLAYER_FLIP);
                                        }
                                        else
                                        {
                                            IdleCases();
                                        }
                                    }
                                }
                        }
                        else
                        {
                            if (playerScript.PlayerRigideBody.velocity.y < 0)
                            {
                                playerState = PlayerState.landing;

                            }
                            else
                            {
                                playerState = PlayerState.jumping;
                            }
                        }                       
                        break;

                    case PlayerState.slide:
                        if (playerScript.playerController.isSliding)
                        {
                            ChangeAnimationState(PLAYER_SLIDE);
                        }
                        else
                        {
                            playerState = PlayerState.idle;
                        }
                        break;

                    case PlayerState.shootingMagic:
                        ChangeAnimationState(PLAYER_MAGIC_SPELL);
                        if (!playerScript.playerInputs.SpellPressed)
                        {
                            playerState = PlayerState.idle;
                        }
                        break;

                    case PlayerState.running:
                        //running timer, how much time the Player had been running. To see if we want to play slide (stop) animation or not 
                        runTimer += Time.deltaTime;
                        //FlipAnimationPlayed = false;

                        //Transitions : 
                        if (playerScript.playerController.IsGrounded())
                        {

                            if (playerScript.playerInputs.SpellPressed)
                            {
                                playerState = PlayerState.shootingMagic;
                            }
                            else if (playerScript.playerController.isSliding)
                            {
                                playerState = PlayerState.slide;
                            }
                            else
                            {
                                if (playerScript.playerInputs.horizontal == 0)
                                {
                                    if (runTimer >= 0.2f)
                                    {
                                        stoppingAnimationHadBeenPlayed = false;
                                        playerState = PlayerState.Stopping;

                                    }
                                    else
                                    {
                                        playerState = PlayerState.idle;

                                    }
                                }
                                else if (IsPushing())
                                {
                                    playerState = PlayerState.pushing;
                                }
                                else
                                {
                                    if (playerScript.playerController.charachterFlipped && !playerScript.playerInputs.InteractPressed)
                                    {
                                        ChangeAnimationState(PLAYER_FLIP);
                                    }
                                    else
                                    {
                                        RuningCases();
                                    }

                                }
                            }
                        }
                        else
                        {

                            if (playerScript.PlayerRigideBody.velocity.y < 0)
                            {
                                playerState = PlayerState.landing;

                            }
                            else
                            {
                                playerState = PlayerState.jumping;
                            }
                        }
                        break;

                    case PlayerState.Stopping:
                        if (playerScript.playerController.IsGrounded())
                        {
                            if (playerScript.playerInputs.SpellPressed)
                            {
                                playerState = PlayerState.shootingMagic;
                            }
                            else
                            {
                                if (!playerScript.playerController.charachterFlipped && !stoppingAnimationHadBeenPlayed)
                                {
                                    ChangeAnimationState(PLAYER_STOP);
                                }
                                else
                                {
                                    playerState = PlayerState.idle;
                                }
                            }

                        }
                        else
                        {
                            playerState = PlayerState.idle;

                        }
                        break;

                    case PlayerState.umbrella:
                        if (!playerScript.playerController.IsGrounded() && !playerScript.playerController.RopeHanging)
                        {
                            if (playerScript.playerInputs.umbrellaIsOpen)
                            {
                                if (!openingUmbrellaAnimationHadBeenPlayed)
                                {
                                    ChangeAnimationState(OPEN_UMBRELLA);
                                }
                                else
                                {
                                    if (playerScript.PlayerRigideBody.velocity.y <= 0)
                                    {
                                        ChangeAnimationState(GLIDE_UMBRELLA);

                                    }
                                    else
                                    {
                                        ChangeAnimationState(GLIDE_UMBRELLA_ON_FAN_ENTRY);

                                    }
                                }
                            }
                            else
                            {
                                openingUmbrellaAnimationHadBeenPlayed = false;

                                if (!closingUmbrellaAnimationHadBeenPlayed)
                                {
                                    ChangeAnimationState(CLOSE_UMBRELLA);
                                }
                                else
                                {
                                    playerState = PlayerState.landing;
                                }

                            }


                        }
                        else
                        {
                            hitGroundAnimationHadBeenPlayed = false;
                            playerState = PlayerState.hitGround;
                        }
                        break;


                    case PlayerState.jumping:
                        if (!playerScript.playerController.IsGrounded())
                        {
                            if (!playerScript.playerController.IsClimbing)
                            {
                                if (playerScript.playerInputs.umbrellaIsOpen)
                                {
                                    playerState = PlayerState.umbrella;

                                }
                                else
                                {
                                    jumpingCases();
                                }
                            }
                            else
                            {
                                playerState = PlayerState.climbing;
                            }
                            if (playerScript.PlayerRigideBody.velocity.y <= 0)
                            {
                                playerState = PlayerState.landing;
                            }
                        }
                        else
                        {
                            playerState = PlayerState.idle;
                        }


                        break;

                    case PlayerState.landing:
                        openingUmbrellaAnimationHadBeenPlayed = false;

                        if (!playerScript.playerController.IsGrounded())
                        {
                            if (!playerScript.playerController.IsClimbing)
                            {
                                if (playerScript.playerInputs.umbrellaIsOpen)
                                {
                                    playerState = PlayerState.umbrella;


                                }
                                else
                                {
                                    landingCases();
                                }
                            }

                            else
                            {
                                playerState = PlayerState.climbing;

                            }
                            if (playerScript.PlayerRigideBody.velocity.y > 0)
                            {
                                playerState = PlayerState.jumping;
                            }

                        }
                        else
                        {
                            hitGroundAnimationHadBeenPlayed = false;
                            playerState = PlayerState.hitGround;
                        }
                        break;

                    case PlayerState.hitGround:
                        playerScript.playerInputs.InteractPressed = false;
                        throwingAxeAnimationµIsPlaying = false;
                        if (!hitGroundAnimationHadBeenPlayed)
                        {
                            if (playerScript.playerController.IsGrounded())
                            {
                                ChangeAnimationState(PLAYER_HITGROUND);

                            }
                            else
                            {
                                if (playerScript.PlayerRigideBody.velocity.y < 0)
                                {
                                    playerState = PlayerState.landing;

                                }
                                else
                                {
                                    playerState = PlayerState.jumping;
                                }
                            }
                        }
                        else
                        {
                            if (playerScript.playerInputs.horizontal == 0)
                            {
                                playerState = PlayerState.idle;

                            }
                            else
                            {
                                playerState = PlayerState.running;
                            }
                        }


                        break;

                    //        if (playerScript.PlayerRigideBody.velocity.y < 0)
                    //        {
                    //            playerState = PlayerState.landing;

                    //        }
                    //        else
                    //        {
                    //            playerState = PlayerState.jumping;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if (playerScript.playerInputs.horizontal == 0)
                    //    {
                    //        playerState = PlayerState.idle;

                    //    }
                    //    else
                    //    {
                    //        playerState = PlayerState.running;
                    //    }
                    //}





                    case PlayerState.pushing:
                        //if (!AudioManager.instance.sounds[6].source.isPlaying)
                        //{
                        //    AudioManager.instance.sounds[6].source.Play();
                        //}
                        ChangeAnimationState(PLAYER_PUSH);

                        if (!IsPushing() || !playerScript.playerController.IsGrounded() || (playerScript.playerInputs.horizontal == 0))
                        {
                            //pAnim.speed = 1;
                            //AudioManager.instance.sounds[6].source.Stop();
                            playerState = PlayerState.idle;

                        }

                        //if (playerScript.playerInputs.horizontal == 0)
                        //{
                        //    AudioManager.instance.sounds[6].source.Stop();
                        //    pAnim.speed = 0;
                        //}
                        //else
                        //{
                        //    pAnim.speed = 1;
                        //}
                        break;

                    case PlayerState.climbing:
                        ChangeAnimationState(PLAYER_CLIMB);
                        playerScript.playerInputs.InteractPressed = false;
                        if (playerScript.playerController.IsClimbing)
                        {
                            if (playerScript.playerInputs.Vertical == 0)
                            {
                                pAnim.speed = 0;
                            }
                            else
                            {
                                pAnim.speed = 1;


                            }
                        }
                        else
                        {
                            pAnim.speed = 1;
                            playerState = PlayerState.idle;

                        }
                        break;
                }
            }
        }
        else
        {
            switch (playerScript.death)
            {
                case PlayerScript.DeathType.spikes:
                    ChangeAnimationState(PLAYER_DEATH_SPIKES);
                    break;
                case PlayerScript.DeathType.rollingSpikes:
                    if (!playerScript.playerController.IsGrounded())
                    {
                        ChangeAnimationState(PLAYER_DEATH_ROLLINGSPIKES);
                    }
                    else
                    {
                        playerScript.death = PlayerScript.DeathType.spikes;
                    }
                    break;
            }
        }
    }

    private void ChangeAnimationState(string newState)
    {
        if (PlayerCurrentAnimationState == newState)
            return;
        pAnim.Play(newState);
        PlayerCurrentAnimationState = newState;
    }

    internal bool IsPushing()
    {
        foreach (Transform BoxCheck in BoxChecks)
        {
            if (Physics2D.Linecast(transform.position, BoxCheck.position, whatIsBox))
            {
                return true;
            }
        }
        return false;
    }


    private void IdleCases()
    {
        switch (playerScript.weapon)
        {
            case PlayerScript.Weapon.axe:
                if (playerScript.playerInputs.InteractPressed)
                {
                    ChangeAnimationState(PLAYER_THROW_AXE);

                }
                else
                {
                    ChangeAnimationState(PLAYER_IDLE_AXE);
                    throwingAxeAnimationµIsPlaying = false;
                }
                break;

            case PlayerScript.Weapon.spear:

                if (playerScript.playerInputs.InteractPressed)
                {
                    ChangeAnimationState(PLAYER_THROW_SPEAR);
                }
                else
                {
                    ChangeAnimationState(PLAYER_IDLE_SPEAR);
                    throwingSpearAnimationµIsPlaying = false;
                }
                break;


            case PlayerScript.Weapon.unarmed:

                if (!playerScript.playerInputs.InteractPressed)
                {
                    throwingSpearAnimationµIsPlaying = false;
                    throwingAxeAnimationµIsPlaying = false;
                    playerScript.playerInputs.InteractPressed = false;

                    if (playerScript.playerInputs.Vertical >= -0.7f)
                    {
                        ChangeAnimationState(PLAYER_IDLE);

                    }
                    else
                    {
                        ChangeAnimationState(PLAYER_CROUCH);
                    }



                }
                break;

        }
    }
    private void RuningCases()
    {

        switch (playerScript.weapon)
        {
            case PlayerScript.Weapon.axe:
                stoppingAnimationHadBeenPlayed = true;
                if (playerScript.playerInputs.InteractPressed)
                {
                    ChangeAnimationState(PLAYER_THROW_AXE);

                }
                else if (!throwingAxeAnimationµIsPlaying)
                {
                    ChangeAnimationState(PLAYER_WALK_AXE);
                    throwingAxeAnimationµIsPlaying = false;


                }
                break;

            case PlayerScript.Weapon.spear:

                stoppingAnimationHadBeenPlayed = true;
                if (playerScript.playerInputs.InteractPressed)
                {
                    ChangeAnimationState(PLAYER_THROW_SPEAR);
                }
                else
                {
                    ChangeAnimationState(PLAYER_WALK_SPEAR);
                    throwingSpearAnimationµIsPlaying = false;

                }
                break;


            case PlayerScript.Weapon.unarmed:
                if (!playerScript.playerInputs.InteractPressed)
                {
                    throwingSpearAnimationµIsPlaying = false;
                    throwingAxeAnimationµIsPlaying = false;
                    playerScript.playerInputs.InteractPressed = false;

                    
                    ChangeAnimationState(PLAYER_RUN);

                }
                break;
        }
    }
    private void jumpingCases()
    {
        switch (playerScript.weapon)
        {
            case PlayerScript.Weapon.axe:
                ChangeAnimationState(PLAYER_IDLE_AXE);
                break;
            case PlayerScript.Weapon.spear:
                if (!jumpingAnimationIsplaying)
                {
                    ChangeAnimationState(PLAYER_JUMP_SPEAR);
                    jumpingAnimationIsplaying = true;
                }

                break;
            case PlayerScript.Weapon.unarmed:
                if (!jumpingAnimationIsplaying)
                {
                    if (playerScript.playerInputs.horizontal == 0)
                    {
                        ChangeAnimationState(PLAYER_JUMP);
                        jumpingAnimationIsplaying = true;

                    }
                    else
                    {
                        ChangeAnimationState(PLAYER_JUMP2);
                        jumpingAnimationIsplaying = true;


                    }
                }

                break;
        }
    }

    private void landingCases()
    {
        switch (playerScript.weapon)
        {
            case PlayerScript.Weapon.axe:
                if (playerScript.playerInputs.InteractPressed)
                {
                    ChangeAnimationState(PLAYER_THROW_AXE);

                }
                else
                {
                    ChangeAnimationState(PLAYER_WALK_AXE);
                    throwingAxeAnimationµIsPlaying = false;
                    playerScript.playerInputs.InteractPressed = false;



                }
                break;
            case PlayerScript.Weapon.spear:
                if (playerScript.playerInputs.InteractPressed)
                {
                    ChangeAnimationState(PLAYER_THROW_SPEAR);
                }
                else
                {
                    ChangeAnimationState(PLAYER_LAND_SPEAR_ENTRY);
                    throwingSpearAnimationµIsPlaying = false;
                    playerScript.playerInputs.InteractPressed = false;


                }

                break;
            case PlayerScript.Weapon.unarmed:
                playerScript.playerInputs.InteractPressed = false;
                if (!landEntryPlayed)
                {
                    ChangeAnimationState(PLAYER_LAND_ENTRY);
                }
                //ChangeAnimationState(PLAYER_LAND);
                throwingSpearAnimationµIsPlaying = false;
                throwingAxeAnimationµIsPlaying = false;
                playerScript.playerInputs.InteractPressed = false;
                break;
        }
    }
    private void HitGroundCases()
    {
        switch (playerScript.weapon)
        {
            case PlayerScript.Weapon.axe:
                ChangeAnimationState(PLAYER_HITGROUND);
                break;
            case PlayerScript.Weapon.spear:
                ChangeAnimationState(PLAYER_HITGROUND_SPEAR);
                break;
            case PlayerScript.Weapon.unarmed:
                ChangeAnimationState(PLAYER_HITGROUND);
                break;
            default:
                break;
        }
    }

    private void ResetingBooleans()
    {
        if (playerScript.playerController.IsGrounded() || !playerScript.playerInputs.umbrellaIsOpen)
        {
            openingUmbrellaAnimationHadBeenPlayed = false;
            landEntryPlayed = false;
        }
        if (playerScript.playerInputs.umbrellaIsOpen)
        {
            closingUmbrellaAnimationHadBeenPlayed = false;
        }
        if (playerScript.playerController.changingDir || playerScript.playerInputs.InteractPressed)
        {
            stoppingAnimationHadBeenPlayed = true;
        }
        if (playerState != PlayerState.jumping)
        {
            jumpingAnimationIsplaying = false;
        }
        //if (playerState != PlayerState.changeDirection)
        //{
        //    FlipAnimationPlayed = false;
        //}
        if (playerScript.playerController.RopeHanging)
        {
            closingUmbrellaAnimationHadBeenPlayed = true;
        }
        //if (!playerScript.playerController.IsGrounded())
        //{
        //    playerScript.playerController.charachterFlipped = false;

        //}
    }

    private void FlipAnimationPlayedToTrue()
    {
        playerScript.playerController.charachterFlipped = false;
        playerScript.playerAnimation.FlipAnimationPlayed = true;
    }
}
