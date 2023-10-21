using System.Collections.Generic;
using UnityEngine;

public class ObstacleMoving : MonoBehaviour
{
    [SerializeField] List<Rigidbody> AffectedRigidBodies = new List<Rigidbody>();
    [SerializeField] float Force;



    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<Rigidbody>() != null && collider.CompareTag("Box"))
        {
            AffectedRigidBodies.Add(collider.gameObject.GetComponent<Rigidbody>());
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.GetComponent<Rigidbody>() != null && collider.CompareTag("Box"))
        {
            AffectedRigidBodies.Remove(collider.gameObject.GetComponent<Rigidbody>());

        }
    }

    void FixedUpdate()
    {
        if (AffectedRigidBodies != null && AffectedRigidBodies.Count > 0)
        {
            foreach (Rigidbody rb in AffectedRigidBodies)
            {
                if (rb != null)
                {
                    rb.AddForce(Vector3.forward * -Force);
                }
            }
        }

    }
}
