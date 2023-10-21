using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swinging : MonoBehaviour
{

    [SerializeField] ConstantForce force;
    [SerializeField] float maxValue;
    float effectiveForce;
    float sign = 1;
    float timer;
    [SerializeField] float timeBeforChangingforce;


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        sign *= -1;
        if (timer > timeBeforChangingforce)
        {
            effectiveForce = sign * maxValue;
            timer = 0;
            
        }

        force.force = new Vector3(0, 0, effectiveForce);
    }
}
