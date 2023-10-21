using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpriteTowardItself : MonoBehaviour
{
    [SerializeField] internal float rotationSpeed = 500;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}
