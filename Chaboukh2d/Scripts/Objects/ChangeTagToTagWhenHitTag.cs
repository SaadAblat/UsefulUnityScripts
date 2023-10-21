using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTagToTagWhenHitTag : MonoBehaviour
{
    [SerializeField] string colliderTag;
    [SerializeField] string newTag;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(colliderTag))
        {
            gameObject.tag = newTag;
        }
    }
}
