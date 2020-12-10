using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    #region private variable
    private float MaxHp, DefValue, ProjectileDef, MeleeDef, EnemyDamage, PlayerAttackPat, KnockbackValue;
    private int Index;
    private bool isDamage, isDeath;
    private Rigidbody2D rb;
    private GameObject Player;
    #endregion

    #region SerializeField variable
    [SerializeField] private Enemy_SCAO Enemy_SCAO;
    [SerializeField] private BossAI BossAI;
    [SerializeField] private GameObject DamagePopCanvas, DamagePopUp, DamagePopUpCrit ,WeakPoint;
    [SerializeField] private Slider[] EnemyHpBar;
    [SerializeField] private Image[] colorBG,colorFill;
    [SerializeField] private bool isEnemyGodMode;
    #endregion
    
    #region Class Property

    #region Get Property
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
    public float GetBossMaxHp
    {
        get
        {
            return MaxHp;
        }
    }
    public float GetPlayerAttackPat
    {
        get
        {
            return PlayerAttackPat;
        }
    }
    public float GetBossDamage
    {
        get
        {
            return EnemyDamage;
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
    public GameObject GetPlayerGameObject
    {
        get
        {
            return Player;
        }
    }
    public Slider[] GetEnemyHpBar
    {
        get
        {
            return EnemyHpBar;
        }
    }
    #endregion
    #region Set Property
    public bool SetisDamage
    {
        set
        {
            isDamage = value;
        }
    }
    #endregion

    #endregion
    
    
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
        BossAI.SetIsFlyAble = Enemy_SCAO.FlyAble; 
        BossAI.SetJumpForce = Enemy_SCAO.JumpForce;

        if(BossAI != null)
        {
            BossAI.SetRetreatDis = Enemy_SCAO.RetreatDis;
            BossAI.SetDetectRange = Enemy_SCAO.DetectRange;
            BossAI.SetAttackDis = Enemy_SCAO.AttackDis;
            BossAI.SetSpeed = Enemy_SCAO.speed;
        }
    }
    void Start()
    {
        foreach(var HpBar in EnemyHpBar)
        {
            HpBar.maxValue = MaxHp;
            HpBar.value = MaxHp;    
        }

        foreach(var item in colorBG)
        {
            item.CrossFadeAlpha(0f,0f,true);
        }
        foreach(var item in colorFill)
        {
            item.CrossFadeAlpha(0f,0f,true);
        }
    }
    private void Update()
    {
        if(EnemyHpBar[0].value <= 0 && Index != 0)
        {
            foreach(var item in colorBG)
            {
                item.CrossFadeAlpha(0f,0.5f,false);
            }
            foreach(var item in colorFill)
            {
                item.CrossFadeAlpha(0f,0.5f,false);
            }

            BossAI.enabled = false;
            this.gameObject.tag = "DeathEnemy";

            if(WeakPoint != null)
            {
                WeakPoint.tag = "DeathEnemy";
            }

            isDeath = true;
            
            //Destroy In Animation
        }
        else if(EnemyHpBar[EnemyHpBar.Length - 1].value != MaxHp && BossAI != null)
        {
            if(BossAI.GetPlayerInRange == false)
            {
                BossAI.StartFollowPlayer();
            }
        }
    }
    public void HpCal(float Damage, float DamageAdj, float AttackPat, float AttackType, float Knockback)
    {
        foreach(var item in colorBG)
        {
            item.CrossFadeAlpha(1f,0f,true);
        }
        foreach(var item in colorFill)
        {
            item.CrossFadeAlpha(1f,0f,true);
        }
        KnockbackValue = Knockback;
        PlayerAttackPat = AttackPat;
        float damage = Damage * DamageAdj;
        if(AttackType == 1f)
        {
            damage = damage - (damage * ((DefValue + MeleeDef)/100f));
        }
        else if(AttackType == 2f)
        {
            damage = damage - (damage * ((DefValue + ProjectileDef)/100f));
        }
        
        damage = Mathf.Ceil(damage);
        
        if(damage <= 0)
        {
            damage = 0f;
        }

        if(!isEnemyGodMode)
        {
            for(int i = EnemyHpBar.Length - 1; i >= 0; i--)
            {
                if(EnemyHpBar[i].value > 0)
                {
                    EnemyHpBar[i].value -= damage;
                    break;
                }
            }
        }

        float randY = Random.Range(1f,1.6f);
        float randX = Random.Range(-0.5f,0.6f);

        GameObject DamagePop;

        if(DamageAdj <= 1)
        {
            DamagePop = Instantiate(DamagePopUp);
            Vector2 SpawnPos = new Vector2(transform.position.x + randX, transform.position.y + randY);
            DamagePop.transform.SetParent(DamagePopCanvas.transform,false);
            DamagePop.transform.position = SpawnPos;
            DamagePop.GetComponent<DamagePopUp>().SetDamagePopUp(damage);
        }
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
            foreach(var HpBar in EnemyHpBar)
            {
                HpBar.value = 0f;    
            }
        }    
    }
}
