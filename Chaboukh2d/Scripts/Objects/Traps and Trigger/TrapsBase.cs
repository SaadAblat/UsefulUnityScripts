using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsBase : MonoBehaviour
{
    [SerializeField] TrapsTrigger trigger;


    private void Update()
    {
        BehaviorOnUpdate();
        if (trigger.TriggerActivated)
        {
            ActivateTrap();
        }
        if (trigger.TriggerDown)
        {
            ActivateTrapOnHold();
        }
        else
        {
            DesactivateTrapOnRelease();
        }
    }
    /// <summary>
    /// Activate traps once
    /// </summary>
    public virtual void ActivateTrap()
    {
        trigger.TriggerActivated = false;
    }
    /// <summary>
    /// Activate traps while trigger is down
    /// </summary>
    public virtual void ActivateTrapOnHold()
    {

    }
    public virtual void DesactivateTrapOnRelease()
    {

    }

    public virtual void BehaviorOnUpdate()
    {

    }

  


}
