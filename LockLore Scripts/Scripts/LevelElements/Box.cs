using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class Box : MonoBehaviour
{
    Rigidbody rb;
    ConstantForce force;
    [SerializeField] float fallMultipier;
    [SerializeField] Transform[] wallCheck;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        force = GetComponent<ConstantForce>();


    }
    private void Update()
    {
        if (rb.velocity.y < -0.05f)
        {
            force.force = new Vector3( 0  , -fallMultipier, 0 ); 
        }
        else
        {
            force.force = new Vector3(0, -0, 0);
        }
        if (Mathf.Abs(rb.velocity.x) > 1)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else if (Mathf.Abs(rb.velocity.z) >1)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }

        MoveFromWall();

    }
    [SerializeField] float moveFromWallForce;
    float timer = 0 ;

    void MoveFromWall()
    {
        bool wallDetected = false;

        foreach (Transform wallcheck in wallCheck)
        {
            if (Physics.Linecast(transform.position, wallcheck.position, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.CompareTag("Wall"))
                {
                    wallDetected = true;
                    timer += Time.deltaTime;
                    if (timer > 1.5f)
                    {
                        Vector3 direction = transform.position - wallcheck.position;
                        rb.AddForce(direction * moveFromWallForce, ForceMode.Force);
                    }
                }
            }
        }

        if (!wallDetected)
        {
            timer = 0;
        }

    }




}
