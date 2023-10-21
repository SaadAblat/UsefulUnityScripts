using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizzard : MonoBehaviour, IEnemyInterface
{
    [SerializeField] EnemyIA enemyIA;

    [SerializeField] Animator lAnim;

    [SerializeField] CapsuleCollider2D lizzardCol;

    [SerializeField] internal float attackForceX;
    [SerializeField] internal float attackForceY;


    const string RANGE_ATTACK = "RangeAttack";
    const string MELEE_ATTACK = "MeleeAttack";
    const string WALK = "Walk";
    const string DEATH = "Death";

    bool enemyIsDead;
    private void Update()
    {
        if (!enemyIA.ExecutingMeleeAttack && !enemyIA.ExecutingRangeAttack && !enemyIsDead)
        {
            lAnim.Play(WALK);

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
        enemyIA.ExecutingRangeAttack = true;
        lAnim.Play(RANGE_ATTACK);
    }


}
