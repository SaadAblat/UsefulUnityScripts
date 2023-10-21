using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlickeringLightWithMoreOptions : MonoBehaviour
{
    [SerializeField] private TeleportPlayer portal;
    [SerializeField] private float minIntensity;
    [SerializeField] float maxIntensity;
    [SerializeField] float minInnerRadius;
    [SerializeField] float maxInnerRadius;
    [SerializeField] float minOuterRadius;
    [SerializeField] float maxOuterRadius;
    UnityEngine.Rendering.Universal.Light2D lighT;
    float lightInnerRadius;
    float lightOuterRadius;
    float lightIntensity;

    

    // Start is called before the first frame update
    void Start()
    {
        lighT = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!portal.collidedWithSomething)
        {
            lightIntensity = Random.Range(minIntensity, maxIntensity);
            lighT.intensity = lightIntensity;

            lightInnerRadius = Random.Range(minInnerRadius, maxInnerRadius);
            lighT.pointLightInnerRadius = lightInnerRadius;

            lightOuterRadius = Random.Range(minOuterRadius, maxOuterRadius);
            lighT.pointLightOuterRadius = lightOuterRadius; 
        }
        else
        {
            lighT.intensity = maxIntensity * 1.5f;
            lighT.pointLightInnerRadius = maxInnerRadius * 1.5f;
            lighT.pointLightOuterRadius = maxOuterRadius * 1.5f;
        }
    }
}
