using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisions : MonoBehaviour
{
    
    internal PlayerScript playerScript;
    [SerializeField] internal CapsuleCollider2D playerCollider;
    [SerializeField] internal CapsuleCollider2D playerSlidingCollider;

    [SerializeField] HingeJoint2D RopeHing;
    Rigidbody2D hangingRope;
    GameObject ropeThatWeJustDetachedFrom;

    [SerializeField] GameObject bottomSpearPrefab;
    [SerializeField] Transform bottomSpearstartpos;
    [SerializeField] float xJumpForceRope;
    [SerializeField] float yJumpForceRope;
    [SerializeField] float firstPushForce;
    bool firstPushExecuted;

    //Platform
    private bool isOnPlateform;
    bool isFallingFromThePlatform;

    private void Start()
    {
        playerScript = PlayerScript.Instance;
    }
    ////        if (playerScript.playerController.isSliding)
    //    {
    //        playerCollider.enabled = false;
    //        playerSlidingCollider.enabled = true;
    //    }

    IEnumerator fallingFromPlateform()
    {
        playerScript.playerInputs.RopeJumpRequested = false;
        playerScript.playerInputs.FallFromPlatform = false;
        playerCollider.enabled = false;
        playerSlidingCollider.enabled = false;

        isFallingFromThePlatform = true;

        yield return new WaitForSeconds(0.3f);
        
        isFallingFromThePlatform = false;

    }
    void JumpingFromRopes()
    {
        firstPushExecuted = false;
        playerScript.playerController.RopeHanging = false;
        playerScript.PlayerRigideBody.velocity = Vector2.zero;
        playerScript.PlayerRigideBody.AddRelativeForce(new Vector2(playerScript.playerInputs.horizontal * xJumpForceRope, yJumpForceRope));
        RopeHing.enabled = false;

    }
    void FirstPush()
    {
        playerScript.PlayerRigideBody.velocity = Vector2.zero;
        playerScript.PlayerRigideBody.AddForce(new Vector2 (playerScript.playerInputs.horizontal * firstPushForce, 0f));

        firstPushExecuted = true;

    }
    void Attach()
    {
        RopeHing.enabled = true;
        RopeHing.connectedBody = hangingRope;
        if (!firstPushExecuted)
        {
            FirstPush();
        }
    }
    private void FixedUpdate()
    {
        // empty the rope that we can't connect with
        if (playerScript.playerController.IsGrounded())
        {
            ropeThatWeJustDetachedFrom = null;
        }
        //activate the hinge  and attach
        if (playerScript.playerController.RopeHanging)
        {
            Attach();
            if (playerScript.playerInputs.RopeJumpRequested)
            {
                JumpingFromRopes();
            }

        }
        else
        {
            if (playerScript.playerInputs.RopeJumpRequested)
            {
                playerScript.playerInputs.RopeJumpRequested = false;
            }
        }

        //Colliders Management
        if (isOnPlateform)
        {
            if (playerScript.playerInputs.FallFromPlatform)
            {
                playerScript.PlayerRigideBody.AddForce(Vector2.down * 4, ForceMode2D.Impulse);

                StartCoroutine(fallingFromPlateform());
            }
            else
            {
                if (!isFallingFromThePlatform)
                {
                    if (playerScript.playerController.isSliding || playerScript.playerAnimation.isCrouching)
                    {
                        playerCollider.enabled = false;
                        playerSlidingCollider.enabled = true;
                    }
                    else
                    {
                        playerCollider.enabled = true;
                        playerSlidingCollider.enabled = false;
                    }
                }

            }
        }
        else
        {

            playerScript.playerInputs.FallFromPlatform = false;
            if (!isFallingFromThePlatform)
            {
                if (playerScript.playerController.isSliding || playerScript.playerAnimation.isCrouching)
                {
                    playerCollider.enabled = false;
                    playerSlidingCollider.enabled = true;
                }
                else
                {
                    playerCollider.enabled = true;
                    playerSlidingCollider.enabled = false;
                }
            }


        }
    }

    private void Update()
    {
        if (playerScript.playerController.killBounce)
        {
            ropeThatWeJustDetachedFrom = null;
        }
    }
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Lizzard"))
        {
            playerScript.PlayerIsDead = true;
            playerScript.death = PlayerScript.DeathType.rollingSpikes;
            AudioManager.instance.Play("Stab");
        }
        if (collision.gameObject.CompareTag("Plateform"))
        {
            isOnPlateform = true;
            
        }
        if (collision.gameObject.CompareTag("RollingSpikes") || collision.gameObject.CompareTag("EnemyWeapon"))
        {
            playerScript.PlayerIsDead = true;
            playerScript.death = PlayerScript.DeathType.rollingSpikes;
            AudioManager.instance.Play("Stab");
        }
        if (collision.gameObject.CompareTag("Spikes"))
        {
            playerScript.PlayerIsDead = true;
            playerScript.death = PlayerScript.DeathType.spikes;
            AudioManager.instance.Play("Stab");
        }
        if (collision.gameObject.CompareTag("Key"))
        {
            playerScript.GotTheKey = true;
            Destroy(collision.gameObject);
            AudioManager.instance.Play("PickUpKey");
            
        }
        // Pick up the spear if the player is not on the ladder or holding the axe
        if (playerScript.weapon == PlayerScript.Weapon.unarmed)
        {
            if (collision.gameObject.CompareTag("SpearToCollect") && !playerScript.playerController.IsClimbing)
            {
                playerScript.playerInputs.InteractPressed = false;
                playerScript.weapon = PlayerScript.Weapon.spear;
                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Rope"))
        {
            Physics2D.IgnoreCollision(collision.collider, playerCollider);
        }


    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Plateform"))
        {
            isOnPlateform = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("HangingRope"))
        {
            if (!playerScript.playerController.RopeHanging && (ropeThatWeJustDetachedFrom == null || collision.gameObject != ropeThatWeJustDetachedFrom))
            {
                playerScript.playerController.RopeHanging = true;
                hangingRope = collision.gameObject.GetComponent<Rigidbody2D>();
                ropeThatWeJustDetachedFrom = collision.gameObject;
            }
        }
        if (collision.gameObject.CompareTag("TopOfLadder"))
        {
            if (playerScript.playerController.IsClimbing)
            {
                playerScript.playerController.topOfLadder = true;

            }
            //playerScript.playerController.LadderToRight = collision.GetComponent<TopOfTheLadder>().toRight;
        }

            if (collision.gameObject.CompareTag("RollingSpikes") )
        {
            playerScript.PlayerIsDead = true;
            playerScript.death = PlayerScript.DeathType.rollingSpikes;
            AudioManager.instance.Play("Stab");
        }

        if (PowerGenerator.powerGeneratorInstance != null)
        {
            if (collision.CompareTag("Fans") && PowerGenerator.powerGeneratorInstance.PowerActivated)
            {
                playerScript.PlayerIsDead = true;
                playerScript.death = PlayerScript.DeathType.spikes;
                AudioManager.instance.Play("Stab");

            }
        }

        if (collision.CompareTag("Interactable"))
        {
            playerScript.playerController.DontThrow = true;
        }
        if (collision.CompareTag("Ladder"))
        {
            playerScript.playerController.IsLadder = true;


        }

        if (collision.CompareTag("Umbrella") && !playerScript.playerController.IsClimbing)
        {
            playerScript.HoldingUmbrella = true;
            Destroy(collision.gameObject);
        }
        

        
        if (playerScript.weapon == PlayerScript.Weapon.unarmed)
        {
            if (!playerScript.playerController.IsClimbing)
            {
                // Pick up the spear if the player is not on the ladder or holding the axe
                if (collision.CompareTag("SpearToCollect"))
                {
                    playerScript.playerInputs.InteractPressed = false;
                    playerScript.weapon = PlayerScript.Weapon.spear;
                    Destroy(collision.gameObject);
                    AudioManager.instance.Play("CollectWeapon");
                }

                // Pick up the axe if the player is not on the ladder or holding the spear
                if (collision.CompareTag("Axe"))
                {
                    playerScript.playerInputs.InteractPressed = false;
                    playerScript.weapon = PlayerScript.Weapon.axe;
                    Destroy(collision.gameObject);
                    AudioManager.instance.Play("CollectWeapon");
                    playerScript.playerController.axeThrown = false;
                }
            }

        }

        //Door And Key
        if (collision.gameObject.CompareTag("Key"))
        {
            playerScript.GotTheKey = true;
            Destroy(collision.gameObject);
            //FindObjectOfType<AudioManager>().Play("PickUpKey");
            AudioManager.instance.Play("PickUpKey");
        }
        if (collision.CompareTag("Door") && playerScript.GotTheKey)
        {
            playerScript.GotTheKey = false;
            playerScript.OpenedTheDoor = true;
            //FindObjectOfType<AudioManager>().Play("UnlockMainDoor");
            AudioManager.instance.Play("UnlockMainDoor");
        }

        // if the player is not holding the spear or holding it but not facing the enemy
        if (playerScript.weapon != PlayerScript.Weapon.spear || (playerScript.weapon == PlayerScript.Weapon.spear && !facingEnemy(collision.transform)))
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                playerScript.PlayerIsDead = true;
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            playerScript.weapon = PlayerScript.Weapon.unarmed;
            Instantiate(bottomSpearPrefab, bottomSpearstartpos.position, Quaternion.identity);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("TopOfLadder"))
        {
            playerScript.playerController.topOfLadder = false;
        }

        if (collision.CompareTag("Ladder"))
        {
            playerScript.playerController.IsLadder = false;
            playerScript.playerController.IsClimbing = false;

        }
        if (collision.CompareTag("Interactable"))
        {
            playerScript.playerController.DontThrow = false;
        }

    }

    /// <summary>
    /// Return true if the player is facing the enemy
    /// </summary>
    /// <param name="Enemy"></param>
    /// <returns></returns>
    bool facingEnemy(Transform Enemy)
    {
        if ((transform.position.x - Enemy.position.x < 0 && playerScript.playerController.Player_Locking_Right) || 
            (transform.position.x - Enemy.position.x > 0 && !playerScript.playerController.Player_Locking_Right))
        {
            return true;
        }
        return false;
    }
}
