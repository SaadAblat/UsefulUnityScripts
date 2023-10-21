using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    public bool PortalIsActive;
    [SerializeField] Transform destination;
    [SerializeField] TeleportPlayer otherPortal;

    internal bool collidedWithSomething;

    // Start is called before the first frame update
    void Awake()
    {
        PortalIsActive = true;
       
    }
    private void Start()
    {
        collidedWithSomething = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collidedWithSomething = true;
        if (PortalIsActive)
        {
            AudioManager.instance.Play("PortalEntry");
            CinemachineShake.CameraInstance.ShakeCamera(0.8f, 0.3f, 5f);
            StartCoroutine(Teleport(collision));
            //collision.gameObject.transform.position = GetDestination().position;
            //otherPortal.PortalIsActive = false;
        }
    }
    void OnTriggerExit2D (Collider2D collision)
    {
        collidedWithSomething = false;
        PortalIsActive = true;
    }
    Transform GetDestination()
    {
        return destination;
    }

    IEnumerator Teleport(Collider2D subject)
    {
        if (subject != null)
        {
            if (subject.attachedRigidbody != null)
            {
                subject.attachedRigidbody.velocity = Vector2.zero;
                subject.attachedRigidbody.gravityScale = 0;
            }
        }
        yield return new WaitForSeconds(0.3f);
        if (subject != null)
        {
            subject.gameObject.transform.position = GetDestination().position;
            otherPortal.PortalIsActive = false;
            if (subject.attachedRigidbody != null)
            {
                subject.attachedRigidbody.velocity = Vector2.zero;
                subject.attachedRigidbody.gravityScale = 1;
            }
        }
        
    }
}
