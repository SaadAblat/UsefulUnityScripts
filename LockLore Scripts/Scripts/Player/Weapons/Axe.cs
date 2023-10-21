using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float axeThrowForce_Y;
    [SerializeField] float axeThrowForce_X;
    [SerializeField] float torqueForce;
    ConstantForce force;

    [SerializeField] Transform[] bladeChecks;
    [SerializeField] Transform bladePosStart;
    [SerializeField] LayerMask whatIsGround;


    public bool HitTheTrigger;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        force = GetComponent<ConstantForce>();
        rb.maxAngularVelocity = float.PositiveInfinity;

        rb.AddForce(transform.up * axeThrowForce_Y, ForceMode.Impulse);
        rb.AddForce(transform.forward * axeThrowForce_X, ForceMode.Impulse);
        rb.AddTorque(transform.right * torqueForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rope"))
        {
            Destroy(collision.gameObject);
        }

        if (BladeHasHit() || HitTheTrigger)
        {

            if (!rb.isKinematic)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }

            force.enabled = false;
            
            
            //transform.parent = collision.transform;
            this.enabled = false;

        }

    }


    internal bool BladeHasHit()
    {
        foreach (Transform bladeChech in bladeChecks)
        {
            if (Physics.Linecast(bladePosStart.position, bladeChech.position, whatIsGround))
            {
                return true;
            }
        }
        return false;
    }


}
