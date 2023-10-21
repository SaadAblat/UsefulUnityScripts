using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTriggerColliderAfterTime : MonoBehaviour
{

    [SerializeField] BoxCollider2D TriggerCollider;
    [SerializeField] float time;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivateTrigger(time));
    }

   IEnumerator ActivateTrigger(float time)
    {
        yield return new WaitForSeconds(time);
        TriggerCollider.enabled = true;
    }
}
