using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHitTag : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string Tag;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag))
        {
            Destroy(gameObject);
        }
    }
}
