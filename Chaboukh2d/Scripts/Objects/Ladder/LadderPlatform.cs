using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class LadderPlatform : MonoBehaviour
{
    float Verticali;
    Collider2D col;


    private void Start()
    {
        col = GetComponent<Collider2D>();
        Verticali = PlayerScript.Instance.playerInputs.Vertical;
    }
    private void Update()
    {
        if (Verticali < 0)
        {
            col.enabled = false;
        }
        else col.enabled = true ;
    }
}
