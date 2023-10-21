using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locky : MonoBehaviour, IEnemyInterface
{
    [SerializeField] EnemyIA enemyIA;

    [SerializeField] Animator lAnim;

    [SerializeField] CapsuleCollider2D lizzardCol;

    [SerializeField] internal float attackForceX;
    [SerializeField] internal float attackForceY;


    const string RANGE_ATTACK = "RangeAttack";
    const string MELEE_ATTACK = "MeleeAttack";
    const string WALK = "Walk";
    const string WALK_SPEAR = "WalkWithSpear";
    const string DEATH = "Death";

    bool enemyIsDead;
    private void Update()
    {
        if (!enemyIsDead)
        {
            if (!enemyIA.ExecutingMeleeAttack && !enemyIA.ExecutingRangeAttack)
            {
                if (enemyIA.haveRangeAttack)
                {
                    lAnim.Play(WALK_SPEAR);
                }
                else
                {
                    lAnim.Play(WALK);
                }
            }
        }

    }
    public void Death()
    {
        lizzardCol.enabled = false;
        enemyIsDead = true;
        enemyIA.state = EnemyIA.EnemyState.Death;
        enemyIA.EnemyRigideBody.velocity = Vector2.zero;
        //enemyIA.EnemyRigideBody.isKinematic = true;
        enemyIA.EnemyRigideBody.drag = 1;
        PlayerScript.Instance.playerController.killBounce = true;
        lAnim.Play(DEATH);


    }


    public void MeleeAttack()
    {
        enemyIA.ExecutingMeleeAttack = true;
        lAnim.Play(MELEE_ATTACK);


    }

    public void RangeAttack()
    {
        if (!enemyIA.ExecutingRangeAttack)
        {
            enemyIA.ExecutingRangeAttack = true;
            lAnim.Play(RANGE_ATTACK);

        }
    }


}