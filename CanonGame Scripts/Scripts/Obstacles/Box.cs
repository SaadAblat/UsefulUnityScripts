using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] int fallingSpeedThatDestroys;
    Rigidbody rb;
    //bool isFalling;
    private void Awake()
    {
       rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("CannonBall"))
        {
            health -= collision.gameObject.GetComponent<CanonBall>().Damage;
        }
        //if (collision.gameObject.CompareTag("Ground"))
        //{
        //    if (isFalling)
        //    {
        //        Explode();
        //    }
        //}
    }
    private void Update()
    {
        //if (rb.velocity.y < fallingSpeedThatDestroys)
        //{
        //    isFalling = true;
        //}
        if (health <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Destroy(gameObject);
        Score.GameScore += 5;
    }

}
