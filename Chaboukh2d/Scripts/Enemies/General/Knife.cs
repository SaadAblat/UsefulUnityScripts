using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float ThrowForceç_Y;
    [SerializeField] float ThrowForceç_X;
    [SerializeField] float torqueForce;

    private float KnifeTimer;
    [SerializeField] private float knifeLifespan = 3f;

    private Vector2 direction;
    private bool startTimer = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, 0f);

        rb.AddForce(Vector2.up * ThrowForceç_Y, ForceMode2D.Impulse);
        rb.AddForce(direction * ThrowForceç_X, ForceMode2D.Impulse);
        rb.AddTorque(-torqueForce, ForceMode2D.Impulse);

        


    }
    private void Update()
    {
        if (startTimer)
        {
            KNIFETimer(); 
        }
    }

    public void Initialized(Vector2 direction)
    {
        this.direction = direction;
    }

    private void KNIFETimer()
    {
        KnifeTimer += Time.deltaTime;
        if (KnifeTimer > knifeLifespan)
        {
            Destroy(gameObject);
            KnifeTimer = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        startTimer = true;
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
