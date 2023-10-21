using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggersThatConsumeAxe : MonoBehaviour
{
    PlayerScript ps;
    [SerializeField] GameObject axeVisualOnly;
    private void Awake()
    {
        ps = FindObjectOfType<PlayerScript>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Axe"))
        {
            ps.Weapons.PlayerHaveTheAxe = false;
            GameObject newAxe = Instantiate(axeVisualOnly, collision.transform.position, collision.transform.rotation);
            newAxe.transform.parent = transform;
            StartCoroutine(DestroyAxe(collision.gameObject));
        }
    }

    IEnumerator DestroyAxe(GameObject axe)
    {
        yield return new WaitForSeconds(0.01f);
        Destroy(axe);
    }
}
