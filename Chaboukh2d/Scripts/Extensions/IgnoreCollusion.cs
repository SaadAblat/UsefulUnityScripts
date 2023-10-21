using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollusion : MonoBehaviour
{

    void Awake()
    {
        Physics2D.IgnoreLayerCollision(8, 9);
        Physics2D.IgnoreLayerCollision(8,10);
        Physics2D.IgnoreLayerCollision(10,10);

    }
}

