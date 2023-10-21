using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]

public class MagicArrow : MonoBehaviour
{
    public Rigidbody2D KnifeRigidbody;
    [SerializeField] private float speed;


    // Start is called before the first frame update
    void Awake()
    {
        KnifeRigidbody = GetComponent<Rigidbody2D>();
        KnifeRigidbody.gravityScale = 0;
    }

    // Update is called once per frame
    void Start()
    {
        KnifeRigidbody.velocity = new Vector2(0f, 0f);
        if (PlayerScript.Instance.playerController.Player_Locking_Right)
        {
            KnifeRigidbody.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
        }
        else
        {
            KnifeRigidbody.AddForce(Vector2.left * speed, ForceMode2D.Impulse);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
