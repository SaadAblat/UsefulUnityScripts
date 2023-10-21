using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] int damage;

    [SerializeField] float timeScaleEffect;
    [SerializeField] float timeToResetTimeScale;
    [SerializeField] Collider col;

    [SerializeField] Transform tip;

    [SerializeField] float cameraShakeIntensity;
    [SerializeField] float cameraShakeTime;
    [SerializeField] float cameraShakeFrequency;
    bool waiting;

    [SerializeField] GameObject damageTextGameObject;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (other.GetComponent<IDamageable>() != null && !other.GetComponent<IDamageable>().IsTakingDamage())
            {
                int dmg = Random.Range(damage - 2, damage + 2);
                other.GetComponent<IDamageable>().TakeDamage(dmg, transform.parent.position);

                if (!waiting)
                {
                    var damageTextRoot = Instantiate(damageTextGameObject, other.transform.position + Vector3.up * 0.5f, Quaternion.identity);
                    damageTextRoot.GetComponent<DamageText>().DamageTextComponent.text = dmg.ToString();
                    StartCoroutine(DamageEffect());
                }
            }
        }
    }

    IEnumerator DamageEffect()
    {
        CinemachineShake.CameraInstance.ShakeCamera(cameraShakeIntensity, cameraShakeTime, cameraShakeFrequency);
        waiting = true;
        Time.timeScale = timeScaleEffect;

        yield return new WaitForSecondsRealtime(timeToResetTimeScale);
        Time.timeScale = 1;
        waiting = false;
    }


    //var damageTextRoot = Instantiate(damageTextGameObject, other.transform.position + Vector3.up * 0.5f, Quaternion.identity);
    //damageTextRoot.GetComponent<DamageText>().DamageTextComponent.text = dmg.ToString();
    //StartCoroutine(DamageEffect());

    //IEnumerator DamageEffect()
    //{
    //    CinemachineShake.CameraInstance.ShakeCamera(cameraShakeIntensity, cameraShakeTime, cameraShakeFrequency);
    //    waiting = true;
    //    Time.timeScale = timeScaleEffect;

    //    yield return new WaitForSecondsRealtime(timeToResetTimeScale);
    //    Time.timeScale = 1;
    //    waiting = false;

    //}
}
