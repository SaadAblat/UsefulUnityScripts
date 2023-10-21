using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpensOnTriggerHorizontal : TrapsBase
{
    [SerializeField] Transform graphic;
    [SerializeField]  float speed;
    [SerializeField]  private Transform right;
    [SerializeField]  private Transform left;
    // Start is called before the first frame update
    public override void ActivateTrapOnHold()
    {

            if (graphic.transform.position.x <= right.position.x)
            {
                graphic.transform.position += speed * Time.deltaTime * Vector3.right;
            }
        

    }
    public override void DesactivateTrapOnRelease()
    {

            if (graphic.transform.position.x >= left.position.x)
            {
                graphic.transform.position += speed/3 * Time.deltaTime * Vector3.left;
      
            }
    }
}
