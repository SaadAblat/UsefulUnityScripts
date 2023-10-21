using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    internal string PlayerCurrentAnimationState;
    Animator doorAnimator;

    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = GetComponent<Animator>();
        ChangeAnimationState("Closed");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerScript.Instance.OpenedTheDoor)
        {
            ChangeAnimationState("DoorOpening");
        }
    }
    private void ChangeAnimationState(string newState)
    {
        if (PlayerCurrentAnimationState == newState)
            return;
        doorAnimator.Play(newState);
        PlayerCurrentAnimationState = newState;
    }
}
