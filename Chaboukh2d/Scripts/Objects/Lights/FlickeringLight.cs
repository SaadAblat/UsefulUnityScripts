using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlickeringLight : MonoBehaviour
{
    [SerializeField] float minIntensity;
    [SerializeField] float maxIntensity;
    UnityEngine.Rendering.Universal.Light2D lighT;
    float lightIntensity;

    // Start is called before the first frame update
    void Start()
    {
        lighT = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lightIntensity = Random.Range(minIntensity, maxIntensity);
        lighT.intensity = lightIntensity;
    }
}
