using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Attack : MonoBehaviour
{
    #region private variable
    private Boss Boss;
    private float AttackPat, DamageAdj, AttackType, Knockback;
    #endregion

    #region SerializeField variable
    [SerializeField] private GameObject AttackHitFx;
    #endregion

    #region Class property
    public void SetValue(float AtkPat, float DmgAdj, float AtkType, float knockvalue)
    {
        AttackPat = AtkPat;
        DamageAdj = DmgAdj;
        AttackType = AtkType;
        Knockback = knockvalue;
    }
    #endregion
    
    
    //Get all component
    private void Awake()
    {
        Boss = GetComponentInParent<Boss>();    
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

            other.GetComponent<PlayerController>().PlayerHpCal(Boss.GetBossDamage * DamageAdj, AttackPat, AttackType, Knockback);
        }
        else if(other.CompareTag("PlayerGuard"))
        {
            print("Hit Guard");
            other.GetComponent<Guard_Skill>().GuardCal(Boss.GetBossDamage * DamageAdj);
        }
    }
}
