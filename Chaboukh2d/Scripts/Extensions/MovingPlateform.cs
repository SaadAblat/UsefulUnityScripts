using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlateform : MonoBehaviour
{
    public GameObject platformPathStart;
    public GameObject platformPathEnd;
    [SerializeField] private float speed;
    private Vector3 startPosition;
    private Vector3 endPosition;

    private void OnEnable()
    {
        startPosition = platformPathStart.transform.position;
        endPosition = platformPathEnd.transform.position;
        StartCoroutine(Vector3LerpCoroutine(gameObject, endPosition, speed));
    }
    void Start()
    {
        startPosition = platformPathStart.transform.position;
        endPosition = platformPathEnd.transform.position;

        StartCoroutine(Vector3LerpCoroutine(gameObject, endPosition, speed));
        
    }
    // Update is called once per frame
    IEnumerator Vector3LerpCoroutine(GameObject obj, Vector3 target, float speed)
    {
        Vector3 startPosition = obj.transform.position;
        float time = 0f;

        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.Lerp(startPosition, target, (time / Vector3.Distance(startPosition, target)) * speed);
            time += Time.deltaTime;
            yield return null;
        }
    }
    void FixedUpdate()
    {
        if (transform.position == endPosition)
        {
            StartCoroutine(Vector3LerpCoroutine(gameObject, startPosition, speed));
        }
        if (transform.position == startPosition)
        {
            StartCoroutine(Vector3LerpCoroutine(gameObject, endPosition, speed));
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        col.collider.transform.SetParent(transform);
        col.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Extrapolate;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        col.gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
        col.collider.transform.parent = null;
    }


}


