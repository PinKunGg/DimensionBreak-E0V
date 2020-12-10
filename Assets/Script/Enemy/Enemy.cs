using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    #region private variable
    private float MaxHp, DefValue, ProjectileDef, MeleeDef, EnemyDamage, PlayerAttackPat, KnockbackValue;
    private Rigidbody2D rb;
    private GameObject Player;
    private int Index;
    private bool isDamage, isDeath;
    #endregion

    #region SerializeField variable
    [SerializeField] private Enemy_SCAO Enemy_SCAO;
    [SerializeField] private EnemyAI EnemyAI;
    [SerializeField] private GameObject DamagePopCanvas, DamagePopUp, DamagePopUpCrit, WeakPoint;
    [SerializeField] private Slider EnemyHpBar;
    [SerializeField] private Image colorBG,colorFill;
    [SerializeField] private bool isEnemyGodMode;
    #endregion
    
    #region Class Property
    public int GetEnemyIndex
    {
        get
        {
            return Index;
        }
    }
    public float GetKnockbackValue
    {
        get
        {
            return KnockbackValue;
        }
    }
    public float GetEnemyMaxHp
    {
        get
        {
            return MaxHp;
        }
    }
    public float GetEnemyDamage
    {
        get
        {
            return EnemyDamage;
        }
    }
    public float GetPlayerAttackPat
    {
        get
        {
            return PlayerAttackPat;
        }
    }
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
    public bool SetisDamage
    {
        set
        {
            isDamage = value;
        }
    }
    public GameObject GetPlayerGameObject
    {
        get
        {
            return Player;
        }
    }
    public Slider GetEnemyHpBar
    {
        get
        {
            return EnemyHpBar;
        }
    }
    #endregion
    
    //Get all component
    private void Awake()
    {
        Player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        MaxHp = Enemy_SCAO.MaxHp;
        DefValue = Enemy_SCAO.DefValue;
        ProjectileDef = Enemy_SCAO.ProjectileDef;
        MeleeDef = Enemy_SCAO.MeleeDef;
        EnemyDamage = Enemy_SCAO.Damage;
        Index = Enemy_SCAO.Index;

        if(EnemyAI != null)
        {
            EnemyAI.SetIsFlyAble = Enemy_SCAO.FlyAble; 
            EnemyAI.SetJumpForce = Enemy_SCAO.JumpForce;
            EnemyAI.SetRetreatDis = Enemy_SCAO.RetreatDis;
            EnemyAI.SetDetectRange = Enemy_SCAO.DetectRange;
            EnemyAI.SetAttackDis = Enemy_SCAO.AttackDis;
            EnemyAI.SetSpeed = Enemy_SCAO.speed;
        }
    }

    //Prepare all setting
    void Start()
    {
        EnemyHpBar.maxValue = MaxHp;
        EnemyHpBar.value = MaxHp;
        colorBG.CrossFadeAlpha(0f,0f,true);
        colorFill.CrossFadeAlpha(0f,0f,true);
    }

    private void Update()
    {
        if(EnemyHpBar.value <= 0 && Index != 0)
        {
            colorBG.CrossFadeAlpha(0f,0.5f,false);
            colorFill.CrossFadeAlpha(0f,0.5f,false);
            EnemyAI.enabled = false;
            this.gameObject.tag = "DeathEnemy";

            if(WeakPoint != null)
            {
                WeakPoint.tag = "DeathEnemy";
            }

            isDeath = true;
            
            //Destroy In Animation
        }
        else if(EnemyHpBar.value != MaxHp && EnemyAI != null)
        {
            if(EnemyAI.GetPlayerInRange == false)
            {
                EnemyAI.StartFollowPlayer();
            }
        }
    }
    public void HpCal(float Damage, float DamageAdj, float AttackPat, float AttackType, float Knockback)
    {
        colorBG.CrossFadeAlpha(1f,0f,true);
        colorFill.CrossFadeAlpha(1f,0f,true);
        KnockbackValue = Knockback;
        PlayerAttackPat = AttackPat;
        float damage = Damage * DamageAdj;
        
        //Calculate Damage upon AttackPattern
        if(AttackType == 1f)
        {
            damage = damage - (damage * ((DefValue + MeleeDef)/100f));
        }
        else if(AttackType == 2f)
        {
            damage = damage - (damage * ((DefValue + ProjectileDef)/100f));
        }
        
        //Ceil damage
        damage = Mathf.Ceil(damage);
        
        //if damage <= 0 make damage = 0
        if(damage <= 0)
        {
            damage = 0f;
        }

        //Enemy God Mode
        if(!isEnemyGodMode)
        {
            EnemyHpBar.value -= damage;
        }
        
        //Spawn Damage indicator
        float randY = Random.Range(1f,1.6f);
        float randX = Random.Range(-0.5f,0.6f);
        
        GameObject DamagePop;

        //Normal Damage PopUp
        if(DamageAdj <= 1)
        {
            DamagePop = Instantiate(DamagePopUp);
            Vector2 SpawnPos = new Vector2(transform.position.x + randX, transform.position.y + randY);
            DamagePop.transform.SetParent(DamagePopCanvas.transform,false);
            DamagePop.transform.position = SpawnPos;
            DamagePop.GetComponent<DamagePopUp>().SetDamagePopUp(damage);
        }

        //Critical Damage PopUp
        else if(DamageAdj > 1)
        {
            DamagePop = Instantiate(DamagePopUpCrit);
            Vector2 SpawnPos = new Vector2(transform.position.x + randX, transform.position.y + randY);
            DamagePop.transform.SetParent(DamagePopCanvas.transform,false);
            DamagePop.transform.position = SpawnPos;
            DamagePop.GetComponent<DamagePopUp>().SetDamagePopUp(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("DeathZone"))
        {
            EnemyHpBar.value = 0f;
        }    
    }
}
