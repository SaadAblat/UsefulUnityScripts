using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MoveObjectOnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform triggerEndPoint;
    [SerializeField] private  float speed;
    [SerializeField] private  float triggerSpeed;
    private bool moveObject;

    void Update()
    {
        if (moveObject)
        {
            target.transform.position = Vector3.Lerp(target.transform.position, endPoint.position, speed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, triggerEndPoint.position, triggerSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Axe"))
        {
            moveObject = true;
            collision.transform.parent = transform;

        }
    }


}
