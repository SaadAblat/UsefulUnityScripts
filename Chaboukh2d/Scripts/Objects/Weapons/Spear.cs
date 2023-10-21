using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    Rigidbody2D rb;
    //BoxCollider2D col;
    bool collided = false;
    [SerializeField] float spearThrowForce_X;
    [SerializeField] float spearThrowForce_Y;
    [SerializeField] float torqueForce;
    [SerializeField] Collider2D triggerCollider;
    [SerializeField] GameObject bottomSpearPrefab;
    [SerializeField] Transform bottomSpearstartpos;
    private bool collidedWithEnemy;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //col = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {

        rb.velocity = new Vector2(0f, 0f);
        rb.AddForce(Vector2.up * spearThrowForce_Y, ForceMode2D.Impulse);
        if (PlayerScript.Instance.playerController.Player_Locking_Right)
        {
            rb.AddForce(Vector2.right * spearThrowForce_X, ForceMode2D.Impulse);
            //rb.AddTorque(-torqueForce, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.left * spearThrowForce_X, ForceMode2D.Impulse);
            //rb.AddTorque(torqueForce, ForceMode2D.Impulse);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!collided)
        {
            FaceTheDirectionTheArrowMovingInto();
            

        }
        if (collidedWithEnemy)
        {
            Instantiate(bottomSpearPrefab, bottomSpearstartpos.position, Quaternion.identity);
            Destroy(gameObject); 
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        collided = true;
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        rb.freezeRotation = true;
        //triggerCollider.enabled = false;
        gameObject.tag = "SpearToCollect";
        //if (collision.gameObject.layer == 3)
        //{

        //}

        if (collision.gameObject.CompareTag("Enemy"))
        {

            collidedWithEnemy = true;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collidedWithEnemy = true;
        }
    }
    void FaceTheDirectionTheArrowMovingInto()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }



}
