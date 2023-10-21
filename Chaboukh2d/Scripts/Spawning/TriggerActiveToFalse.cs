using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActiveToFalse : MonoBehaviour
{
    [SerializeField] Spawner spawner;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        spawner.isActive = false;
    }
}
