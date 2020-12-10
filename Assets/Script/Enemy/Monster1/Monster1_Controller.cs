using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1_Controller : MonoBehaviour
{
    #region private variable
    private Monster1_Animation Monster1_Animation;
    private float AttackRandom;
    private bool isRetreat, isCurrentRetreat;
    #endregion

    #region SerializeField variable
    [SerializeField] private Enemy Enemy;
    [SerializeField] private EnemyAI EnemyAI;
    #endregion
    
    //Get all component
    private void Awake()
    {
        Monster1_Animation = GetComponent<Monster1_Animation>();    
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
            Monster1_Animation.WalkToPlayer();
        }
        else if(EnemyAI.GetisGetAwayFromPlayer == true)
        {
            Monster1_Animation.WalkAwayFromPlayer();
        }
        else if(EnemyAI.GetIsGoToPlayer == false && EnemyAI.GetisGetAwayFromPlayer == false)
        {
            Monster1_Animation.StandStill();
        }
    }
    void AttackPatternManager()
    {
        if(Enemy.GetEnemyHpBar.value > (Enemy.GetEnemyMaxHp / 2))
        {
            if(EnemyAI.GetDistanceBetweenEnemyAndPlayer > EnemyAI.GetAttackDis && Enemy.GetisDamage == false && Monster1_Animation.GetIsEnemyAttack == false)
            {
                EnemyAI.GoToPlayer();
            }
            else if(EnemyAI.GetDistanceBetweenEnemyAndPlayer < EnemyAI.GetAttackDis && Enemy.GetisDamage == false && Monster1_Animation.GetIsEnemyAttack == false)
            {
                EnemyAI.StandStill();
                Monster1_Animation.AttackPat();
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
                if(EnemyAI.GetDistanceBetweenEnemyAndPlayer > EnemyAI.GetAttackDis && Enemy.GetisDamage == false && Monster1_Animation.GetIsEnemyAttack == false)
                {
                    EnemyAI.GoToPlayer();
                }
                else if(EnemyAI.GetDistanceBetweenEnemyAndPlayer < EnemyAI.GetAttackDis && Enemy.GetisDamage == false && Monster1_Animation.GetIsEnemyAttack == false)
                {
                    EnemyAI.StandStill();
                    Monster1_Animation.AttackPat();
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
