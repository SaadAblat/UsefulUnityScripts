using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCameraOnHitTag : MonoBehaviour
{
    [SerializeField] List<string> ObjectsThatMakeItShake;
    [SerializeField] private float intensity;
    [SerializeField] float duration;
    [SerializeField] float frequency;
    bool hasHitTheTag;
    bool justShakedTheHellOutOfTheFCamera;
    // Update is called once per frame
    void Update()
    {
        if (hasHitTheTag && !justShakedTheHellOutOfTheFCamera)
        {
            CinemachineShake.CameraInstance.ShakeCamera(intensity, duration, frequency);
            hasHitTheTag = false;
            justShakedTheHellOutOfTheFCamera = true;

            //justShakedTheHellOutOfTheFCamera = true;
            //StartCoroutine(justShakedTheHellOutOfTheFCameraToFalse());

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(string obj in ObjectsThatMakeItShake)
        {
            //  && !justShakedTheHellOutOfTheFCamera

            if (collision.gameObject.CompareTag(obj))
            {
                hasHitTheTag = true;
            }
        }
    }

    IEnumerator justShakedTheHellOutOfTheFCameraToFalse()
    {
        yield return new WaitForSeconds(3f);
        justShakedTheHellOutOfTheFCamera = false;
    }




}
