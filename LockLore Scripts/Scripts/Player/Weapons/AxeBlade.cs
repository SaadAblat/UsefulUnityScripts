using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeBlade : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] ConstantForce force;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            rb.drag = 5;
            force.enabled = true;
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            
           
        }
    }
}
