using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class BigTeddy : MonoBehaviour
{
    [Header("Connection to other Script")]
    [SerializeField] DontCrossLimit DontCrossLimitScript;
    private GameObject player;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;

    [Header("PlayerDetection")]
    [SerializeField] private float agroDistanceX;
    [SerializeField] private float agroDistanceY;

    [Header("Direction")]
    [SerializeField] bool _facingRight;
    [SerializeField] bool changeDirectionAfterReturnToStartPos;

    private Rigidbody2D rb;
    private Animator an;
    Vector2 startpos;

    bool HaveSeenPlayer;
    bool hasChangedDirection;
    public bool bigTeddyIsDead;
    bool isKillingPlayer;

    [Header("Ground Variables")]
    [SerializeField] private LayerMask whatisGround;
    [SerializeField] Transform[] groundCheck;
    [SerializeField] float groundLinearDrag;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] Transform[] visionCheck;


    [Header("Colliders")]
    [SerializeField] Collider2D triggerCollider;
    [SerializeField] Collider2D normalCollider;
    [SerializeField] Collider2D UponHeadCollider;

    [Header("Particles")]
    [SerializeField] ParticleSystem PlayerBlood;
    [SerializeField] ParticleSystem BigTeddyBlood;
    [SerializeField] UnityEngine.Rendering.Universal.Light2D EnemyLight;
    float lightStep = 0.3f;

    bool spikeDeath;
    bool spearDeath;

    bool returningBack;
    private bool isInCollisionWithJumpTrigger;
    float PlayerCollisionTimer;
    [SerializeField] float PlayerCollisionTime;

    bool growingPlayedOnce;
    bool landed;

    bool collidingWithWall = false;
    Sound canSeePlayerSound;
    Sound canSeePlayerSound0;
    Sound landingSound;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        player = PlayerScript.Instance.gameObject;
        startpos = transform.position;
        HaveSeenPlayer = false;
        bigTeddyIsDead = false;
        hasChangedDirection = false;
        isKillingPlayer = false;
        returningBack = false;
        landed = true;
        canSeePlayerSound  = Array.Find(AudioManager.instance.sounds, item => item.name == "CanSee1");
        canSeePlayerSound0 = AudioManager.instance.sounds[16];

        landingSound = Array.Find(AudioManager.instance.sounds, item => item.name == "BigTeddyLand");


    }

    private void FixedUpdate()
    {
        
        

        if (player != null)
        {
            if (!bigTeddyIsDead && !isKillingPlayer)
            {
                if (IsGrounded() || (!IsGrounded() && isInCollisionWithJumpTrigger))
                {
                    rb.drag = groundLinearDrag;
                    if (CanSeePlayer() && !DontCrossLimitScript.hasCrossLimit && !returningBack)
                    {

                        EnemyLight.intensity = 2;
                        // There is a better way to do it, just save the starting value of _isfacingRight bool and Changedirection if it's not the same.
                        hasChangedDirection = false;
                        
                        LookAtTarget();
                        if (_facingRight)
                        {
                            Move(1);

                        }
                        else
                        {
                            Move(-1);
                        }
                    }
                    else
                    {
                        TurnTheLightOff();
                        if (!IscloseToStartPos())
                        {
                            returningBack = true;

                            LookAtStartingPosition();
                            if (_facingRight)
                            {
                                Move(1);

                            }
                            else
                            {
                                Move(-1);
                            }

                        }
                        else
                        {
                            returningBack = false;
                            // There is a better way to do it, just save the starting value of _isfacingRight bool and Changedirection if it's not the same.
                            if (changeDirectionAfterReturnToStartPos && HaveSeenPlayer && !hasChangedDirection)
                            {
                                ChangeDirection();
                                hasChangedDirection = true;
                                TurnTheLightOff();
                            }
                        }
                    }
                }
                else if (!IsGrounded())
                {
                    rb.drag = 1;
                }
            }
        }
    }

    private void Update()
    {
        if (IsGrounded())
        {
            if (!landed)
                {
                landed = true;
                if (!landingSound.source.isPlaying)
                {
                    AudioManager.instance.Play("BigTeddyLand");
                }
                }
        }
        else
        {
            landed = false;
        }

        if (Mathf.Abs(transform.position.y - startpos.y) > 0.5f || collidingWithWall)
        {
            startpos = transform.position;
            if (collidingWithWall)
            {
                collidingWithWall = false;
            }

        }

        if (player != null)
        {
            if (!bigTeddyIsDead && !isKillingPlayer)
            {
                if (playerIsStandingOnBigTeddyHead())
                {
                    PlayerCollisionTimer += Time.deltaTime;
                    if (PlayerCollisionTimer >= PlayerCollisionTime)
                    {
                        UponHeadCollider.enabled = true;
                    }
                }
                else
                {
                    PlayerCollisionTimer = 0;
                }
                if (IsGrounded())
                {
                    if (CanSeePlayer() && !DontCrossLimitScript.hasCrossLimit && !returningBack && !collidingWithWall && rb.velocity.x >=0.1f)
                    {
                        if (!canSeePlayerSound.source.isPlaying)
                        {
                            AudioManager.instance.Play("CanSee1");
                        }

                        if (!canSeePlayerSound0.source.isPlaying && !growingPlayedOnce)
                        {
                            AudioManager.instance.Play("CanSeeThePlayer");
                            growingPlayedOnce = true;
                        }
                        an.Play("Walk");
                        // There is a better way to do it, just save the starting value of _isfacingRight bool and Changedirection if it's not the same.
                    }
                    else
                    {
                        if (!IscloseToStartPos())
                        {
                            an.Play("WalkNormal");
                        }
                        else
                        {

                            an.Play("Idle");
                        }
                    }
                }
                // if not grounded
                else
                {
                    an.Play("Falling");
                }
            }
            else if (bigTeddyIsDead)
            {
                TurnTheLightOff();
                if (spikeDeath)
                {
                    an.Play("Death");
                    rb.velocity = Vector2.zero;
                    rb.isKinematic = true;

                }
                else if (spearDeath)
                {
                    an.Play("DeathWithSpear");
                }
                triggerCollider.enabled = false;
                if (!spikeDeath)
                {
                    normalCollider.enabled = false;
                    rb.drag = 1;
                }


            }
            else if (isKillingPlayer)
            {
                an.Play("KillingPlayer");
                if (player.gameObject != null)
                {
                    Destroy(player.gameObject);
                }
            }
        }
    }


    private bool CanSeePlayer()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < agroDistanceX && Mathf.Abs(player.transform.position.y - transform.position.y) < agroDistanceY && (player.transform.position.y - transform.position.y) >0)
        {
            HaveSeenPlayer = true;
            return true;
        }
        else return false;
    }
    private bool playerIsStandingOnBigTeddyHead()
    {
        foreach (Transform vision in visionCheck)
        {
            if (Physics2D.Linecast(transform.position, vision.position, playerLayer))
            {
                return true;
            }
        }
        return false;
    }

    private bool IscloseToStartPos()
    {
        if (Mathf.Abs(transform.position.x - startpos.x) <= 0.2f)
        {
            return true;
        }
        else return false;
    }

    private void Move(float direction)
    {

        rb.AddForce(new Vector2(direction, 0f) * acceleration);
        if (Mathf.Abs(rb.velocity.x) > speed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x)
                * (speed), rb.velocity.y);
        }
        //if (collidedWithJumpTrigger)
        //{
        //    EnemyJump(direction);
        //}
        if (isInCollisionWithJumpTrigger)
        {
            EnemyJump(direction);
        }

    }

    private void LookAtTarget()
    {
        if (player != null)
        {
            float DirX = player.transform.position.x - transform.position.x;

            if (DirX > 0 && !_facingRight || DirX < 0 && _facingRight)
            {
                ChangeDirection();
            }
        }
    }
    private void ChangeDirection()
    {
        _facingRight = !_facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y * 1, transform.localScale.z * 1);
    }

    private void LookAtStartingPosition()
    {
        if (player != null)
        {
            float DirX = startpos.x - transform.position.x;

            if (DirX > 0 && !_facingRight || DirX < 0 && _facingRight)
            {
                ChangeDirection();
            }
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Spikes"))
        {
            bigTeddyIsDead = true;
            spikeDeath = true;
            AudioManager.instance.Play("Stab");
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            collidingWithWall = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            collidingWithWall = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("JumpTrigger"))
        {
            isInCollisionWithJumpTrigger = true;
        }
        if (collision.CompareTag("Player"))
        {
            isKillingPlayer = true;

        }
        if (collision.gameObject.CompareTag("Spear"))
        {
            bigTeddyIsDead = true;
            spearDeath = true;
            StartCoroutine(TimeScaleEffect(0.05f));
            
            AudioManager.instance.Play("Stab");
        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("JumpTrigger"))
        {
            isInCollisionWithJumpTrigger = false;
        }
    }

    bool IsGrounded()
    {
        foreach (Transform groundcheck in groundCheck)
        {
            if (Physics2D.Linecast(transform.position, groundcheck.position, whatisGround))
            {
                return true;

            }
        }
        return false;
    }

    void EnemyJump(float direction)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up *10, ForceMode2D.Impulse);
        rb.AddForce(Vector2.right * 20 * direction, ForceMode2D.Impulse);

    }
    public void PlayPlayerBloodExplosion()
    {
        PlayerBlood.Play();
    }
    public void PlayBigTeddyBloodExplosion()
    {
        BigTeddyBlood.Play();
    }

    void TurnTheLightOff()
    {
        if (EnemyLight.intensity > 0.1f)
        {
            EnemyLight.intensity -= lightStep * Time.fixedDeltaTime;
        }
        
    }

    IEnumerator TimeScaleEffect(float time)
    {
        Time.timeScale = 0.2f;
        CinemachineShake.CameraInstance.ShakeCamera(0.8f, 0.8f, 4f);
        yield return new WaitForSeconds(time);
        Time.timeScale = 1;
    }

}
