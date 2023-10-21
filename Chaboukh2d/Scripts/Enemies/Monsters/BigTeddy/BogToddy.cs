using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BogToddy : MonoBehaviour, IEnemyInterface
{
    [SerializeField] EnemyIA enemyIA;

    [SerializeField] Animator lAnim;

    [SerializeField] CapsuleCollider2D lizzardCol;

    [SerializeField] internal float attackForceX;
    [SerializeField] internal float attackForceY;


    const string WALK = "Walk";
    const string IDLE = "Idle";

    bool enemyIsDead;
    private void Update()
    {
        lAnim.SetFloat("speed", Mathf.Abs(enemyIA.EnemyRigideBody.velocity.x));
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


    }


    public void MeleeAttack()
    {
        enemyIA.ExecutingMeleeAttack = true;


    }

    public void RangeAttack()
    {
        enemyIA.ExecutingRangeAttack = true;
    }


}

