using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLayerWhenPlayerLand : MonoBehaviour
{
    private void Update()
    {

        if (PlayerScript.Instance.playerAnimation.PlayerCurrentAnimationState == "LandEntry")
        {
            gameObject.layer = LayerMask.NameToLayer("Ground");
        }
        if (PlayerScript.Instance.playerAnimation.PlayerCurrentAnimationState == "Jump" || PlayerScript.Instance.playerAnimation.PlayerCurrentAnimationState == "Jump2")
        {
            gameObject.layer = LayerMask.NameToLayer("Default");

        }

    }

}
