using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnTagRigideBodyActive : MonoBehaviour
{
    [SerializeField] string Tag;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }

    //Added Recently it can be the cause of some problems
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tag))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }
}
