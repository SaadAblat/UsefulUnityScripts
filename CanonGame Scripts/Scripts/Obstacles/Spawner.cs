using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject[] BoxPrefabs;
    float Timer;
    [SerializeField] float timeToShoot;
    [SerializeField] float timeToShootMultiplier;
    [SerializeField] Transform Player;

    [SerializeField] float XRange;
    [SerializeField] float YRange;
    [SerializeField] float ZRange;
    Vector3 startPos;
    [SerializeField] Material material;
    [SerializeField] GameObject SpawnerNextPos;

    Vector3 newPos;
    Vector3 lookPos;
    Vector3 destinationPos;
    Quaternion destinationRot;
    Quaternion newRot;
    float destinationTimer;
    [SerializeField] float timeToChangeDestination;
    [SerializeField] float timeToChangeDestMultiplier;
    [SerializeField] float speed;
    [SerializeField] float speedMultiplier;
    [SerializeField] float MinDistanceToChangeNextPos;

    void Start()
    {
        Timer = 0;
        destinationTimer = 0;
        startPos = transform.position;
        destinationPos = startPos;
        newPos = startPos;
        newRot = transform.rotation;
        destinationRot = newRot;
    }



    void Update()
    {
        timeToShoot -= Time.deltaTime * timeToShootMultiplier;
        timeToChangeDestination -= Time.deltaTime * timeToChangeDestMultiplier;

        Timer += Time.deltaTime;
        if (Timer > timeToShoot)
        {
            Timer = 0;
            LaunchObject();
        }


        destinationTimer += Time.deltaTime;
        if (destinationTimer >= timeToChangeDestination)
        {
            destinationTimer = 0;
            destinationPos = newPos;
            destinationRot = newRot;
            FindNewPos();
            FindNewRotation();
        }

        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, destinationPos, speed * Time.deltaTime),
            Quaternion.Slerp(transform.rotation, destinationRot, speed*5 * Time.deltaTime));

        if (Vector3.Distance(transform.position, destinationPos) < MinDistanceToChangeNextPos)
        {
            SpawnerNextPos.transform.SetPositionAndRotation(newPos, newRot);
        }
    }

    void FindNewPos()
    {

        float randomXPos = Random.Range((startPos.x + XRange), (startPos.x - XRange));
        float randomYPos = Random.Range((startPos.y + YRange), (startPos.y - YRange));
        float randomZPos = Random.Range((startPos.z + ZRange), (startPos.z - ZRange));

        newPos = new Vector3(randomXPos + 5, randomYPos + 5, randomZPos + 5);
        


    }
    void FindNewRotation()
    {
        lookPos = Player.position - newPos;
        lookPos.y = 0;
        newRot = Quaternion.LookRotation(lookPos);
    }

    void LaunchObject()
    {
        int randomIndex = Random.Range(0, BoxPrefabs.Length);
        Instantiate(BoxPrefabs[randomIndex], transform.position, transform.rotation);
    }

    [SerializeField] float scoreAmountWhenHit;
    [SerializeField] float coinsAmountWhenHit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CannonBall"))
        {
            Score.GameScore += 10;
            Score.Coins += 10;
            StartCoroutine(TakeDamage());
        }
    }

    IEnumerator TakeDamage()
    {
        material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        material.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        material.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        material.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        material.color = Color.red;

    }
}
