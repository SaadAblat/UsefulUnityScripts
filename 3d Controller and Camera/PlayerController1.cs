using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEditor.Animations;
public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform orientation;
    Vector3 moveDirection;
    Rigidbody rb;
    float horizontal;
    float vertical;




    //[SerializeField] Animator Jump;
    //[SerializeField] Animator Fall;
    [SerializeField] Animator an;

    [Header("Movement")]
    [SerializeField] float airMultilpier;
    [Header("Movement Variables")]
    [SerializeField]
    float acceleration;

    [SerializeField]
    float runSpeed;

    [SerializeField]
    float groundLinearDrag;
    [SerializeField]
    float airlinearDrag;


    bool isSpriting;
    float sprintMultiplier;
    [SerializeField] float setSprintMultiplier;


    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;


    [Header("Ground Check")]
    [SerializeField] Transform[] groundCheck;
    [SerializeField] LayerMask whatIsGround;
    bool jumpRequested = false;

    [Header("Jump Variables")]
    [SerializeField]
    float JumpForce;

    [SerializeField]
    float hangTime;
    float hangTimeCounter;
    [SerializeField]
    float jumpTime;
    [SerializeField]
    float jumpPressedRememberTime;
    float jumpPressedRememberTimer;
    [SerializeField]
    float _minSpeedtoFall;

    [SerializeField]
    float fallMultipier;
    [SerializeField]
    float lowJumpMultipier;

    bool hasLanded;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        sprintMultiplier = 1;
    }
    private void FixedUpdate()
    {
        Move();
        ManageDrag();
        HandleMovement();
    }
    // Update is called once per frame

    float notGroundedTimer;
    [SerializeField] float notGroundedTimeToPlayFallAnimation;
    void Update()
    {
        GetInput();

        if (!IsGrounded())
        {
            notGroundedTimer += Time.deltaTime;
            FallMultiplier();

            if (rb.velocity.y > 0)
            {
                an.Play("Jump");
            }
            else
            {
                if (notGroundedTimer >= notGroundedTimeToPlayFallAnimation)
                {
                    hasLanded = false;
                    an.Play("Land");
                }
            }
            

        }
        else
        {
            if (!hasLanded)
            {
                an.Play("HitGround");
                Invoke(nameof(HasLandedToFalse), 0.35f);
                Vector3 flatVelocity = Vector3.zero;
            }
            else
            {
                Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                if (flatVelocity.magnitude < 3f)
                {
                    an.Play("Idle");

                }
                else
                {
                    an.Play("Run");
                }
            }
            notGroundedTimer = 0;
            
                
            
        }

    }
    void HasLandedToFalse()
    {
        hasLanded = true;
    }
    private void HandleMovement()
    {
        if (IsGrounded())
        {
            hangTimeCounter = hangTime;
        }
        hangTimeCounter -= Time.fixedDeltaTime;
        if (jumpRequested)
        {
            Jump();
        }
        else
        {
            jumpRequested = false;
        }


    }

    void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        jumpPressedRememberTimer -= Time.deltaTime;

        if (Input.GetKeyDown(jumpKey))
        {
            jumpPressedRememberTimer = jumpPressedRememberTime;
        }
        if (jumpPressedRememberTimer > 0 && hangTimeCounter > 0)
        {
            jumpRequested = true;
        }

        if (Input.GetKey(sprintKey))
        {
            isSpriting = true;
        }
        else
        {
            isSpriting = false;
        }
    }
    void Move()
    {
        // movement direction
        moveDirection = orientation.forward * vertical + orientation.right * horizontal;

        

       Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

       

        if (flatVelocity.magnitude > (runSpeed * sprintMultiplier) && IsGrounded())
        {
            Vector3 limitVelocity = flatVelocity.normalized * runSpeed * sprintMultiplier;
            rb.velocity = new Vector3(limitVelocity.x, rb.velocity.y, limitVelocity.z);
            
        }
        else
        {
            if (IsGrounded())
            {
                rb.AddForce(acceleration * sprintMultiplier * moveDirection.normalized, ForceMode.Force);
            }
            else
            {
                rb.AddForce(acceleration * airMultilpier * sprintMultiplier * moveDirection.normalized, ForceMode.Force);
            }
        }

        SprintMultiplier();
    }
    private void GroundLinearDrag()
    {
        // this makes a difference in jump high between while moving vs jumping without moving, cause the groundlinear drag get activated when still 
        if (Mathf.Abs(horizontal) < 0.4f && Mathf.Abs(vertical) < 0.4f && !Input.GetKey(jumpKey))
        {
            rb.drag = groundLinearDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
   
    private void AirLinearDrag()
    {
        rb.drag = airlinearDrag;
    }

    private void SprintMultiplier()
    {
        if (!isSpriting)
        {
            sprintMultiplier = 1f;
        }
        else
        {
            sprintMultiplier = setSprintMultiplier;
        }
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

    [SerializeField] float HoritonalJumpForce;
    private void Jump()
    {
        jumpRequested = false;

        moveDirection = orientation.forward * vertical + orientation.right * horizontal;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        rb.AddForce(moveDirection.normalized * HoritonalJumpForce, ForceMode.Impulse);
        

        hangTimeCounter = 0;
    }

    private void FallMultiplier()
    {
        if (rb.velocity.y < _minSpeedtoFall)
        {
            rb.velocity += (fallMultipier - 1) * Physics.gravity.y * Time.deltaTime * Vector3.up;
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += (lowJumpMultipier - 1) * Physics.gravity.y * Time.deltaTime * Vector3.up;
        }
    }

}
