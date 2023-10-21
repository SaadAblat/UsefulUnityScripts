using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShiningLight : MonoBehaviour
{
    [SerializeField]  UnityEngine.Rendering.Universal.Light2D objectLight;
    [SerializeField] private float minIntensity;
    [SerializeField] private float maxIntensity;
    [SerializeField] private float lightStep;

    bool Decrease = false;

    // Update is called once per frame
    void Update()
    {
        if (objectLight.intensity >= maxIntensity)
        {
            Decrease = true;
        }
        else if (objectLight.intensity <= minIntensity)
        {
            Decrease = false;
        }

        if (Decrease)
        {
            objectLight.intensity -= lightStep * Time.deltaTime;
        }
        else
        {
            objectLight.intensity += lightStep * Time.deltaTime;
        }


    }
}
