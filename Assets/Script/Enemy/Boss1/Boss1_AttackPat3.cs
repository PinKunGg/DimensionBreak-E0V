using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_AttackPat3 : MonoBehaviour
{
    #region private variable
    private Boss Boss;
    private float AttackPat, DamageAdj, AttackType;
    #endregion

    #region SerializeField variable
    [SerializeField] private GameObject AttackHitFx;
    #endregion

    #region Class property
    public void SetValue(float AtkPat, float DmgAdj, float AtkType)
    {
        AttackPat = AtkPat;
        DamageAdj = DmgAdj;
        AttackType = AtkType;
    }
    #endregion

    //Get all component
    private void Awake()
    {
        Boss = GetComponentInParent<Boss>();    
    }
    private void OnParticleCollision(GameObject other)
    {
        print(other.name + " - " + other.tag);

        Vector2 point = other.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);

        int RotRand = Random.Range(-90,90);

        GameObject HitPar = Instantiate(AttackHitFx,point,Quaternion.Euler(RotRand,90f,90f));
        Destroy(HitPar,1f);

        if(other.CompareTag("Player"))
        {
            print("Hit Player");

            other.GetComponent<PlayerController>().PlayerHpCal(Boss.GetBossDamage * DamageAdj, AttackPat, AttackType, 10f);
        }
        else if(other.CompareTag("PlayerGuard"))
        {
            print("Hit Guard");

            other.GetComponent<Guard_Skill>().GuardCal(Boss.GetBossDamage * DamageAdj);
        }  
    }
}
