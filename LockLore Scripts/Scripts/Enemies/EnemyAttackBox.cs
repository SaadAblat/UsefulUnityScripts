using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] Transform EnemyRoot;
    Collider col;

    private void Awake()
    {
         col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") )
        {
            col.enabled = false;
            other.GetComponent<IDamageable>()?.TakeDamage(damage, EnemyRoot.position);
            Debug.Log("DamageDealt");
        }
    }


}
