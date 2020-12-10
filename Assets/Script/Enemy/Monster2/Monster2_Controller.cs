using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster2_Controller : MonoBehaviour
{
    private Enemy Enemy;
    private EnemyAI EnemyAI;
    private Monster2_Animation Monster2_Animation;
    private float AttackRandom;
    private bool isRetreat, isCurrentRetreat;

    //Get all component
    private void Awake()
    {
        Enemy = GetComponent<Enemy>();
        EnemyAI = GetComponent<EnemyAI>();
        Monster2_Animation = GetComponent<Monster2_Animation>();
    }
    private void Update()
    {
        if(EnemyAI.GetPlayerInRange == true && Enemy.GetisDeath == false)
        {
            AttackPatternManager();
        }
        else if(Enemy.GetisDeath == true)
        {
            this.enabled = false;
        }

        if(EnemyAI.GetIsGoToPlayer == true)
        {
            Monster2_Animation.WalkToPlayer();
        }
        else if(EnemyAI.GetisGetAwayFromPlayer == true)
        {
            Monster2_Animation.WalkAwayFromPlayer();
        }
        else if(EnemyAI.GetIsGoToPlayer == false && EnemyAI.GetisGetAwayFromPlayer == false)
        {
            Monster2_Animation.StandStill();
        }
    }

    void AttackPatternManager()
    {
        if(Enemy.GetEnemyHpBar.value > (Enemy.GetEnemyMaxHp / 2))
        {
            if(EnemyAI.GetDistanceBetweenEnemyAndPlayer > EnemyAI.GetAttackDis && Enemy.GetisDamage == false && Monster2_Animation.GetIsEnemyAttack == false)
            {
                EnemyAI.GoToPlayer();
            }
            else if(EnemyAI.GetDistanceBetweenEnemyAndPlayer < EnemyAI.GetAttackDis && Enemy.GetisDamage == false && Monster2_Animation.GetIsEnemyAttack == false)
            {
                EnemyAI.StandStill();
                Monster2_Animation.M2_AttackPatManager();
            }
        }
        else if(Enemy.GetEnemyHpBar.value <= (Enemy.GetEnemyMaxHp / 2))
        {
            if(isRetreat == false)
            {
                isRetreat = true;
                isCurrentRetreat = false;
                AttackRandom = Random.value;
            }

            if(AttackRandom > 0.3f)
            {
                if(EnemyAI.GetDistanceBetweenEnemyAndPlayer > EnemyAI.GetAttackDis && Enemy.GetisDamage == false && Monster2_Animation.GetIsEnemyAttack == false)
                {
                    EnemyAI.GoToPlayer();
                }
                else if(EnemyAI.GetDistanceBetweenEnemyAndPlayer < EnemyAI.GetAttackDis && Enemy.GetisDamage == false && Monster2_Animation.GetIsEnemyAttack == false)
                {
                    EnemyAI.StandStill();
                    Monster2_Animation.M2_AttackPatManager();
                }

                if(isCurrentRetreat == false)
                {
                    isCurrentRetreat = true;
                    Invoke("ResetRetreat",1.5f);
                }
            }
            else if(AttackRandom <= 0.3f)
            {
                EnemyAI.GetAwayFromPlayer();

                if(isCurrentRetreat == false)
                {
                    isCurrentRetreat = true;
                    Invoke("ResetRetreat",2f);
                }
            }
        }
    }
    void ResetRetreat()
    {
        print("Reset");
        isRetreat = false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,EnemyAI.GetAttackDis);
    }
}
