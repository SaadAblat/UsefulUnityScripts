using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawningAfterFalling : MonoBehaviour
{
    [SerializeField] Transform spawnPos;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = spawnPos.position;
        }
    }
}
