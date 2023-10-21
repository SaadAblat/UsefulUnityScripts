using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeParentOnCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D col)
    {
        col.collider.transform.SetParent(transform);
        col.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Extrapolate;
        Debug.Log("Hey");
    }

    void OnCollisionExit2D(Collision2D col)
    {
        col.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
        col.collider.transform.parent = null;
        Debug.Log("by");

    }
}
