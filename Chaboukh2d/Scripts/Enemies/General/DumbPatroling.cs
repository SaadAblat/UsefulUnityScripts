using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbPatroling : MonoBehaviour
{
    [SerializeField] Transform Right;
    [SerializeField] Transform Left;
    [SerializeField] float speed;
    [SerializeField] float acceleration;
    Rigidbody2D lizRb;
    bool facingRight;



    //Animations
    [SerializeField] Animator lAnim;

    public enum LizzardMovementDir
    {
        movingToRight,
        movingToLeft
    }
    LizzardMovementDir state;


    private void Move(int direction)
    {
        lizRb.AddForce(new Vector2(direction, 0f) * acceleration);
        if (Mathf.Abs(lizRb.velocity.x) > speed)
        {
            lizRb.velocity = new Vector2(Mathf.Sign(lizRb.velocity.x)
                * (speed), lizRb.velocity.y);
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        lizRb = GetComponent<Rigidbody2D>();
        state = LizzardMovementDir.movingToLeft;

    }

    private void FixedUpdate()
    {
        if (facingRight)
        {
            Move(1);
        }
        else
        {
            Move(-1);
        }
    }

    // Update is called once per frame
    private void Update ()
    {

        switch (state)
        {
            case LizzardMovementDir.movingToRight:
                facingRight = true;
                transform.localScale = new Vector3(-1, 1, 1);
                if (transform.position.x >= Right.position.x)
                {
                    state = LizzardMovementDir.movingToLeft;
                }
                break;
            case LizzardMovementDir.movingToLeft:
                facingRight = false;
                transform.localScale = new Vector3(1, 1, 1);
                if (transform.position.x <= Left.position.x)
                {
                    state = LizzardMovementDir.movingToRight;

                }
                break;
        }
    }



}
