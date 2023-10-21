using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStuckWhenHitTag : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private string Tagi;
    private Vector3 saveVelocityBeforImpact;
    private GameObject col;
    private bool collidiedWithWood = false;

    float initialDrag;

    private void Start()
    {
        initialDrag = rb.drag;
    }
    private void Update()
    {
        if (col == null && collidiedWithWood)
        {
            rb.isKinematic = false;
            rb.freezeRotation = false;
            transform.parent = null;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tagi))
        {
            transform.parent = collision.transform.parent;
            rb.isKinematic = true;
            saveVelocityBeforImpact = rb.velocity;
            rb.velocity = Vector2.zero;
            rb.freezeRotation = true;
            col = collision.gameObject;
            collidiedWithWood = true;
        }
        else
        {
            rb.drag = 10f;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        rb.drag = initialDrag;
    }
}
