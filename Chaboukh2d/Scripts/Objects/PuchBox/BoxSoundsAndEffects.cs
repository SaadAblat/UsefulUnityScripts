using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSoundsAndEffects : MonoBehaviour
{

    bool landed;
    bool touchingTheGround;

    float timer;
    float time = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        landed = true;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!touchingTheGround)
        {
            timer += Time.deltaTime;
            if (timer > time)
            {
                landed = false;
            }
        }
        if (!landed)
        {
            if (touchingTheGround)
            {
                timer = 0;
                landed = true;
                AudioManager.instance.Play("BoxImpact");
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.CompareTag("Spikes"))
        {
            touchingTheGround = true;
            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3 || collision.gameObject.CompareTag("Spikes"))
        {
            touchingTheGround = false;
        }
    }

}
