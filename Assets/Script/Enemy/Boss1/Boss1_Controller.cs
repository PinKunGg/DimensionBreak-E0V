using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Controller : MonoBehaviour
{
    private Boss Boss;
    private BossAI BossAI;
    private Boss1_Animation Boss1_Animation;
    private float AttackRandom;
    private bool isRetreat, isCurrentRetreat;

    //Get all component
    private void Awake()
    {
        Boss = GetComponent<Boss>();
        BossAI = GetComponent<BossAI>();
        Boss1_Animation = GetComponent<Boss1_Animation>();    
    }
    private void Update()
    {
        if(BossAI.GetPlayerInRange == true && Boss.GetisDeath == false)
        {
            AttackPatternManager();
        }
        else if(Boss.GetisDeath == true)
        {
            this.enabled = false;
        }

        if(BossAI.GetIsGoToPlayer == true)
        {
            Boss1_Animation.WalkToPlayer();
        }
        else if(BossAI.GetisGetAwayFromPlayer == true)
        {
            Boss1_Animation.WalkAwayFromPlayer();
        }
        else if(BossAI.GetIsGoToPlayer == false && BossAI.GetisGetAwayFromPlayer == false)
        {
            Boss1_Animation.StandStill();
        }
    }

    void AttackPatternManager()
    {
        if(Boss.GetEnemyHpBar[1].value > (Boss.GetBossMaxHp / 2))
        {
            if(BossAI.GetDistanceBetweenEnemyAndPlayer > BossAI.GetAttackDis && Boss.GetisDamage == false && Boss1_Animation.GetIsEnemyAttack == false && Boss1_Animation.GetIsAttackPat3 == false)
            {
                BossAI.GoToPlayer();
            }
            else if(BossAI.GetDistanceBetweenEnemyAndPlayer < BossAI.GetAttackDis && Boss.GetisDamage == false && Boss1_Animation.GetIsEnemyAttack == false && Boss1_Animation.GetIsAttackPat3 == false)
            {
                BossAI.StandStill();
                Boss1_Animation.AttackPat();
            }
        }
        else if(Boss.GetEnemyHpBar[1].value <= (Boss.GetBossMaxHp / 2))
        {
            if(isRetreat == false)
            {
                isRetreat = true;
                isCurrentRetreat = false;
                AttackRandom = Random.value;
            }

            if(AttackRandom > 0.3f)
            {
                if(BossAI.GetDistanceBetweenEnemyAndPlayer > BossAI.GetAttackDis && Boss.GetisDamage == false && Boss1_Animation.GetIsEnemyAttack == false && Boss1_Animation.GetIsAttackPat3 == false)
                {
                    BossAI.GoToPlayer();
                }
                else if(BossAI.GetDistanceBetweenEnemyAndPlayer < BossAI.GetAttackDis && Boss.GetisDamage == false && Boss1_Animation.GetIsEnemyAttack == false && Boss1_Animation.GetIsAttackPat3 == false)
                {
                    BossAI.StandStill();
                    Boss1_Animation.AttackPat();
                }
                if(isCurrentRetreat == false)
                {
                    isCurrentRetreat = true;
                    Invoke("ResetRetreat",1.5f);
                }
            }
            else if(AttackRandom <= 0.3f && Boss1_Animation.GetIsAttackPat3 == false)
            {
                BossAI.GetAwayFromPlayer();

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
        Gizmos.DrawWireSphere(transform.position,BossAI.GetAttackDis);
    }
}
