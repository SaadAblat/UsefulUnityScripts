using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyGameObjectOnTouchAfterTime : MonoBehaviour
{

    [SerializeField] string Tag;
    [SerializeField] float time;

    // Start is called before the first frame update
    void Awake()
    {
        if (Tag == null)
        {
            Tag = "Small Stones";
        }
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag))
        {
            StartCoroutine(DestroyGameObject(time, collision.gameObject));
        }
    }

    IEnumerator DestroyGameObject (float timeToDestroy, GameObject gameObjectToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObjectToDestroy);
    }
}
