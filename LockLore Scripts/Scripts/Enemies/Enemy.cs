using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;

[SelectionBase]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] float agroDistance;
    [SerializeField] float meleeDistance;
    [SerializeField] float speed;
    [SerializeField] float acceleration;
    [SerializeField] float groundDrag;
    [SerializeField] float airDrag;
    [SerializeField] float jumpAttackDrag;
    PlayerScript player;
    Rigidbody rb;
    [SerializeField] Animator animator;
    [SerializeField] string chasingAnimation;
    [SerializeField] string idleAnimation;
    [SerializeField] string attackAnimation;
    [SerializeField] string takeDamageAnimation;
    [SerializeField] string idleBatteAnimation;
    [SerializeField] string deathAnimation;

    [SerializeField] Material hitMaterial;
    List<Material> hitMaterials = new();
    List<Material> orginalMaterials = new();
    [SerializeField] Renderer bodyRenderer;


    [SerializeField] Collider hitBox;

    [SerializeField] float health;
    bool isDead;

    bool isTakingDamage;
    [SerializeField] GameObject bloodDir;
    [SerializeField] GameObject blood;
    [SerializeField] Transform bloodPos;

    [SerializeField] Transform groundCheck;


    [SerializeField] float delayBetweenAttacks;
    float delayTimer;
    bool isAttacking;

    [SerializeField] float rotationSpeed;

    [SerializeField] LayerMask WhatIsGround;

    [SerializeField] float JumpForceY;
    [SerializeField] float JumpForceZ;
    [SerializeField] float JumpForceY2;
    [SerializeField] float JumpForceZ2;
    bool jumpAttack;

    enum Stat
    {
        idle,
        chasing,
        melee,
        takingDamage
    }
    Stat stat;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        rb = GetComponent<Rigidbody>();
        orginalMaterials = bodyRenderer.materials.ToList();
        for (int i = 0; i < orginalMaterials.Count; i++)
        {
            hitMaterials.Add(hitMaterial);
        }

    }

    public void Death()
    {
       
        this.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
        ClampVelocity();
        if (isDead)
        {
            animator.Play(deathAnimation);
            gameObject.layer = 1;
        }
        else
        {
            Stats();
            Animations();
            ManageDrag();
        }


        if (health <= 0)
        {
            isDead = true;
        }

    }
    private void FixedUpdate()
    {
        switch (stat)
        {
            case Stat.idle:
                break;
            case Stat.chasing:
                if (!isAttacking)
                {
                    ChasePlayer();
                    RotateTowardPlayerMelee();
                }
                break;
            case Stat.melee:
                if (!isAttacking)
                {
                    RotateTowardPlayerMelee();
                }
                break;
            default:
                break;
        }
    }
    void Stats()
    {
        Debug.Log(stat);
        if (player != null)
        {


            if (!isTakingDamage && !isAttacking && !animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation))
            {
                jumpAttack = false;
                var flatPosition = new Vector3(transform.position.x, 0, transform.position.z);
                var flatPlayerPos = new Vector3(player.transform.position.x, 0, player.transform.position.z);
                var flatDistance = Vector3.Distance(flatPosition, flatPlayerPos);
                if (flatDistance <= agroDistance && flatDistance > meleeDistance)
                {

                    stat = Stat.chasing;

                }
                else if (flatDistance <= meleeDistance)
                {
                    stat = Stat.melee;

                }
                else
                {
                    stat = Stat.idle;
                }
            }
            else if (isTakingDamage)
            {
                stat = Stat.takingDamage;
            }






        }
    }
    void Animations()
    {
        switch (stat)
        {
            case Stat.idle:
                animator.Play(idleAnimation);
                break;
            case Stat.chasing:
                animator.Play(chasingAnimation);
                delayTimer = delayBetweenAttacks;
                break;
            case Stat.melee:
                delayTimer += Time.deltaTime;
               
                if (delayTimer > delayBetweenAttacks)
                {
                    animator.Play(attackAnimation);
                    delayTimer = 0;
                }
                else
                {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation))
                    {
                        animator.Play(idleBatteAnimation);
                    }
                }
                break;
            case Stat.takingDamage:
                delayTimer = delayBetweenAttacks;
                if (isTakingDamage)
                {
                    animator.Play(takeDamageAnimation);
       
                }
                break;
            default:
                break;
        }
    }

    void ManageDrag()
    {
        if (IsGrounded() && !jumpAttack)
        {
            rb.drag = groundDrag;
        }
        else if (jumpAttack)
        {
            rb.drag = jumpAttackDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }
 

    public void TakingDamageToFalse()
    {
        isTakingDamage = false;
    }

  

    void ClampVelocity()
    {
        if (rb.velocity.magnitude >= speed)
        {
            Vector3.ClampMagnitude(rb.velocity, speed);
        }
    }
    void ChasePlayer()
    {
        
        if (rb.velocity.magnitude >= speed)
        {
           Vector3.ClampMagnitude(rb.velocity, speed);
        }
        else
        {
            rb.AddForce(acceleration * transform.forward, ForceMode.Force);
        }

    }


    void RotateTowardPlayerMelee()
    {
        Vector3 playerDirection = player.transform.position - transform.position;
        transform.forward = Vector3.Slerp(transform.forward, new Vector3(playerDirection.x,0, playerDirection.z), rotationSpeed);

    }


    [SerializeField] float damageForce;
    bool canTakeDamageAgain = true;
    public void TakeDamage(float damage, Vector3 playerPos)
    {


            health -= damage;
            isTakingDamage = true;
            animator.Play(takeDamageAnimation, -1, 0f);
            StartCoroutine(DamageEffect());
            Instantiate(blood, bloodPos.position, Quaternion.identity);
            Vector3 playerDirection = playerPos - transform.position;
            rb.AddForce(damageForce * -playerDirection.normalized, ForceMode.Impulse);

    }

    IEnumerator DamageEffect()
    {
        canTakeDamageAgain = false;
        bodyRenderer.materials = hitMaterials.ToArray();
        yield return new WaitForSecondsRealtime(0.1f);
        bodyRenderer.materials = orginalMaterials.ToArray();
        
        isAttacking = false;
        yield return new WaitForSecondsRealtime(0.2f);
        canTakeDamageAgain = true;
    }

    public void ActivateHitBox()
    {
        hitBox.enabled = true;
        isAttacking = true;
    }
    public void DesactivateHitBox()
    {
        hitBox.enabled = false;
    }
    public void IsAttackingToFalse()
    {
        isAttacking = false;
    }


    internal bool IsGrounded()
    {
        if (Physics.Linecast(transform.position, groundCheck.position, WhatIsGround))
        {
            return true;
        }
        return false;
    }



    public void JumpForward()
    {
        
        if (IsGrounded())
        {
            
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * JumpForceY, ForceMode.Impulse);
            rb.AddForce(transform.forward * JumpForceZ, ForceMode.Impulse);
            jumpAttack = true;
        }
       
    }
    public void JumpBackWard()
    {
        if (IsGrounded())
        {
            
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * JumpForceY2, ForceMode.Impulse);
            rb.AddForce(transform.forward * JumpForceZ2, ForceMode.Impulse);
            jumpAttack = true;
        }
    }


    public void jumpAttackToFalse()
    {
        jumpAttack = false;
    }

    public bool IsTakingDamage()
    {
        return !canTakeDamageAgain;
    }
}
 