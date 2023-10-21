using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingObstacle : MonoBehaviour
{
    [SerializeField] float LaunchForce;
    [SerializeField] float upForce;

    [SerializeField] int LaunchRandomValue ;
    [SerializeField] int UpRandomValue;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        float actualLaunchForce = Random.Range(LaunchForce - LaunchRandomValue, LaunchForce + LaunchRandomValue);
        float actualUpForce = Random.Range(upForce - UpRandomValue, upForce - UpRandomValue);
        rb.AddForce(actualLaunchForce * transform.forward);
        rb.AddForce(actualUpForce * transform.up);
    }
}
