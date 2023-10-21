using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpensOnTrigger : TrapsBase
{
    [SerializeField] Transform graphic;
    [SerializeField] private float speedUp;
    [SerializeField] private float speedDown;
    [SerializeField]  private Transform up;
    [SerializeField]  private Transform down;
    // Start is called before the first frame update
    public override void ActivateTrapOnHold()
    {

            if (graphic.transform.position.y <= up.position.y)
            {
                graphic.transform.position += speedUp * Time.deltaTime * Vector3.up;
            }
        

    }
    public override void DesactivateTrapOnRelease()
    {

            if (graphic.transform.position.y >= down.position.y)
            {
                graphic.transform.position += speedDown * Time.deltaTime * Vector3.down;
      
            }
    }


}
