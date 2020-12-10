using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_Animation : MonoBehaviour
{
    #region private variable
    private Animator anima;
    private Boss Boss;
    private Rigidbody2D rb;
    private Boss1_Controller Boss1_Controller;
    private TimeStopEffect TimeStopEffect;
    private Vector2 IncommingHitPos;
    private Transform Player;
    private bool isAttack, isAttackPat3, isAttackPat3_Charging;
    private float AttackRandom, PlayerAttackPat, timer;
    private string defaultTag, defaultWeakPointTag;
    #endregion

    #region SerializeField variable
    [SerializeField] private Enemy_SCAO Enemy_SCAO;
    [SerializeField] private Boss1_Attack Boss1_Attack;
    [SerializeField] private Boss1_AttackPat3 Boss1_AttackPat3;
    [SerializeField] private GameObject WeakPoint;
    [SerializeField] private Transform LaserSpawnPos;
    [SerializeField] private GameObject HeadFx, LaserCharge, LaserAttack;
    #endregion

    #region Class property
    public bool GetIsEnemyAttack
    {
        get
        {
            return isAttack;
        }
    }
    public bool GetIsAttackPat3
    {
        get
        {
            return isAttackPat3;
        }
    }
    #endregion
    
    //Get all component
    private void Awake()
    {
        Player = GameObject.Find("Player").transform;
        Boss = GetComponent<Boss>();
        anima = GetComponentInChildren<Animator>();
        Boss1_Controller = GetComponent<Boss1_Controller>();
        TimeStopEffect = GetComponent<TimeStopEffect>();
        rb = GetComponent<Rigidbody2D>();
    }

    //Prepare all setting
    private void Start()
    {
        Boss1_Controller.enabled = false;
        HeadFx.SetActive(true);
        LaserCharge.SetActive(false);
        LaserAttack.SetActive(false);

        defaultTag = null;
        defaultTag = this.gameObject.tag;
        defaultWeakPointTag = null;
        defaultWeakPointTag = WeakPoint.tag;
        this.gameObject.tag = "EnemySpawnSequent";
        WeakPoint.tag = "EnemySpawnSequent";
        Invoke("SpawnComplete",0.5f);
    }
    private void Update()
    {
        if(Boss.GetisDeath == true)
        {
            TimeStopEffect.SelfDeath();
            DeathAnima();
            isAttackPat3 = false;
            timer = 0f;
            LaserAttack.SetActive(false);
            LaserCharge.SetActive(false);
            Destroy(gameObject,4f);
        }
        if(TimeStopEffect.GetisStop == false && Boss1_Controller.enabled == false)
        {
            Boss1_Controller.enabled = true;
        }
        else if(TimeStopEffect.GetisStop == true && Boss1_Controller.enabled == true)
        {
            Boss1_Controller.enabled = false;
        }

        if(TimeManager.GetTimeStop() == false && isAttackPat3 == true && isAttackPat3_Charging == true)
        {
            Vector3 dir = Player.position - LaserSpawnPos.position;
            float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg - 90f;
            LaserSpawnPos.rotation = Quaternion.Euler(0f,0f,angle);
        }

        if(isAttackPat3 && TimeManager.GetTimeStop() == false)
        {
            timer += Time.deltaTime;

            //print(timer);
        }

        if(timer > 7f && timer < 9f)
        {
            isAttackPat3_Charging = false;
            print("Shoot");
            LaserAttack.SetActive(true);
        }
        else if(timer > 10f && timer < 12f)
        {
            print("Finish LaserShoot");
            HeadFx.SetActive(true);
            LaserAttack.SetActive(false);
            LaserCharge.SetActive(false);
            isAttackPat3 = false;
            CoolDownAttack();
        }
    }
    private void SpawnComplete()
    {
        this.gameObject.tag = defaultTag;
        Boss1_Controller.enabled = true;
        WeakPoint.tag = defaultWeakPointTag;
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
            Boss1_Attack.SetValue(1f,1f,1f,0f);
            anima.SetTrigger("isAttack_Pat1");
            Invoke("CoolDownAttack", 0.5f);
        }
        else if (AttackRandom <= 0.5 && AttackRandom > 0.3)
        {
            Boss1_Attack.SetValue(2f,1f,1f,5f);
            anima.SetTrigger("isAttack_Pat2");
            Invoke("CoolDownAttack", 0.5f);
        }
        else if (AttackRandom <= 0.3)
        {
            print("Bruhhhhhhhhhhhhhhh");

            if(Boss.GetEnemyHpBar[1].value != Boss.GetBossMaxHp)
            {
                timer = 0f;
                isAttackPat3_Charging = true;
                isAttackPat3 = true;
                Boss1_AttackPat3.SetValue(3,0.05f,2f);
                anima.SetTrigger("isAttack_Pat3");
                HeadFx.SetActive(false);
                LaserCharge.SetActive(true);
                print("Charging");
            }

            ResetAttack();
        }
    }
    void DeathAnima()
    {
        WeakPoint.SetActive(false);
        this.GetComponent<BoxCollider2D>().enabled = false;
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
        Boss.SetisDamage = false;
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
        Boss.SetisDamage = true;
        anima.SetTrigger("isDamage");
        CancelInvoke();
        Invoke("ResetAttack",0.3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("PlayerAttack") && Boss.GetisDeath == false)
        {
            PlayerAttackPat = Boss.GetPlayerAttackPat;
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
                TakeKnockback(Boss.GetKnockbackValue);
            }
            else if(PlayerAttackPat == 4f) //FallAttack
            {
                TakeDamage();
                TakeKnockback(Boss.GetKnockbackValue);
            }
            else if(PlayerAttackPat == 5f) //DashAttack
            {
                TakeDamage();
            }
            else if(PlayerAttackPat == 6f) //FallDirectAttack
            {
                TakeDamage();
                TakeKnockback(Boss.GetKnockbackValue);
            }
        }
    }
}
