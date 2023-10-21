using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    Rigidbody2D AxeRigideBody;
    [SerializeField] float axeThrowForceç_Y;
    [SerializeField] float axeThrowForceç_X;
    [SerializeField] float torqueForce;
    [SerializeField] float timeToChangeIntoAxeTag;
    [SerializeField] Collider2D ColSolid;
    [SerializeField] Collider2D ColTrigger;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivateCols());
        AxeRigideBody = GetComponent<Rigidbody2D>();
        AxeRigideBody.velocity = new Vector2(0f, 0f);
        AxeRigideBody.AddForce(Vector2.up * axeThrowForceç_Y, ForceMode2D.Impulse);
        
        if (PlayerScript.Instance.playerController.Player_Locking_Right)
        {
            AxeRigideBody.AddForce(Vector2.right * axeThrowForceç_X, ForceMode2D.Impulse);
            AxeRigideBody.AddTorque(-torqueForce, ForceMode2D.Impulse);
        }
        else
        {
            AxeRigideBody.AddForce(Vector2.left * axeThrowForceç_X, ForceMode2D.Impulse);
            AxeRigideBody.AddTorque(torqueForce, ForceMode2D.Impulse);

        }
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rope"))
        {
            Destroy(collision.gameObject);
            //if (!AudioManager.instance.sounds[14].source.isPlaying)
            //{
            //    AudioManager.instance.Play("AxeImpactRope");
            //}
            AudioManager.instance.Play("AxeImpactRope");
        }
        if (collision.CompareTag("All The Ropes"))
        {
            if (collision.gameObject != null)
            {
                StartCoroutine(DestroyAllRopes(collision));
                if (!AudioManager.instance.sounds[14].source.isPlaying)
                {
                    AudioManager.instance.Play("AxeImpactRope");
                }
            }
        }
    }

    IEnumerator DestroyAllRopes(Collider2D collision)
    {
        yield return new WaitForSeconds(0.5f);
        if (collision.gameObject != null)
                {
            collision.gameObject.SetActive(false);

                }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Rope") || collision.gameObject.CompareTag("All The Ropes"))
        {
            //if (!AudioManager.instance.sounds[14].source.isPlaying)
            //{
            //    AudioManager.instance.Play("AxeImpactRope");
            //}
            AudioManager.instance.Play("AxeImpactRope");
        }
        else
        {
            if (!AudioManager.instance.sounds[13].source.isPlaying)
            {
                AudioManager.instance.Play("AxeImpact");
            }
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ActivateCols()
    {
        yield return new WaitForSeconds(timeToChangeIntoAxeTag);
        ColSolid.enabled = true;
        ColTrigger.enabled = true;
    }
}
