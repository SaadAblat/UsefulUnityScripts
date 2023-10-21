using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsLizzard : MonoBehaviour
{

    [SerializeField] EnemyIA enemyIA;
    [SerializeField] Lizzard lizzard;


    [SerializeField] GameObject BallPrefab;
    [SerializeField] Transform BallPos;


    // ANIMATION'S EVENTS :
    public void SetExecutingRangeAttackToFalse()
    {
        enemyIA.ExecutingRangeAttack = false;
    }

    public void SetExecutingMeleeAttackToFalse()
    {
        enemyIA.ExecutingMeleeAttack = false;

    }

    public void JumpAttack()
    {
        enemyIA.EnemyRigideBody.velocity = Vector2.zero;
        enemyIA.EnemyRigideBody.AddForce(Vector2.up * lizzard.attackForceY);

        if (enemyIA._facingRight)
        {
            enemyIA.EnemyRigideBody.AddForce(Vector2.right * lizzard.attackForceX);
        }
        else
        {
            enemyIA.EnemyRigideBody.AddForce(Vector2.left * lizzard.attackForceX);
        }
    }

    public void ThrowBall()
    {
        if (enemyIA._facingRight)
        {
            GameObject tmp = Instantiate(BallPrefab, BallPos.position, Quaternion.identity);
            tmp.GetComponent<Knife>().Initialized(Vector2.right);
        }
        else
        {
            GameObject tmp = Instantiate(BallPrefab, BallPos.position, Quaternion.Euler(new Vector3(0, 180, 0)));
            tmp.GetComponent<Knife>().Initialized(Vector2.left);
        }
    }

}
