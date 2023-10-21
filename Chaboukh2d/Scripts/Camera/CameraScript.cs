using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]  public SpriteRenderer rink;
    // Start is called before the first frame update
    void Start()
    {
        float orthoSize = rink.bounds.size.x * Screen.height / Screen.width * 0.5f;
        //float orthoSize = rink.bounds.size.y / 2;
        CinemachineShake.CameraInstance.cinemaMachineVirtualCamera.m_Lens.OrthographicSize = orthoSize;
    }

}
