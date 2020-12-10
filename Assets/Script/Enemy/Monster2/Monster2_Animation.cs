using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster2_Animation : MonoBehaviour
{
    #region private variable
    private Animator anima;
    private Enemy Enemy;
    private EnemyAI EnemyAI;
    private Rigidbody2D rb;
    private Monster2_Projectile_Attack Monster2_Projectile_Attack;
    private Monster2_Controller Monster2_Controller;
    private TimeStopEffect TimeStopEffect;
    private Transform Player;
    private Vector2 IncommingHitPos, PlayerVector;
    private bool isAttack, isAttack_Pat3, isAttack_Pat3Going, isAttack_Pat3SetUp;
    private string defaultTag;
    private float AttackRandom, PlayerAttackPat, AttackPat3_AttackRange;
    #endregion

    #region SerizlizeField variable
    [SerializeField] private Enemy_SCAO Enemy_SCAO;
    [SerializeField] private Monster2_Attack Monster2_Attack;
    [SerializeField] private Collider2D BodyCollider;
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
    

    //Get al component
    private void Awake()
    {
        Enemy = GetComponent<Enemy>();
        EnemyAI = GetComponent<EnemyAI>();
        anima = GetComponentInChildren<Animator>();
        Monster2_Controller = GetComponent<Monster2_Controller>();
        Monster2_Projectile_Attack = GetComponent<Monster2_Projectile_Attack>();
        TimeStopEffect = GetComponent<TimeStopEffect>();
        rb = GetComponent<Rigidbody2D>();

        Player = GameObject.Find("Player").transform;
    }
    private void Start()
    {
        Monster2_Controller.enabled = false;
        EnemyAI.SetIsUseLowFly = true;    
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
        if(TimeStopEffect.GetisStop == false && Monster2_Controller.enabled == false)
        {
            Monster2_Controller.enabled = true;
        }
        else if(TimeStopEffect.GetisStop == true && Monster2_Controller.enabled == true)
        {
            Monster2_Controller.enabled = false;
        }

        if(isAttack_Pat3Going == true && TimeManager.GetTimeStop() == false)
        {
            EnemyAI.SetIsUseLowFly = false;

            Vector3 dir = transform.position - Player.position;
            float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg - 270f;
            transform.rotation = Quaternion.Euler(0f,0f,angle);

            PlayerVector = new Vector2(Player.position.x, Player.position.y);
            
            EnemyAI.MoveToWardToPlayer();
            //rb.position = Vector2.MoveTowards(transform.position, PlayerVector, Enemy_SCAO.speed * Time.deltaTime);

            float dis = Vector2.Distance(transform.position, PlayerVector);

            if(dis < AttackPat3_AttackRange)
            {
                AttackPat3_Attack();
            }
        }
    }
    private void SpawnComplete()
    {
        Monster2_Controller.enabled = true;
        this.gameObject.tag = defaultTag;
    }
    public void WalkToPlayer()
    {
        anima.SetBool("isIdle", false);
        anima.SetBool("isForward", true);
        anima.SetBool("isBackward", false);
    }
    public void StandStill()
    {
        anima.SetBool("isIdle", true);
        anima.SetBool("isForward", false);
        anima.SetBool("isBackward", false);
    }
    public void WalkAwayFromPlayer()
    {
        anima.SetBool("isIdle", false);
        anima.SetBool("isForward", false);
        anima.SetBool("isBackward", true);
    }
    public void M2_AttackPatManager()
    {
        if(isAttack_Pat3 == false)
        {
            AttackPat();
        }
    }
    void AttackPat()
    {
        isAttack = true;
        anima.SetBool("isIdle", false);
        anima.SetBool("isWalk", false);

        AttackRandom = Random.value;
        if (AttackRandom > 0.5) //50%
        {
            Invoke("SpawnProjectile1Delay",0.4f);
            anima.SetTrigger("isAttack_Pat1");
            Invoke("CoolDownAttack", 0.5f);
        }
        else if (AttackRandom <= 0.5 && AttackRandom > 0.3) //~31% - 49%
        {
            Invoke("SpawnProjectile2Delay",0.4f);
            anima.SetTrigger("isAttack_Pat2");
            Invoke("CoolDownAttack", 0.5f);
        }
        else if (AttackRandom <= 0.3) //20%
        {
            isAttack_Pat3 = true;
            Monster2_Attack.SetValue(3f,1f,1f,10f);
            anima.SetTrigger("isAttack_Pat3");
            isAttack_Pat3SetUp = true;
            AttackPat3_SetUp();
        }
    }
    void SpawnProjectile1Delay()
    {
        Monster2_Projectile_Attack.SpawnProjectile1();
        Monster2_Projectile_Attack.SetProjectileDamage = Enemy.GetEnemyDamage;
    }
    void SpawnProjectile2Delay()
    {
        Monster2_Projectile_Attack.SpawnProjectile2();
        Monster2_Projectile_Attack.SetProjectileDamage = Enemy.GetEnemyDamage;
    }
    void AttackPat3_SetUp()
    {
        if(isAttack_Pat3SetUp == true)
        {
            Invoke("AttackPat3_GoingDelay",0.5f);
            isAttack_Pat3SetUp = false;
        }
    }
    void AttackPat3_GoingDelay()
    {
        isAttack_Pat3Going = true;
    }
    void AttackPat3_Attack()
    {
        isAttack_Pat3Going = false;
        transform.rotation = Quaternion.Euler(0f,0f,0f);
        anima.SetBool("Attack_Pat3_Attack",true);
        isAttack_Pat3 = false;
        Invoke("CoolDownAttack", 0.5f);
    }
    void DeathAnima()
    {
        transform.rotation = Quaternion.Euler(0f,0f,0f);
        rb.gravityScale = 1f;
        BodyCollider.enabled = false;
        anima.SetTrigger("isDeath");
    }
    void CoolDownAttack()
    {
        EnemyAI.SetIsUseLowFly = true;
        anima.SetBool("Attack_Pat3_Attack",false);
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
        isAttack_Pat3 = false;
        isAttack_Pat3Going = false;
        isAttack_Pat3SetUp = false;
        transform.rotation = Quaternion.Euler(0f,0f,0f);
    }

    public void TakeKnockback(float value)
    {
        rb.AddForce(transform.right * IncommingHitPos.x * value, ForceMode2D.Impulse);
        rb.AddForce(transform.up * IncommingHitPos.y * value, ForceMode2D.Impulse);
        Invoke("StopKnockback",0.2f);
    }
    void StopKnockback()
    {
        rb.velocity = Vector2.zero;
    }
    public void TakeDamage()
    {
        transform.rotation = Quaternion.Euler(0f,0f,0f);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, AttackPat3_AttackRange);
    }
}
