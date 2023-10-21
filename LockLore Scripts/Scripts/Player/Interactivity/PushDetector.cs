using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushDetector : MonoBehaviour
{
    [SerializeField] PlayerScript ps;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Push"))
        {
            ps.Controller.isPushing = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Push"))
        {
            ps.Controller.isPushing = false;
        }
    }
}
