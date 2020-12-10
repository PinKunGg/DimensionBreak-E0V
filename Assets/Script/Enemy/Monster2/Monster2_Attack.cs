using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster2_Attack : MonoBehaviour
{
    #region private variable
    private Enemy Enemy;
    private float AttackPat, DamageAdj, AttackType, Knockback;
    #endregion

    #region SerializeField variable
    [SerializeField] private GameObject AttackHitFx;
    #endregion
    
    #region Class property
    public void SetValue(float Atkpat, float DmgAdj, float AtkType, float knockValue)
    {
        AttackPat = Atkpat;
        DamageAdj = DmgAdj;
        AttackType = AtkType;
        Knockback = knockValue;
    }
    #endregion

    //Get all component
    private void Awake()
    {
        Enemy = GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            print("Hit Player");

            Vector2 point = other.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);

            int RotRand = Random.Range(-90,90);
            GameObject HitPar = Instantiate(AttackHitFx,point,Quaternion.Euler(RotRand,90f,90f));
            Destroy(HitPar,1f);
            
            other.GetComponent<PlayerController>().PlayerHpCal(Enemy.GetEnemyDamage * DamageAdj, AttackPat, AttackType, Knockback);
        }
        else if(other.CompareTag("PlayerGuard"))
        {
            print("Hit Guard");
            other.GetComponent<Guard_Skill>().GuardCal(Enemy.GetEnemyDamage * DamageAdj);
        }
    }
}
