
using Unity.VisualScripting;
using UnityEngine;


[SelectionBase]
public class PlayerController : MonoBehaviour
{
    Vector3 moveDirection;


    
    

    [SerializeField] PlayerScript ps;

    [Header("Movement Variables")]
    [SerializeField] float rotationSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float runSpeed;
    [SerializeField] float groundLinearDrag;
    [SerializeField] float airlinearDrag;

    [Header("Jump Variables")]
    [SerializeField] float JumpForce;
    [SerializeField] float HoritonalJumpForce;
    [SerializeField] float airMultilpier;
    [SerializeField] float hangTime;

    [SerializeField] float jumpTime;


    [SerializeField] float _minSpeedtoFall;
    [SerializeField] float fallMultipier;
    [SerializeField] float lowJumpMultipier;

    internal bool HasLanded;
    [Header("Ground Check")]
    [SerializeField] Transform[] groundCheck;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] Transform[] stairsCheck;
    [SerializeField] string StairsTag;
    [Header("Pushing")]
    internal bool isPushing;
    [Header("Player Step Climb")]
    [SerializeField] GameObject stepRayUpper;
    [SerializeField] GameObject stepRayLower;
    [SerializeField] float stepSmooth = 2f;
    [Header("Animation")]
    float notGroundedTimer;
    [SerializeField] float notGroundedTimeToPlayFallAnimation;
    internal bool IsAttacking;

    internal bool CanRotate = true;
    private void OnDisable()
    {
        ResetAttackVariables();
        ps.Inputs.hangTimeCounter = hangTime;
        HasLanded = true;
    }
    void Start()
    {
    }
    private void FixedUpdate()
    {
        StepClimb();
        if (!IsAttacking)
        {
            Move();
        }
        ManageDrag();
        HandleMovement();
    }
    void Update()
    {
        moveDirection = transform.forward * ps.Inputs.horizontal + -1 * ps.Inputs.vertical * transform.right;
        if (CanRotate)
        {
            Rotate();
        }
        

        ManageAnimation();
        ManageAttack();
        if (!IsGrounded())
        {
            notGroundedTimer += Time.deltaTime;
            FallMultiplier();
        }
        else
        {
            notGroundedTimer = 0;
        }
    }
    void HasLandedToTrue()
    {
        HasLanded = true;
    }
    private void HandleMovement()
    {
        if (IsGrounded())
        {
            ps.Inputs.hangTimeCounter = hangTime;
        }
        ps.Inputs.hangTimeCounter -= Time.fixedDeltaTime;
        if (ps.Inputs.jumpRequested && !IsAttacking)
        {
            Jump();
        }
        else
        {
            ps.Inputs.jumpRequested = false;
        }
    }
    void Rotate()
    {


        if (moveDirection != Vector3.zero)
        {
            ps.PlayerObject.transform.forward = Vector3.Slerp(ps.PlayerObject.transform.forward, moveDirection.normalized, Time.deltaTime * rotationSpeed);
        }
    }
    void Move()
    {
        //moveDirection = transform.forward * ps.Inputs.horizontal + -1 * ps.Inputs.vertical * transform.right;
        float pushingFactor;
        if (isPushing)
        {
            pushingFactor = 0.7f;
            if (Mathf.Abs(ps.Inputs.vertical) != 0)
            {
                ps.rb.constraints = RigidbodyConstraints.FreezePositionZ;
                ps.rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
            else if (Mathf.Abs(ps.Inputs.horizontal) != 0)
            {
                ps.rb.constraints = RigidbodyConstraints.FreezePositionX;
                ps.rb.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
        else
        {
            ps.rb.constraints = RigidbodyConstraints.None;
            ps.rb.constraints = RigidbodyConstraints.FreezeRotation;
            pushingFactor = 1;
        }

      
        if (IsGrounded())
        {
            ps.rb.AddForce(acceleration * pushingFactor * moveDirection.normalized, ForceMode.Force);
        }
        else
        {
            ps.rb.AddForce(acceleration * pushingFactor * airMultilpier * moveDirection.normalized, ForceMode.Force);
        }
        Vector3 flatVelocity = new Vector3(ps.rb.velocity.x, 0f, ps.rb.velocity.z);
        if (flatVelocity.magnitude > (runSpeed * pushingFactor))
        {
            Vector3 limitVelocity = Vector3.ClampMagnitude(flatVelocity, runSpeed * pushingFactor);
            ps.rb.velocity = new Vector3(limitVelocity.x, ps.rb.velocity.y, limitVelocity.z);

        }
    }
    [SerializeField] float attackDrag;
    private void GroundLinearDrag()
    {
        // this makes a difference in jump high between while moving vs jumping without moving, cause the groundlinear drag get activated when still , solved by checking if
        // the jumpKey is not pressed
        if (((Mathf.Abs(ps.Inputs.horizontal) < 0.4f && Mathf.Abs(ps.Inputs.vertical) < 0.4f  && !ps.Inputs.jumpRequested)))
        {
            if (ComboIsPlaying || ps.IsTakingDamage)
            {
                ps.rb.drag = attackDrag;
            }
            else
            {
                ps.rb.drag = groundLinearDrag;
            }
              
        }
        else
        {
            ps.rb.drag = 0;
        }

    }
    private void AirLinearDrag()
    {
        ps.rb.drag = airlinearDrag;
    }
    internal bool IsGrounded()
    {
        foreach (Transform groundcheck in groundCheck)
        {
            if (Physics.Linecast(transform.position, groundcheck.position, whatIsGround))
            {
                return true;
            }
        }
        return false;
    }
    internal bool IsClimbingStairs()
    {
        foreach (Transform stairCheck in stairsCheck)
        {
            if (Physics.Linecast(transform.position, stairCheck.position, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.CompareTag(StairsTag))
                {
                    return true;
                }
            }

        }
        return false;
    }
    void ManageDrag()
    {
        if (IsGrounded())
        {
            GroundLinearDrag();
        }
        else
        {
            AirLinearDrag();
        }
    }
    private void Jump()
    {
        ps.Inputs.jumpRequested = false;
        moveDirection = transform.forward * ps.Inputs.horizontal + -1 * ps.Inputs.vertical * transform.right;    
        ps.rb.velocity = new Vector3(ps.rb.velocity.x, 0f, ps.rb.velocity.z);
        ps.rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        ps.rb.AddForce(moveDirection.normalized * HoritonalJumpForce, ForceMode.Impulse);
        ps.Inputs.hangTimeCounter = 0;
    }
    private void FallMultiplier()
    {
        if (ps.rb.velocity.y < _minSpeedtoFall)
        {
            ps.rb.velocity += (fallMultipier - 1) * Physics.gravity.y * Time.deltaTime * Vector3.up;
        }
        else if (ps.rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            ps.rb.velocity += (lowJumpMultipier - 1) * Physics.gravity.y * Time.deltaTime * Vector3.up;
        }
    }

    void IsPushingToFalse()
    {
        isPushing = false;
    }
    void StepClimb()
    {
        if (ps.Inputs.vertical != 0 ||  ps.Inputs.horizontal != 0)
        {
            RaycastHit hitLower;
            if (Physics.Raycast(stepRayLower.transform.position, ps.PlayerObject.transform.TransformDirection(Vector3.forward), out hitLower, 0.2f))
            {
                RaycastHit hitUpper;
                if (!Physics.Raycast(stepRayUpper.transform.position, ps.PlayerObject.transform.TransformDirection(Vector3.forward), out hitUpper, 0.4f))
                {
                 
                    ps.rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
                }
            }
            RaycastHit hitLower45;
            if (Physics.Raycast(stepRayLower.transform.position, ps.PlayerObject.transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.2f))
            {
                RaycastHit hitUpper45;
                if (!Physics.Raycast(stepRayUpper.transform.position, ps.PlayerObject.transform.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.4f))
                {
                    ps.rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
                }
            }

            RaycastHit hitLowerMinus45;
            if (Physics.Raycast(stepRayLower.transform.position, ps.PlayerObject.transform.TransformDirection(-1.5f, 0, 1), out hitLowerMinus45, 0.2f))
            {

                RaycastHit hitUpperMinus45;
                if (!Physics.Raycast(stepRayUpper.transform.position, ps.PlayerObject.transform.TransformDirection(-1.5f, 0, 1), out hitUpperMinus45, 0.4f))
                {
                    ps.rb.position -= new Vector3(0f, -stepSmooth * Time.deltaTime, 0f);
                }
            }

            if (IsClimbingStairs() && (Mathf.Abs(ps.Inputs.horizontal) > 0 || Mathf.Abs(ps.Inputs.vertical) > 0) && Mathf.Abs(ps.rb.velocity.magnitude) < 3f)
            {
                
                ps.rb.position -= new Vector3(ps.Inputs.horizontal * -stepSmooth * Time.deltaTime, -stepSmooth * Time.deltaTime, ps.Inputs.vertical * -stepSmooth * Time.deltaTime);

                
            }

        }
        
    }

    void ManageAnimation()
    {
        comboTimer += Time.deltaTime;
        if (comboTimer >= timeToResetCombo)
        {
            attackIndex = 0;
            comboTimer = 0;
        }
        if (!IsAttacking)
        {
            timerNotAttackingBeforePlayingIdle += Time.deltaTime;
            if (timerNotAttackingBeforePlayingIdle >= timeNotAttackingBeforePlayingIdle)
            {
                if (!IsGrounded() && !IsClimbingStairs())
                {
                    ps.an.speed = 1;
                    if (ps.rb.velocity.y > 0)
                    {
                        ps.an.Play("Jump");
                    }
                    else
                    {
                        if (notGroundedTimer >= notGroundedTimeToPlayFallAnimation / 2.3)
                        {
                            ps.an.Play("Land");
                        }
                        if (notGroundedTimer >= notGroundedTimeToPlayFallAnimation)
                        {

                            HasLanded = false;

                        }
                    }
                }
                else
                {
                    if (!HasLanded)
                    {

                        if (ps.Inputs.horizontal == 0 && ps.Inputs.vertical == 0)
                        {
                            if (!ps.an.GetCurrentAnimatorStateInfo(0).IsName("HitGroundRuninng"))
                            {
                                ps.an.Play("HitGround");
                                Invoke(nameof(HasLandedToTrue), 0.25f);

                            }
                        }
                        else
                        {
                            if (!ps.an.GetCurrentAnimatorStateInfo(0).IsName("HitGround"))
                            {
                                ps.an.Play("HitGroundRuninng");
                                Invoke(nameof(HasLandedToTrue), 0.26f);

                            }
                        }
                    }
                    else
                    {
                        if (isPushing)
                        {

                            ps.an.Play("Push");
                            Vector3 flatVelocity = new Vector3(ps.rb.velocity.x, 0f, ps.rb.velocity.z);
                            ps.an.speed = flatVelocity.magnitude / 2;

                        }
                        else
                        {
                            ps.an.speed = 1;
                            if (ps.Inputs.vertical == 0 && ps.Inputs.horizontal == 0)
                            {
                                if (ps.Weapons.holdingWeapon)
                                {
                                    ps.an.Play("IdleHolding");
                                }
                                else
                                {
                                    ps.an.Play("Idle");
                                }
                            }
                            else
                            {
                                if (ps.Weapons.holdingWeapon)
                                {
                                    ps.an.Play("RunHolding");
                                }
                                else
                                {
                                    ps.an.Play("Run");
                                }
                            }
                        }
                    }
                }
            }
        
        }
        else
        {
            timerNotAttackingBeforePlayingIdle = 0;
            if (ps.Weapons.PlayerWeaponName == PlayerWeapons.PlayerWeaponList.axe)
            {
                ps.an.Play("Throwing");
            }
            else if (ps.Weapons.PlayerWeaponName == PlayerWeapons.PlayerWeaponList.sword)
            {
              
                if (!ComboIsPlaying)
                {
                    if (IsGrounded())
                    {
                        ps.attackPushActive = true;
                    }
              
                    if (attackIndex == 0)
                    {
                        ps.an.Play("Swing1");
                        attackIndex++;
                        comboTimer = 0;
                        ComboIsPlaying = true;


                    }
                    else if (attackIndex == 1)
                    {
                        ps.an.Play("Swing2");
                        attackIndex++;
                        comboTimer = 0;
                        ComboIsPlaying = true;

                    }
                    else if (attackIndex == 2)
                    {
                        ps.an.Play("Thrust");
                        attackIndex = 0;
                        comboTimer = 0;
                        ComboIsPlaying = true;

                    }
                }
            }
        }
    }


    int attackIndex = 0;
    [SerializeField] float timeToResetCombo;
    float comboTimer;
    public bool ComboIsPlaying;
    float timerNotAttackingBeforePlayingIdle;
    [SerializeField] float timeNotAttackingBeforePlayingIdle;
    private void ResetAttackVariables()
    {
        ps.Inputs.Action = false;
        IsAttacking = false;
        timerNotAttackingBeforePlayingIdle = timeNotAttackingBeforePlayingIdle;
        comboTimer = 0;
        ComboIsPlaying = false;
    }
    void ManageAttack()
    {

        if (ps.Inputs.Action  && !isPushing)
        {
            if (ps.Weapons.AxeEquiped)
            {
                if (!ps.Weapons.playerIsWaitingForTheAxeToReturn)
                {
                    ps.Inputs.Action = false;
                    IsAttacking = true;
                    if (IsGrounded())
                    {
                        ps.rb.velocity = Vector3.zero;
                    }
                }
                else
                {
                    ps.Inputs.Action = false;
                }
                
            }
            else if (ps.Weapons.SwordEquiped)
            {

                IsAttacking = true;
                ps.rb.velocity =new Vector3(0, ps.rb.velocity.y,0);
                ps.Inputs.Action = false;

            }
        }
        else if (isPushing)
        {
            ResetAttackVariables();
        }
    }
    private void OnEnable()
    {
        ps.Inputs.Action = false;
        if (ps.rb.velocity.magnitude > runSpeed)
        {
            ps.rb.velocity = Vector3.zero;
        }
    }

    

}
