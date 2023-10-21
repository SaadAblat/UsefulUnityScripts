using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : TrapsBase
{
    [SerializeField] private bool right;
    [SerializeField] private float speed;
    Rigidbody2D rb;
    bool StopMoving;
    Vector2 startPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        StopMoving = false;
        startPos = transform.position;
    }
    public override void ActivateTrap()
    {
        base.ActivateTrap();
        if (!StopMoving)
        {
            rb.velocity = GetDirection() * speed;
        }

    }

    Vector2 GetDirection()
    {
        if (right)
            return Vector2.right;

        return Vector2.left;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        StartCoroutine(ResetTrap());
    }

    IEnumerator ResetTrap()
    {
        rb.gravityScale = 1;
        StopMoving = true;
        yield return new WaitForSeconds(0.2f);
        if (StopMoving)
        {
            transform.position = startPos;
            StopMoving = false;
            rb.gravityScale = 0;
            rb.velocity = Vector2.zero;
        }

    }
}
