using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] PlayerScript ps;
    [SerializeField] GameObject AxePrefab;
    [SerializeField] Transform AxePos;
    [SerializeField] MeleeWeaponTrail MeleeWeaponTrail;

    [SerializeField] Collider Collider;

    [SerializeField] float cameraShakeIntensity;
    [SerializeField] float cameraShakeTime;
    [SerializeField] float cameraShakeFrequency;

    public void isAttackingToFalse()
    {
        ps.Controller.IsAttacking = false;
    }
    public void ComboPlayingToFalse()
    {
        ps.Controller.ComboIsPlaying = false;
       
    }
    public void ThrowAxe()
    {
        ps.Weapons.playerIsWaitingForTheAxeToReturn = true;
        switch (ps.Weapons.PlayerWeaponName)
        {
            case PlayerWeapons.PlayerWeaponList.axe:
                Instantiate(AxePrefab, AxePos.position, ps.PlayerObject.transform.rotation);
                break;
            case PlayerWeapons.PlayerWeaponList.spear:
                break;
            case PlayerWeapons.PlayerWeaponList.sword:
                break;
            case PlayerWeapons.PlayerWeaponList.none:
                break;
            default:
                break;
        }
        
    }
    public void ActivateTrail()
    {
        MeleeWeaponTrail.Emit = true;
        Collider.enabled = true;
        ps.Controller.CanRotate = false;
    }
    public void DesactivateTrail()
    {
        MeleeWeaponTrail.Emit = false;
        Collider.enabled = false;
        ps.Controller.CanRotate = true;
    }

    public void LandingEffect()
    {
        Debug.Log("Running");
        CinemachineShake.CameraInstance.ShakeCamera(cameraShakeIntensity, cameraShakeTime, cameraShakeFrequency);
    }

}
