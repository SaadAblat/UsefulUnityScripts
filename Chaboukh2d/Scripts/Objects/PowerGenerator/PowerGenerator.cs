using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityStandardAssets.CrossPlatformInput;
using System;

public class PowerGenerator : MonoBehaviour
{
    public static PowerGenerator powerGeneratorInstance;
    [SerializeField] SpriteRenderer lamp;
    [SerializeField] UnityEngine.Rendering.Universal.Light2D lampLight;

    [SerializeField] bool fanExist;
    [SerializeField] bool rollingSpikesxist;
    [SerializeField] private MovingPlateform[] movingPlateforms;
    [SerializeField] private RotateSpriteTowardItself[] rotateSpriteTowardItselfScripts;

    List<float> rotationSpeedOnPower = new List<float>();


    Animator animator;

    bool FanOnSoundPlayed;

    internal bool PowerActivated;
    bool playerDetected;

    Sound fanOn;
    Sound fanOff;
    Sound fan;



    // Start is called before the first frame update
    void Awake()
    {
        powerGeneratorInstance = this;
    }

    private void Start()
    {
        if (rollingSpikesxist)
        {
            AudioManager.instance.Stop("RotationSpikes");
            foreach (MovingPlateform movingPlatform in movingPlateforms)
            {
                if (!PowerActivated)
                {
                    movingPlatform.StopAllCoroutines();
                    movingPlatform.enabled = false;
                    
                }
     
            }
            foreach(RotateSpriteTowardItself script in rotateSpriteTowardItselfScripts)
            {
                rotationSpeedOnPower.Add(script.rotationSpeed);
                if (!PowerActivated)
                {
                    script.rotationSpeed = 0;
                }
            }
        }

        PowerActivated = false;
        animator = GetComponent<Animator>();
        lamp.color = Color.red;
        lampLight.color = Color.red;
        animator.Play("PowerOff");


        fan = Array.Find(AudioManager.instance.sounds, item => item.name == "FanSound");
        fanOn = Array.Find(AudioManager.instance.sounds, item => item.name == "FanOnSound");
        fanOff = Array.Find(AudioManager.instance.sounds, item => item.name == "FanOffSound");


    }
    // Update is called once per frame
    void Update()
    {
        if (fanExist)
        {
            if (fan != null && fanOff != null && fanOn != null)
            {
                if (!fanOn.source.isPlaying && PowerActivated && !fan.source.isPlaying)
                {
                    AudioManager.instance.Play("FanSound");
                }
            }

        }
        
        if (playerDetected && (Input.GetKeyDown(KeyCode.X) || CrossPlatformInputManager.GetButtonDown("Action") ) )
        {
            PowerActivated = !PowerActivated;
            AudioManager.instance.Play("GeneratorSwitch");
            CinemachineShake.CameraInstance.ShakeCamera(0.7f, 0.4f, 6f);
            if (PowerActivated)
            {
                lamp.color = Color.green;
                lampLight.color = Color.green;
                animator.Play("PowerOn");

                if (fanExist)
                {
                    if (fanOff.source.isPlaying)
                    {
                        AudioManager.instance.Stop("FanOffSound");
                    }

                    if (!FanOnSoundPlayed)
                    {
                        AudioManager.instance.Play("FanOnSound");
                        FanOnSoundPlayed = true;
                    }
                }
                if (rollingSpikesxist)
                {
                    foreach (MovingPlateform movingPlateform in movingPlateforms)
                    {
                        movingPlateform.enabled = true;
                        AudioManager.instance.Play("RotationSpikes");
                    }
                    foreach (RotateSpriteTowardItself script in rotateSpriteTowardItselfScripts)
                    {
                        for (int i = 0; i < rotateSpriteTowardItselfScripts.Length; i++)
                        {
                            script.rotationSpeed = rotationSpeedOnPower[i];
                        }
                    }
                }
            }
            else
            {
                lamp.color = Color.red;
                lampLight.color = Color.red;
                animator.Play("PowerOff");

                if (fanExist)
                {
                    FanOnSoundPlayed = false;
                    AudioManager.instance.Play("FanOffSound");
                    if (fanOn.source.isPlaying)
                    {
                        AudioManager.instance.Stop("FanOnSound");
                    }
                    if (fan.source.isPlaying)
                    {
                        AudioManager.instance.Stop("FanSound");
                    }
                }
                if (rollingSpikesxist)
                {
                    foreach (MovingPlateform movingPlateform in movingPlateforms)
                    {
                        movingPlateform.enabled = false;
                        AudioManager.instance.Stop("RotationSpikes");
                        movingPlateform.StopAllCoroutines();
                    }
                    foreach (RotateSpriteTowardItself script in rotateSpriteTowardItselfScripts)
                    {
                        for (int i = 0; i < rotateSpriteTowardItselfScripts.Length; i++)
                        {
                            script.rotationSpeed = 0;
                        }
                    }
                }
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            playerDetected = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }
    private void OnDestroy()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Stop("RotationSpikes");
            if (fanOn.source.isPlaying)
            {
                AudioManager.instance.Stop("FanOnSound");
            }
            if (fan.source.isPlaying)
            {
                AudioManager.instance.Stop("FanSound");
            }
            if (fanOff.source.isPlaying)
            {
                AudioManager.instance.Stop("FanOffSound");
            } 
        }
    }
}
