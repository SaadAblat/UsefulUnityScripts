using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : TrapsBase
{
    Rigidbody2D rb;
    bool hadLeftTheGround =false;
    float timer;
    float fallingTimeToBecomeDangerous;
    [SerializeField] private float speedToBecomeDangerous;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        fallingTimeToBecomeDangerous = 0.2f;
    }

    public override void ActivateTrap()
    {
        rb.isKinematic = false;
        if (hadLeftTheGround )
        {
            timer += Time.deltaTime;
            if (timer >= fallingTimeToBecomeDangerous)
            {
                gameObject.tag = "RollingSpikes";
            }
        }
        if (Mathf.Abs(rb.velocity.y) > speedToBecomeDangerous)
        {
            gameObject.tag = "RollingSpikes";
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 && Mathf.Abs(rb.velocity.y) <= speedToBecomeDangerous)
        {
            gameObject.tag = "Untagged";
            hadLeftTheGround = false;
            timer = 0;  
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            hadLeftTheGround = true;
        }
    }
}
