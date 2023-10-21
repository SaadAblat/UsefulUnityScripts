using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;

public class EnemyIA : MonoBehaviour
{
    float timer;
    [SerializeField] float timeToPerformARangeAttackAgain;
    [SerializeField] float timeToPerformAMeleettackAgain;

    private IEnemyInterface enemyInterface;
    [SerializeField] private MonoBehaviour Enemy;

    [SerializeField] internal Rigidbody2D EnemyRigideBody;


    public enum EnemyState
    {
        Idle,
        Chasing,
        RangeAttack,
        MeleAttack,
        ReturningToStartPosition,
        Death
    }
    internal EnemyState state;



    [Header("Connection to other Script")]
    private GameObject player;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;

    [Header("PlayerDetection")]
    [SerializeField] private float agroDistanceX;
    [SerializeField] private float agroDistanceY;
    [SerializeField] private float RangeDistanceX;
    [SerializeField] private float RangeDistanceY;
    [SerializeField] private float MeleeDistanceX;
    [SerializeField] private float MeleeDistanceY;
    [SerializeField] private float targetMinHeight;

    [Header("Direction")]
    [SerializeField] internal bool _facingRight;

    private Rigidbody2D rb;
    Vector2 startpos;

    [Header("Ground Variables")]
    [SerializeField] private LayerMask whatisGround;
    [SerializeField] Transform[] groundCheck;
    [SerializeField] Transform groundFrontCheckStart1;
    [SerializeField] Transform groundFrontCheckStart2;
    [SerializeField] Transform groundFrontCheckEnd1;
    [SerializeField] Transform groundFrontCheckEnd2;
    [SerializeField] float groundLinearDrag;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] Transform[] visionCheck;

    private bool collidedWithObstacle;
    private bool collidingWithObstacle;

    [SerializeField] internal bool haveRangeAttack;
    [SerializeField] internal bool haveMeleeAttack;

    internal bool ExecutingRangeAttack;
    internal bool ExecutingMeleeAttack;

    private void Start()
    {
        enemyInterface = Enemy.GetComponent<IEnemyInterface>();
        timer = 0;
        state = EnemyState.Idle;
        rb = GetComponent<Rigidbody2D>();
        player = PlayerScript.Instance.gameObject;
        startpos = transform.position;
        
    }
    private void Update()
    {
        if (collidedWithObstacle)
        {
            //startpos = transform.position;
            collidedWithObstacle = false;
        }

    }
    private void FixedUpdate()
    {
        if (player != null)
        {
            switch (state)
            {

                case EnemyState.Idle:
                    if (CanSeePlayer())
                    {
                        state = EnemyState.Chasing;
                    }
                    break;


                case EnemyState.Chasing:

                        // If There is ground in front
                        if (IsGroundInFront() && !collidingWithObstacle)
                        {
                            LookAtTarget();

                        // if the player is in agro range and there is no obstacles in front of the enemy
                        if (!collidingWithObstacle && CanSeePlayer())
                        {

                            // change to range state if the player is in range and the enemy have a range attack
                            if (playerInRange() && haveRangeAttack)
                            {
                                state = EnemyState.RangeAttack;
                            }

                            // otherwise follow the player as long as he is in the enemy vision range
                            else
                            {
                                if (!collidingWithObstacle)
                                {
                                    Move(_facingRight);
                                    if (haveMeleeAttack && playerInMelee())
                                    {
                                        state = EnemyState.MeleAttack;
                                    }
                                }
                                else
                                {
                                    state = EnemyState.ReturningToStartPosition;
                                }



                            }
                        }
                            // if the player is not in agro range or there is an obstacke in front of him return to the start position
                            else
                            {
                                state = EnemyState.ReturningToStartPosition;

                            }

                        }
                        // else if there is no ground in front of the enemy
                        else
                        {
                            // when enemy have a range attack, if the player is in range, and there is no obstacle in front of the enemy : change to range Attack
                            if (playerInRange() && haveRangeAttack && !collidingWithObstacle)
                            {
                                state = EnemyState.RangeAttack;
                            }
                            // otherwise return back
                            else
                            {
                                state = EnemyState.ReturningToStartPosition;
                            }
                        }
                   

                    break;


                case EnemyState.RangeAttack:

                    if (!haveRangeAttack)
                    {
                        state = EnemyState.Chasing;
                    }
                    // if not already executing a range attack 
                    if (!ExecutingRangeAttack)
                    {
                        // if there is ground in front
                        if (IsGroundInFront())
                        {
                            // if player in range attack
                            if (!playerInMelee() && playerInRange())
                            {
                                LookAtTarget();
                                // if there is no obstacle in front of him execute range attack
                                if (!collidingWithObstacle)
                                {
                                    // play throw animation and at the end of it set executingrangeAttack to false

                                    Move(_facingRight);
                                    timer -= Time.fixedDeltaTime;
                                    if (timer <= 0)
                                    {
                                        enemyInterface.RangeAttack();
                                        timer = timeToPerformARangeAttackAgain;
                                    }
                                }
                                // else if there is an obstacle return back
                                else
                                {
                                    state = EnemyState.ReturningToStartPosition;

                                }
                            }
                            // change to Melee state if the player in melee range
                            else if (playerInMelee() && haveMeleeAttack)
                            {
                                state = EnemyState.MeleAttack;

                            }
                            // change to chasing state if player out of range
                            else if (!playerInRange())
                            {
                                state = EnemyState.Chasing;
                            }

                        }
                        // if there is no ground in front of the enemy
                        else
                        {

                            // if player is not in range or there is an obstacle in front of the enemy retrun back to start pos
                            if (!playerInRange())
                            {
                                if (CanSeePlayer() && !collidingWithObstacle)
                                {
                                    state = EnemyState.Chasing;
                                }
                                else
                                {
                                    state = EnemyState.ReturningToStartPosition;
                                }
                            }
                            // otherwise if player still in range even if there is no ground in front of the enemy execute range attack
                            else
                            {
                                enemyInterface.RangeAttack();

                                //timer -= Time.fixedDeltaTime;
                                //if (timer <= 0)
                                //{
                                //    ThrowKnife();
                                //    timer = timeToThrowKnife;
                                //}
                            }
                        }
                    }
                    

                    break;
            

                case EnemyState.MeleAttack:
                    //if not already executing melee attack
                    if (!ExecutingMeleeAttack)
                    {
                        // if the enemy have a range attack
                        if (haveRangeAttack)
                        {
                            // if there is ground in front of him
                            if (IsGroundInFront())
                            {
                                // no obstacle in front and player in melee range : execute melee attack
                                if (playerInMelee() && !collidingWithObstacle)
                                {
                                    LookAtTarget();
                                    // set to off in animation;

                                    Move(_facingRight);
                                    ExecutingMeleeAttack = true;
                                    enemyInterface.MeleeAttack();
                                    timer -= Time.fixedDeltaTime;
                                    //if (timer <= 0)
                                    //{

                                    //    timer = timeToPerformAMeleettackAgain;
                                    //}
                                }
                                // not in melee range or obstacle in front of him : return to start pos
                                else
                                {
                                    state = EnemyState.RangeAttack;
                                }
                            }
                            // no ground in front
                            else
                            {
                                // if player in range and no obstacle infront of the enemy : change to range state
                                if (playerInRange() && !collidingWithObstacle)
                                {

                                    state = EnemyState.RangeAttack;

                                    //LookAtTarget();
                                    //timer -= Time.fixedDeltaTime;
                                    //if (timer <= 0)
                                    //{
                                    //    ThrowKnife();
                                    //    timer = timeToThrowKnife;
                                    //}
                                }
                                // otherwise change to reutrn to start pos
                                else
                                {
                                    state = EnemyState.ReturningToStartPosition;
                                }
                            }
                        }
                        // in case the enemy have no range attack
                        else
                        {
                            // if there is ground in front
                            if (IsGroundInFront())
                            {
                                // execute melee attack if player in melee range and no obstacle in front of the enemy
                                if (playerInMelee() && !collidingWithObstacle)
                                {
                                    ExecutingMeleeAttack = true;
                                    enemyInterface.MeleeAttack();

                                    LookAtTarget();

                                }
                                else
                                {
                                    state = EnemyState.Chasing;
                                }
                            }
                            else
                            {
                                if (CanSeePlayer() && !collidingWithObstacle)
                                {
                                    LookAtTarget();
                                    state = EnemyState.Chasing;

                                }

                                else
                                {
                                    state = EnemyState.ReturningToStartPosition;
                                }
                            }
                        }
                    }
                   
                    break;


                case EnemyState.ReturningToStartPosition:


                    LookAtStartingPosition();
                    Move(_facingRight);
                    if (IscloseToStartPos())
                    {
                        state = EnemyState.Idle;
                    }
                    break;


                case EnemyState.Death:

                    break;
            }

        }
    }

    private bool CanSeePlayer()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < agroDistanceX && Mathf.Abs(player.transform.position.y - transform.position.y) < agroDistanceY && (player.transform.position.y - transform.position.y) > targetMinHeight)
        {
            return true;
        }
        else return false;
    }
    private bool playerInRange()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < RangeDistanceX && Mathf.Abs(player.transform.position.y - transform.position.y) < RangeDistanceY && (player.transform.position.y - transform.position.y) > targetMinHeight)
        {
            return true;
        }
        else return false;
    }
    private bool playerInMelee()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < MeleeDistanceX && Mathf.Abs(player.transform.position.y - transform.position.y) < MeleeDistanceY && (player.transform.position.y - transform.position.y) > targetMinHeight)
        {
            return true;
        }
        else return false;
    }

    private bool IscloseToStartPos()
    {
        if (Mathf.Abs(transform.position.x - startpos.x) <= 0.5f)
        {
            return true;
        }
        else return false;
    }

    private void Move(bool toRight)
    {
        int direction;
        if (toRight)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
        rb.AddForce(new Vector2(direction, 0f) * acceleration);
        if (Mathf.Abs(rb.velocity.x) > speed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x)
                * (speed), rb.velocity.y);
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
    bool IsGroundInFront()
    {
        if (Physics2D.Linecast(groundFrontCheckStart1.position, groundFrontCheckEnd1.position, whatisGround)
            || Physics2D.Linecast(groundFrontCheckStart2.position, groundFrontCheckEnd2.position, whatisGround))
        {
            return true;
        }
        return false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Spikes") || collision.gameObject.CompareTag("RollingSpikes"))
        //{
        //    state = EnemyState.Death;
        //}

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.CompareTag("Wall"))
        {
            collidedWithObstacle = true;
            collidingWithObstacle = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.CompareTag("Wall"))
        {

            collidingWithObstacle = false;
        }
    }


}
