using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    AreaEffector2D areaEffector;
    private bool playerIsHere;
    [SerializeField] Animator fanAnimator;
    [SerializeField] GameObject fan;

    // Start is called before the first frame update
    void Start()
    {
        areaEffector = GetComponent<AreaEffector2D>();

    }

    // Update is called once per frame
    void Update()
    {

        if (PowerGenerator.powerGeneratorInstance != null)
        {
            if (PowerGenerator.powerGeneratorInstance.PowerActivated)
            {
                fanAnimator.Play("FanBox");
                if (PlayerScript.Instance != null)
                {
                    if (playerIsHere)
                    {
                        if (PlayerScript.Instance.playerInputs.umbrellaIsOpen)
                        {
                            areaEffector.enabled = true;
                        }
                        else
                        {
                            areaEffector.enabled = false;
                        }
                    }

                }
            }
            else
            {
                fanAnimator.Play("FanBoxOff");
                areaEffector.enabled = false;
            }
        }  
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsHere = true;
        }
    }
}
