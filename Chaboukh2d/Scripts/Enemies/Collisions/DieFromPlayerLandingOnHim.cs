using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieFromPlayerLandingOnHim : MonoBehaviour
{
    //Collisions

    private IEnemyInterface enemyInterface;
    [SerializeField] private MonoBehaviour Enemy;


    void Start()
    {
        enemyInterface = Enemy.GetComponent<IEnemyInterface>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag("Player"))
        //{
        //    if (PlayerScript.Instance.PlayerRigideBody.velocity.y < 0)
        //    {
        //        enemyInterface.Death();
        //    }
        //}
    }
}
 