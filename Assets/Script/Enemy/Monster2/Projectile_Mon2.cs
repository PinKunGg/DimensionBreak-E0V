using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Mon2 : MonoBehaviour
{
    #region private variable
    private float DestroyDistance = 20f, Damage, DamageAdj, AttackPat, AttackType, KnockbackValue;
    private Transform Player;
    private Vector2 TargetPos, StartPos;
    #endregion

    #region SerializeField variable
    [SerializeField] private GameObject ImpactEffect;
    #endregion

    #region Class property
    public Vector2 SetStartPos
    {
        set
        {
            StartPos = value;
        }
    }
    public void SetValue(float dmg, float dmgadj, float atkpat, float atktype, float knockback)
    {
        Damage = dmg;
        DamageAdj = dmgadj;
        AttackPat = atkpat;
        AttackType = atktype;
        KnockbackValue = knockback;
    }
    #endregion
    
    
    //Get all component
    private void Awake()
    {
        Player = GameObject.Find("Player").transform;
    }

    //Prepare all setting
    private void Start()
    {
        TargetPos = new Vector2(Player.position.x,Player.position.y);    
    }
    private void Update()
    {
        float DistanceFromStartToNow = Vector2.Distance(StartPos,transform.position);
        
        if(DistanceFromStartToNow > DestroyDistance)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("EnemyAttack") && !other.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("DeathEnemy") && !other.gameObject.CompareTag("Platform"))
        {
            GameObject ImpactFX = Instantiate(ImpactEffect,transform.position,Quaternion.identity);
            Destroy(ImpactFX,0.2f);
            this.gameObject.SetActive(false);
        }
        if(other.CompareTag("Player"))
        {
            print("Hit Player");
            other.GetComponent<PlayerController>().PlayerHpCal(Damage * DamageAdj, AttackPat, AttackType, KnockbackValue);
        }
        else if(other.CompareTag("PlayerGuard"))
        {
            print("Hit Guard");
            other.GetComponent<Guard_Skill>().GuardCal(Damage * DamageAdj);
        }
    }
}