using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapsTrigger : MonoBehaviour
{
    internal bool TriggerActivated = false;
    internal bool TriggerDown = false;
    [SerializeField] Transform graphic;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("RollingSpikes") && !collision.CompareTag("Enemy"))
        {
            TriggerActivated = true;

            if (!TriggerDown)
            {
                TriggerDown = true;
                graphic.Translate(0, -0.1f, 0);
            } 
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("RollingSpikes") && !collision.CompareTag("Enemy"))
        {
            if (TriggerDown)
            {
                TriggerDown = false;
                graphic.Translate(0, 0.1f, 0);
            } 
        }
    }

}


