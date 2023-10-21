using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] internal bool isActive;

    [SerializeField] bool right;
    [SerializeField] GameObject spawnWhat;
    [SerializeField] float SpawningTime;

    [SerializeField] bool spawned;
    private void Start()
    {
                StartCoroutine(SpawnerToFalse());

    }
    // Update is called once per frame
    void Update()
    {
        if (!spawned)
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        if (isActive)
        {
            if (right)
            {
                Instantiate(spawnWhat, transform.position, Quaternion.identity);
                spawned = true;
                StartCoroutine(SpawnerToFalse());

            }
            else
            {
                Instantiate(spawnWhat, transform.position, Quaternion.Euler(new Vector3(0, 180, 0)));
                spawned = true;
                StartCoroutine(SpawnerToFalse());

            }
        }
    }
    IEnumerator SpawnerToFalse()
    {
        yield return new WaitForSeconds(SpawningTime);
        spawned = false;


    }
}
