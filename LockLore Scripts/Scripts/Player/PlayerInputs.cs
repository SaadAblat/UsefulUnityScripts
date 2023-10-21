using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    [Header("References")]
    [SerializeField] float jumpPressedRememberTime;

    [SerializeField] float ActionPressedRememberTime;

    [Header("Inputs")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode InteractKey = KeyCode.X;
    public KeyCode ActionKey = KeyCode.C;


    internal float horizontal;
    internal float vertical;
    internal bool jumpRequested = false;

    internal bool Interact;
    internal bool Action;
    internal float hangTimeCounter;


    float jumpPressedRememberTimer;
    float ActionPressedRememberTimer;




    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        jumpPressedRememberTimer -= Time.deltaTime;
        ActionPressedRememberTimer -= Time.deltaTime;


        if (Input.GetKeyDown(jumpKey))
        {
            jumpPressedRememberTimer = jumpPressedRememberTime;
        }
        if (jumpPressedRememberTimer > 0 && hangTimeCounter > 0)
        {
            jumpRequested = true;
        }

        if (Input.GetKeyDown(InteractKey))
        {
            Interact = true;
        }
        if (Input.GetKeyUp(InteractKey))
        {
            Interact = false;
        }
        if (Input.GetKeyDown(ActionKey))
        {
            ActionPressedRememberTimer = ActionPressedRememberTime;
        }

        if (ActionPressedRememberTimer > 0)
        {
            Action = true;
        }

    }
}
