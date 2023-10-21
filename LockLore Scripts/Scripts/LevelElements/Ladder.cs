using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[SelectionBase]
public class Ladder : MonoBehaviour
{
    [SerializeField] Transform pos;
    bool isClimbing;
    GameObject player;
    [SerializeField] Transform top;
    [SerializeField] Transform bottom;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerInputs>() != null)
        {
            if (other.GetComponent<PlayerInputs>().Interact)
            {
                player = other.gameObject;
                isClimbing = !isClimbing;
                SwitchScripts();

                
                other.GetComponent<PlayerInputs>().Interact = false;
            }
        }
    }

    void SetPositionAndRotation()
    {
        if (player != null)
        {
            if (player.transform.position.y <= top.position.y)
            {
                player.transform.position = new Vector3(pos.position.x, player.transform.position.y, pos.position.z);
            }
            else
            {
                if (player.GetComponent<PlayerInputs>().vertical <= 0)
                {
                    player.transform.position = new Vector3(pos.position.x, top.position.y - 0.2f, pos.position.z);
                }
                
            }
            
            player.GetComponent<PlayerScript>().PlayerObject.transform.rotation = transform.rotation;
        }
    }
    void SwitchScripts()
    {
        player.GetComponent<PlayerScript>().Controller.enabled = !isClimbing;
        player.GetComponent<PlayerScript>().ClimbController.enabled = isClimbing;
    }

    [SerializeField] float forwardForce;
    private void Update()
    {
      

        if (isClimbing)
        {
            SetPositionAndRotation();
        }
        if (player != null)
        {

            if (isClimbing)
            {
                PlayerScript ps = player.GetComponent<PlayerScript>();
                if (ps.Inputs.jumpRequested)
                {
                    isClimbing = false;
                    SwitchScripts();
                }
                if (player.transform.position.y > top.position.y)
                {
                    isClimbing = false;
                    SwitchScripts();
                    
                    ps.rb.AddForce(ps.PlayerObject.transform.forward * forwardForce * Time.deltaTime, ForceMode.Impulse);

                }
                if (player.transform.position.y < bottom.position.y)
                {
                    isClimbing = false;
                    SwitchScripts();
                }
            }
        }
    }

}
