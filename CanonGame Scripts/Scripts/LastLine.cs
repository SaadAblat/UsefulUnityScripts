using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastLine : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box"))
        {
            gameManager.GameOver = true;
        }
    }
}
