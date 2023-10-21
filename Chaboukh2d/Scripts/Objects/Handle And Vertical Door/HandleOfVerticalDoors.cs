using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityStandardAssets.CrossPlatformInput;
using Cinemachine;

public class HandleOfVerticalDoors : MonoBehaviour
{
    private GameObject targetGroup;
    bool on;
    bool PlayerDetected;
    [SerializeField] GameObject[] verticalDoors;
    [SerializeField] Animator[] vDoorAnimators;
    [SerializeField] Animator HandleAnimator;
    
    bool firstTime = true;
    UnityEngine.Rendering.Universal.Light2D handleLight;
    private bool interactionLightPlayed;

    private void Start()
    {
        if (targetGroup == null)
        {
            targetGroup = new GameObject();
            targetGroup.AddComponent<CinemachineTargetGroup>();
        }

        on = false;
        HandleAnimator.Play("Off");
        foreach (Animator animator in vDoorAnimators)
        {
            animator.Play("Idle");
        }
        foreach (GameObject door in verticalDoors)
        {
            targetGroup.GetComponent<CinemachineTargetGroup>().AddMember(door.transform, 1, 1);
        }

        handleLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();




    }
    private void Update()
    {
        if (HandleAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f && !interactionLightPlayed && PlayerDetected)
        {
            StartCoroutine(addLightIntesityOnInteraction(0.1f, 0.3f));
            interactionLightPlayed = true;
        }
        else
        {
            interactionLightPlayed = false;
        }
        if (!firstTime)
        {
            if (on)
            {
                HandleAnimator.Play("On");
                StartCoroutine(openTheDoors());
            }
            else
            {
                HandleAnimator.Play("Off");
                StartCoroutine(closeTheDoors());
            }
        }

        if ((Input.GetKeyDown(KeyCode.X) || CrossPlatformInputManager.GetButtonDown("Action") ) && PlayerDetected)
        {
            on = !on;
            StartCoroutine(LookAtTheDoors(1.5f));
            if (on)
            {
                AudioManager.instance.Play("HandleOn");
            }
            else
            {
                AudioManager.instance.Play("HandleOff");
            }
            if (firstTime)
            {
                firstTime = false;
            }  
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerDetected = true;
            handleLight.intensity += 0.5f;
            
            
            // Show A message saying press X to interact (optional)
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerDetected = false;
            handleLight.intensity -= 0.5f;
            // Show A message saying press X to interact (optional)
        }
    }

    IEnumerator openTheDoors()
    {
        yield return new WaitForSeconds(0.2f);
        foreach (Animator animator in vDoorAnimators)
        {
            animator.Play("Open");
        }
        foreach (GameObject gameObject in verticalDoors)
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        }
    }
    IEnumerator closeTheDoors()
    {
        yield return new WaitForSeconds(0.2f);
        foreach (Animator animator in vDoorAnimators)
        {
            animator.Play("Close");
        }
        foreach (GameObject gameObject in verticalDoors)
        {
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        }
        
    }
    IEnumerator addLightIntesityOnInteraction(float time, float lightIntensity)
    {
        handleLight.intensity += lightIntensity;
        yield return new WaitForSeconds(time);
        handleLight.intensity -= lightIntensity;

    }
    IEnumerator LookAtTheDoors(float time)
    {
        CinemachineShake.CameraInstance.cinemaMachineVirtualCamera.Follow = targetGroup.transform;
        yield return new WaitForSeconds(time);
        if (PlayerScript.Instance.gameObject != null)
        {
            CinemachineShake.CameraInstance.cinemaMachineVirtualCamera.Follow = PlayerScript.Instance.gameObject.transform;
        }

    }

}
