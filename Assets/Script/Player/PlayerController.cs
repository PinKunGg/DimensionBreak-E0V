using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region SerializeField variable
    [SerializeField] private GameObject DamagePopCanvas, DamagePopUp, MainUI, DeathUI;
    [SerializeField] private Character_SCAO Character_SCAO;
    [SerializeField] private Slider PlayerHp;
    [SerializeField] private PlayerHand1 PlayerHand1;
    [SerializeField] private PlayerHand2 PlayerHand2;
    [SerializeField] private bool isGodMode;
    #endregion

    #region Private variable
    private PlayerMovement PlayerMovement;
    private PlayerAnimation PlayerAnimation;
    private float MaxHp, DefValue, MeleeDef, ProjectileDef;
    private bool isDamage, isDeath;
    private Vector2 IncommingHitPos;
    #endregion

    #region class Property
    public bool GetisDamage
    {
        get
        {
            return isDamage;
        }
    }
    public bool GetisDeath
    {
        get
        {
            return isDeath;
        }
    }
    public bool SetisDamge
    {
        set
        {
            isDamage = value;
        }
    }
    #endregion
    
    //Get all component
    private void Awake()
    {
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerAnimation = GetComponent<PlayerAnimation>();
    }

    //Prepare all setting
    private void Start()
    {
        DeathUI.SetActive(false);
        MainUI.SetActive(true);

        MaxHp = Character_SCAO.MaxHp;
        DefValue = Character_SCAO.DefValue;
        MeleeDef = Character_SCAO.MeleeDef;
        ProjectileDef = Character_SCAO.ProjectileDef;
        PlayerMovement.speed = Character_SCAO.speed;

        PlayerHp.maxValue = MaxHp;
        PlayerHp.value = MaxHp;

        isDeath = false;
    }
    private void Update()
    {
        //Check if Player Hp <= 0 is death
        if(PlayerHp.value <= 0)
        {
            if(DeathUI.activeInHierarchy == false)
            {
                DeathUI.SetActive(true);
                MainUI.SetActive(false);
            }
            
            isDeath = true;
            PlayerAnimation.DeathAnimation();
            CameraShake.instance.DeathCam();
        }
        
        //ﾍ(=￣∇￣)ﾉ
        if(Input.GetKeyDown(KeyCode.G))
        {
            isGodMode = !isGodMode;
        }
    }

    //Player calculate Hp and check knockback method
    public void PlayerHpCal(float value, float EnemyAttackPat, float AttackType, float Knockback)
    {
        if(!SkillManager.GetGuardActive && isDeath == false)
        {
            SoundManager.instance.PlaySelectAudio(0);
            CameraShake.instance.ShakeCam(1f,0.1f);

            float damage = value;

            //Check knockback
            if(EnemyAttackPat == 1f)
            {
                
            }
            else if(EnemyAttackPat == 2f)
            {
                PlayerAnimation.TakeDamageAnima();
                TakeKnockback(Knockback);
            }
            else if(EnemyAttackPat == 3f)
            {
                
            }

            //Calculate damage upon AttackType
            if(AttackType == 1)
            {
                damage = damage - (damage * (DefValue + MeleeDef) / 100f);
            }
            else if(AttackType == 2)
            {
                damage = damage - (damage * (DefValue + ProjectileDef) / 100f);
            }

            //Ceil damge
            damage = Mathf.Ceil(damage);

            //check if damage less than 0
            if(damage <= 0)
            {
                damage = 0f;
            }

            //ﾍ(=￣∇￣)ﾉ
            if(!isGodMode)
            {
                PlayerHp.value -= damage;
            }

            //Get random position to spawn damage indicator that player take
            float randY = Random.Range(1f,1.3f);
            float randX = Random.Range(-0.3f,0.4f);

            //Spawn damage indicator
            GameObject DamagePop = Instantiate(DamagePopUp);
            Vector2 SpawnPos = new Vector2(transform.position.x + randX, transform.position.y + randY);
            DamagePop.transform.SetParent(DamagePopCanvas.transform,false);
            DamagePop.transform.position = SpawnPos;
            DamagePop.GetComponent<DamagePopUp>().SetDamagePopUp(damage);
        }
    }
    public void TakeKnockback(float value)
    {
        PlayerMovement.GetPlayerRb2d().AddForce(transform.right * IncommingHitPos.x * value,ForceMode2D.Impulse);
        PlayerMovement.GetPlayerRb2d().AddForce(transform.up * IncommingHitPos.y * value,ForceMode2D.Impulse);
        Invoke("StopKnockback",0.2f);
    }
    void StopKnockback()
    {
        PlayerMovement.GetPlayerRb2d().velocity = Vector2.zero;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Force die when Player touch "DeathZone"
        if(other.CompareTag("DeathZone"))
        {
            PlayerHp.value = 0f;
        }
        if(isDeath == false)
        {
            if(other.CompareTag("EnemyAttack") && (PlayerHand1.GetisGuard == false && PlayerHand2.GetisGuard == false))
            {
                IncommingHitPos = (transform.position - other.transform.position).normalized;
            }
            if(other.CompareTag("EnemyAttack") && (PlayerHand1.GetisGuard == true || PlayerHand2.GetisGuard == true))
            {
                IncommingHitPos = (transform.position - other.transform.position).normalized;
                TakeKnockback(10f);
            }
        }
    }
}
