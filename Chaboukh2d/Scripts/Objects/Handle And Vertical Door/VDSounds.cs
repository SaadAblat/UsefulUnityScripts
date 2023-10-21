using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VDSounds : MonoBehaviour
{

    public void VDOpen()
    {
        AudioManager.instance.Play("VDOpen");
    }
    public void VDClose()
    {
        AudioManager.instance.Play("VDClose");
    }
    public void VDShake()
    {
        CinemachineShake.CameraInstance.ShakeCamera(0.5f, 0.3f, 3);

    }
}
