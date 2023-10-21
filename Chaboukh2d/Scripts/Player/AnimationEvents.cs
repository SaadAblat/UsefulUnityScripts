using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [Header("Connection to the playerScript")]
    [SerializeField] private PlayerScript playerScript;

    public void HitGroundAnimationHadBeenPlayed()
    {
        playerScript.playerAnimation.hitGroundAnimationHadBeenPlayed = true;
    }
    public void StoppingAnimationHadBeenPlayed()
    {
        playerScript.playerAnimation.stoppingAnimationHadBeenPlayed = true;

    }
    public void OpeningUmbrellaAnimationHadBeenPlayed()
    {
        playerScript.playerAnimation.openingUmbrellaAnimationHadBeenPlayed = true;

    }
    public void ClosingUmbrellaAnimationHadBeenPlayed()
    {
        playerScript.playerAnimation.closingUmbrellaAnimationHadBeenPlayed = true;

    }
    public void ThrowAxe()
    {
        if (!playerScript.playerController.IsObstacleInFrontOfHimAxe())
        {
            playerScript.weapon = PlayerScript.Weapon.unarmed;
            if (playerScript.playerController.Player_Locking_Right)
            {
                Instantiate(playerScript.playerController.axePrefab, playerScript.playerController.axePos.position, Quaternion.identity);
                playerScript.playerController.axeThrown = true;

            }
            else
            {
                Instantiate(playerScript.playerController.axePrefab, playerScript.playerController.axePos.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                playerScript.playerController.axeThrown = true;


            }
    }
    AudioManager.instance.Play("ThrowAxe");

    }
    public void InteractPressedToFalse()
    {
        playerScript.playerInputs.InteractPressed = false;
    }
    public void ThrowSpear()
    {
        if (!playerScript.playerController.IsObstacleInFrontOfHimSpear())
        {
            playerScript.weapon = PlayerScript.Weapon.unarmed;
            if (playerScript.playerController.Player_Locking_Right)
            {
                Instantiate(playerScript.playerController.spearPrefab, playerScript.playerController.spearPos.position, Quaternion.identity);
            }
            else
            {
                Instantiate(playerScript.playerController.spearPrefab, playerScript.playerController.spearPos.position, Quaternion.Euler(new Vector3(0, 180, 0)));
            }
        }
        AudioManager.instance.Play("ThrowAxe");


    }

    public void ThrowingSpearAnimationµIsPlayingSetToTrue()
    {
        playerScript.PlayerRigideBody.velocity = Vector2.zero;
        playerScript.playerAnimation.throwingSpearAnimationµIsPlaying = true;
    }
    public void ThrowingSpearAnimationµIsPlayingSetToFalse()
    {
        playerScript.playerAnimation.throwingSpearAnimationµIsPlaying = false;

    }

    public void ThrowingAxeAnimationµIsPlayingSetToTrue()
    {
        playerScript.PlayerRigideBody.velocity = Vector2.zero;
        playerScript.playerAnimation.throwingAxeAnimationµIsPlaying = true;
    }
    public void ThrowingAxeAnimationµIsPlayingSetToFalse()
    {
        playerScript.playerAnimation.throwingAxeAnimationµIsPlaying = false;

    }

    public void ShootMagic()
    {
        if (playerScript.playerController.Player_Locking_Right)
        {
            Instantiate(playerScript.playerController.magicArrow, playerScript.playerController.magicArrowpos.position, Quaternion.identity);
        }
        else
        {
            Instantiate(playerScript.playerController.magicArrow, playerScript.playerController.magicArrowpos.position, Quaternion.Euler(new Vector3(0, 180, 0)));

        }
    }

    public void SpellPressedToFalse()
    {
        playerScript.playerInputs.SpellPressed = false;
    }


    public void FlipAnimationPlayedToTrue()
    {
        playerScript.playerController.charachterFlipped = false;
    }
    public void FlipAnimationPlayedToFalse()
    {
        playerScript.playerAnimation.FlipAnimationPlayed = false;
    }


    public void PlayBloodExplosion()
    {
        playerScript.playerController.BloodExplosionPrefab.Play();
    }
    public void PlayDust()
    {
        playerScript.playerController.ChangingDirDust.Play();
    }
    public void LandShake()
    {
        CinemachineShake.CameraInstance.ShakeCamera(0.3f, 0.08f, 4f);
    }


    // Sounds
    public void PlayWalkSound()
    {
        AudioManager.instance.Play("Walk");

    }
    public void PlayClimbingSound()
    {
        AudioManager.instance.Play("Climbing");


    }
    public void PlayWalkWithAxe()
    {

        AudioManager.instance.Play("WalkWithAxe");

    }

    public void LandSound()
    {
        AudioManager.instance.Play("Land");
    }

}
