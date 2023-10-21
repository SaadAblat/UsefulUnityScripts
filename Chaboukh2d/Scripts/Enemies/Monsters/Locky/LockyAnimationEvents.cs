using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockyAnimationEvents : MonoBehaviour
{

    [SerializeField] EnemyIA enemyIA;
    [SerializeField] Locky locky;


    [SerializeField] GameObject Spear;
    [SerializeField] Transform SpearPos;
    public void SetExecutingRangeAttackToFalse()
    {
        enemyIA.ExecutingRangeAttack = false;
        enemyIA.haveRangeAttack = false;
        enemyIA.haveMeleeAttack = true;
    }

    public void SetExecutingMeleeAttackToFalse()
    {
        enemyIA.ExecutingMeleeAttack = false;

    }
    public void ThrowBall()
    {
        if (enemyIA._facingRight)
        {
            GameObject tmp = Instantiate(Spear, SpearPos.position, Quaternion.identity);
            tmp.GetComponent<EnemySpear>().Initialized(Vector2.right);
        }
        else
        {
            GameObject tmp = Instantiate(Spear, SpearPos.position, Quaternion.Euler(new Vector3(0, 180, 0)));
            tmp.GetComponent<EnemySpear>().Initialized(Vector2.left);
        }
    }
}
