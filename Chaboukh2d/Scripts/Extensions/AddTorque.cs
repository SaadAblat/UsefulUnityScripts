using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddTorque : MonoBehaviour
{
    Rigidbody2D AxeRigideBody;
    [SerializeField] float torqueForce;
    // Start is called before the first frame update
    void Start()
    {
        AxeRigideBody = GetComponent<Rigidbody2D>();
        AxeRigideBody.AddTorque(torqueForce, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
