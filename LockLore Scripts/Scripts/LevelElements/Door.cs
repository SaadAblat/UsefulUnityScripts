using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        animator.Play("Armature|Close");
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            if (collision.gameObject.GetComponent<PlayerScript>() != null)
            {
                PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
                if (player.hasTheKey)
                {
                    animator.Play("Armature|Open");
                    player.hasOpenedTheDoor = true;
                }
            }
        }
    }
}
