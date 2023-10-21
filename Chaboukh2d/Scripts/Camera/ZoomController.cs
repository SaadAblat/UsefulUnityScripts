using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomController : MonoBehaviour
{
    bool zoomedOut;
    [SerializeField] Transform centre;

    private void Start()
    {
        CinemachineShake.CameraInstance.cinemaMachineVirtualCamera.m_Lens.OrthographicSize = 5;
        CinemachineShake.CameraInstance.cinemaMachineVirtualCamera.Follow = centre;
        zoomedOut = false;
    }
    public void ChangeZoom()
    {
        if (PlayerScript.Instance != null)
        {
            if (!zoomedOut)
            {
                //CinemachineShake.CameraInstance.cinemaMachineVirtualCamera.m_Lens.OrthographicSize = 4.95f;
                StopAllCoroutines();
                StartCoroutine(ZoomOut(0f));
                CinemachineShake.CameraInstance.cinemaMachineVirtualCamera.Follow = centre;
                zoomedOut = true;
            }
            else
            {
                //CinemachineShake.CameraInstance.cinemaMachineVirtualCamera.m_Lens.OrthographicSize = 3;
                StopAllCoroutines();
                StartCoroutine(ZoomIn(0f));
                CinemachineShake.CameraInstance.cinemaMachineVirtualCamera.Follow = PlayerScript.Instance.transform;
                zoomedOut = false;
            } 
        }
        
    }

    IEnumerator ZoomIn(float time)
    {
        CinemachineShake.CameraInstance.cinemaMachineVirtualCamera.m_Lens.OrthographicSize = 3;
        yield return new WaitForSeconds(time);
        

    }
    IEnumerator ZoomOut(float time)
    {
        CinemachineShake.CameraInstance.cinemaMachineVirtualCamera.m_Lens.OrthographicSize = 4.95f;
        yield return new WaitForSeconds(time);
        
    }
}
