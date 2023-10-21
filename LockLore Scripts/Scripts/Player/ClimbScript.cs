using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClimbScript : MonoBehaviour
{
    [SerializeField] PlayerScript ps;
    [SerializeField] float climbSpeed;

    [SerializeField] float ladderJumpForce;
    private void OnEnable()
    {
        ps.rb.isKinematic = true;
        ps.an.Play("Climb");
        ps.an.speed = Mathf.Abs(ps.Inputs.vertical);
        
    }

    private void Update()
    {
        //transform.position += climbSpeed * ps.Inputs.vertical * Time.deltaTime * transform.up;
        transform.Translate(climbSpeed * ps.Inputs.vertical * Time.deltaTime * transform.up);
        ps.an.speed = Mathf.Abs(ps.Inputs.vertical);
    }

    private void OnDisable()
    {
        ps.rb.isKinematic = false;
        ps.an.speed = 1;
    }
}
