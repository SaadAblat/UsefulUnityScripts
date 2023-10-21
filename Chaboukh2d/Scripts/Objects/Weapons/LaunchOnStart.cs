using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchOnStart : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float ThrowForce�_Y;
    [SerializeField] private float ThrowForce�_X;
    [SerializeField] float torqueForce;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0f, 0f);
        rb.AddForce(Vector2.up * ThrowForce�_Y, ForceMode2D.Impulse);

        if (PlayerScript.Instance.playerController.Player_Locking_Right)
        {
            rb.AddForce(Vector2.right * ThrowForce�_X, ForceMode2D.Impulse);
            rb.AddTorque(-torqueForce, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.left * ThrowForce�_X, ForceMode2D.Impulse);
            rb.AddTorque(torqueForce, ForceMode2D.Impulse);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
