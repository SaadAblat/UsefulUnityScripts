using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWhenHitGround : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            Destroy(gameObject);
        }
    }
}
