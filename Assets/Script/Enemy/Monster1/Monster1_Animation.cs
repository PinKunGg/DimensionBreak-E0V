using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Monster1_Animation : MonoBehaviour
{
    #region private variable
    private Animator anima;
    private Enemy Enemy;
    private Rigidbody2D rb;
    private Monster1_Controller Monster1_Controller;
    private TimeStopEffect TimeStopEffect;
    private Vector2 IncommingHitPos;
    private bool isAttack;
    private string defaultTag;
    private float AttackRandom, PlayerAttackPat;
    #endregion

    #region SerializeField variable
    [SerializeField] private Enemy_SCAO Enemy_SCAO;
    [SerializeField] private Monster1_Attack[] Monster1_Attack;
    #endregion

    #region Class property
    public bool GetIsEnemyAttack
    {
        get
        {
            return isAttack;
        }
    }
    #endregion
    

    //Get all component
    private void Awake()
    {
        Enemy = GetComponent<Enemy>();
        anima = GetComponentInChildren<Animator>();
        Monster1_Controller = GetComponent<Monster1_Controller>();
        TimeStopEffect = GetComponent<TimeStopEffect>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    //Prepare all setting
    private void Start()
    {
        Monster1_Controller.enabled = false;

        defaultTag = null;
        defaultTag = this.gameObject.tag;
        this.gameObject.tag = "EnemySpawnSequent";
        Invoke("SpawnComplete",0.5f);
    }
    private void Update()
    {
        if(Enemy.GetisDeath == true)
        {
            TimeStopEffect.SelfDeath();
            DeathAnima();
            Destroy(gameObject,1.5f);
        }
        if(TimeStopEffect.GetisStop == false && Monster1_Controller.enabled == false)
        {
            Monster1_Controller.enabled = true;
        }
        else if(TimeStopEffect.GetisStop == true && Monster1_Controller.enabled == true)
        {
            Monster1_Controller.enabled = false;
        }
    }
    private void SpawnComplete()
    {
        this.gameObject.tag = defaultTag;
        Monster1_Controller.enabled = true;
    }
    public void WalkToPlayer()
    {
        anima.SetBool("isIdle", false);
        anima.SetBool("isWalk", true);
    }
    public void StandStill()
    {
        anima.SetBool("isIdle", true);
        anima.SetBool("isWalk", false);
    }
    public void WalkAwayFromPlayer()
    {
        anima.SetBool("isIdle", false);
        anima.SetBool("isWalk", true);
    }
    public void AttackPat()
    {
        isAttack = true;
        anima.SetBool("isIdle", false);
        anima.SetBool("isWalk", false);

        AttackRandom = Random.value;
        if (AttackRandom > 0.5)
        {
            Monster1_Attack[0].SetValue(1f,1f,1f,0f);
            Monster1_Attack[1].SetValue(1f,1f,1f,0f);
            anima.SetTrigger("isAttack_Pat1");
            Invoke("CoolDownAttack", 0.5f);
        }
        else if (AttackRandom <= 0.5 && AttackRandom > 0.3)
        {
            Monster1_Attack[0].SetValue(2f,1f,1f,10f);
            Monster1_Attack[1].SetValue(2f,1f,1f,10f);
            anima.SetTrigger("isAttack_Pat2");
            Invoke("CoolDownAttack", 0.5f);
        }
        else if (AttackRandom <= 0.3)
        {
            Monster1_Attack[0].SetValue(3f,1.5f,1f,0f);
            Monster1_Attack[1].SetValue(3f,1.5f,1f,0f);
            anima.SetTrigger("isAttack_Pat3");
            Invoke("CoolDownAttack", 0.5f);
        }
    }
    void DeathAnima()
    {
        anima.SetTrigger("isDeath");
    }
    void CoolDownAttack()
    {
        anima.SetBool("isIdle", true);
        anima.SetBool("isWalk", false);
        Invoke("ResetAttack", 1f);
    }
    void ResetAttack()
    {
        anima.SetBool("isIdle", true);
        anima.SetBool("isWalk", false);
        Enemy.SetisDamage = false;
        isAttack = false;
    }

    public void TakeKnockback(float value)
    {
        rb.AddForce(transform.right * IncommingHitPos.x * value,ForceMode2D.Impulse);
        rb.AddForce(transform.up * IncommingHitPos.y * value,ForceMode2D.Impulse);
        Invoke("StopKnockback",0.2f);
    }
    void StopKnockback()
    {
        rb.velocity = Vector2.zero;
    }
    public void TakeDamage()
    {
        Enemy.SetisDamage = true;
        anima.SetTrigger("isDamage");
        CancelInvoke();
        Invoke("ResetAttack",0.3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerAttack") && Enemy.GetisDeath == false)
        {
            PlayerAttackPat = Enemy.GetPlayerAttackPat;
            print("PlayerAttackPat = " + PlayerAttackPat);
            IncommingHitPos = (transform.position - other.transform.position).normalized;

            if(PlayerAttackPat == 1f)
            {
                
            }
            else if(PlayerAttackPat == 2f)
            {
                
            }
            else if(PlayerAttackPat == 3f) //JumpAttack
            {
                TakeDamage();
                TakeKnockback(Enemy.GetKnockbackValue);
            }
            else if(PlayerAttackPat == 4f) //FallAttack
            {
                TakeDamage();
                TakeKnockback(Enemy.GetKnockbackValue);
            }
            else if(PlayerAttackPat == 5f) //DashAttack
            {
                TakeDamage();
            }
            else if(PlayerAttackPat == 6f) //FallDirectAttack
            {
                TakeDamage();
                TakeKnockback(Enemy.GetKnockbackValue);
            }
        }
    }
}
