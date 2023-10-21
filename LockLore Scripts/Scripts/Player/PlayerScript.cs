using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IDamageable
{
    public PlayerInputs Inputs;
    public PlayerController Controller;
    public PlayerWeapons Weapons;
    public ClimbScript ClimbController;
    public Rigidbody rb;
    public Animator an;
    public GameObject PlayerObject;
    public bool isInWater;

    public bool hasTheKey;
    public bool hasOpenedTheDoor;
    public bool IsDead;

    public float Health;

    public bool IsTakingDamage;
    [SerializeField] Renderer bodyRender;
    [SerializeField] Material hitMaterial;
    private Material[] originalMaterials;
    [SerializeField] float DamageForce;
    [SerializeField] float AttackForce;


    private Vector3 targetPosition;
    [SerializeField] private float smoothSpeed;

    public bool attackPushActive;
    [SerializeField] float atackPushTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        originalMaterials = bodyRender.materials;
    }
    private void FixedUpdate()
    {
        if (damagePushActive)
        {
            rb.velocity = Vector3.zero;
            targetPosition = transform.position + DamageForce * damageForceDir.normalized;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }
        if (attackPushActive )
        {
            targetPosition = transform.position + AttackForce * PlayerObject.transform.forward;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            Invoke(nameof(attackPushToFalse), atackPushTime);

        }
    }
    private void Update()
    {
        if (Health <= 0)
        {
            Death();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            hasTheKey = true;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Trap"))
        {
            Death();
        }
    }

    void attackPushToFalse()
    {
        attackPushActive = false;  
    }

    private void Death()
    {
        IsDead = true;
        Controller.enabled = false;
        Inputs.enabled = false;
        ClimbController.enabled = false;
        an.Play("Death");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 4)
        {
            isInWater = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 4)
        {
            isInWater = false;
        }
    }

    Vector3 damageForceDir;
    bool damagePushActive;
    public void TakeDamage(float amount, Vector3 attackerPosition)
    {
        Health -= amount;
        IsTakingDamage = true;
        damageForceDir =  PlayerObject.transform.position - attackerPosition;
        damageForceDir.y = 0;


        damagePushActive = true;

        StartCoroutine(DamageEffect());

    }

    [SerializeField] float damageCameraShakeIntensity, damagecameraShakeTime, damagecameraShakeFrequency;
    [SerializeField] float timeScaleEffect, timeToResetTimeScale;

    IEnumerator DamageEffect()
    {
        CinemachineShake.CameraInstance.ShakeCamera(damageCameraShakeIntensity, damagecameraShakeTime, damagecameraShakeFrequency);
        Inputs.enabled = false;
        Material[] newMaterials = new Material[originalMaterials.Length];
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            newMaterials[i] = hitMaterial;
        }
        bodyRender.materials = newMaterials;
        yield return new WaitForSecondsRealtime(0.15f);
        IsTakingDamage = false;
        bodyRender.materials = originalMaterials;
        Inputs.enabled = true;
        damagePushActive = false;
    }
    IEnumerator DamageFreezeTime()
    {
        Time.timeScale = timeScaleEffect;
        yield return new WaitForSecondsRealtime(timeToResetTimeScale);
        Time.timeScale = 1;
    }

    bool IDamageable.IsTakingDamage()
    {
        return IsTakingDamage;
    }
}
