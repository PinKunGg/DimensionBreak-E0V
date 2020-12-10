using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackManager : MonoBehaviour
{
    #region private variable
    private Animator anima;
    private PlayerHandController PlayerHandController;
    private Rigidbody2D rb;
    private PlayerMovement PlayerMovement;
    private PlayerHand1 PlayerHand1;
    private PlayerHand2 PlayerHand2;
    private bool isDashReady = true, isDashCoolDown, isDash;
    private float AttackTimer = 0f, MaxPat = 2f, DashCoolDown;
    private float pat, FixDashCoolDown = 3f, AttackCoolDown1 = 0.15f,AttackCoolDown2 = 0.4f, AttackCoolDown3 = 0.6f, AttackCoolDown4 = 0.8f, AttackCoolDown5 = 1f;
    #endregion

    #region SerializeField variable
    [SerializeField] private PlayerWeaponManager PlayerWeaponManager;
    [SerializeField] private GameObject DashFx, DashUI;
    [SerializeField] private Text DashCoolDownTxt;
    #endregion

    #region public static variable
    public static bool isAttack, isJumpAttackReady, isFallAttackReady, isFallDirect,isFallDirectAttackReady, isInAttack;
    #endregion

    //Get all component
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anima = GetComponentInChildren<Animator>();
        PlayerHandController = GetComponent<PlayerHandController>();
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerHand1 = GetComponentInChildren<PlayerHand1>();
        PlayerHand2 = GetComponentInChildren<PlayerHand2>();
    }

    //Prepare all setting
    private void Start()
    {
        DashFx.SetActive(false);
        DashUI.SetActive(false);
        DashCoolDownTxt.text = "";
    }
    void Update()
    {
        //Attack pattern TimerCount
        if(isAttack)
        {
            AttackTimer += Time.deltaTime;
        }
        
        //if time >= 1 reset attack pattern
        if(AttackTimer > 1f && isInAttack == false)
        {
            isAttack = false;
            pat = 0f;
            PlayerWeaponManager.SetAttackPat = pat;
            AttackTimer = 0f;
        }
        
        //if Dash Attack is ready
        if(isDashCoolDown == true)
        {
            DashUI.SetActive(true);
            DashCoolDown -= Time.deltaTime;
            DashCoolDownTxt.text = Mathf.Round(DashCoolDown).ToString();

            if(DashCoolDown <= 0f)
            {
                isDashCoolDown = false;
                isDashReady = true;
                DashUI.SetActive(false);
                DashCoolDownTxt.text = "";
            }
        }
        
        //Play weapon "falldirect" animation when Player hit ground
        if(isFallDirectAttackReady && isFallDirect)
        {
            isFallDirect = false;
            isFallDirectAttackReady = false;

            if(PlayerHandController.GetuseHand == 1f)
            {
                PlayerWeaponManager.SetAttackPat = 6f;
                PlayerWeaponManager.WeaponInHandManager();
            }
            else if(PlayerHandController.GetuseHand == 2f)
            {
                PlayerWeaponManager.SetAttackPat = 6f;
                PlayerWeaponManager.WeaponInHandManager();
            }
            else if(PlayerHandController.GetuseHand == 3f)
            {
                PlayerWeaponManager.SetAttackPat = 6f;
                PlayerWeaponManager.SpecialDualAttackManager(6f);
            }
            CameraShake.instance.ShakeCam(3f,0.2f);
            Invoke("ResetFallDirectAttack",AttackCoolDown2);
        }

        //Play DashAttack
        if(isDash)
        {
            if(PlayerMovement.FlipPos.x > 0f)
            {
                rb.velocity = Vector2.right * Mathf.Pow(rb.mass,6f);
            }
            else
            {
                rb.velocity = Vector2.left * Mathf.Pow(rb.mass,6f);
            }
        }
    }

    public void AttackSelection(string ObjType)
    {
        if(ObjType == "CLOSE_RANGE")
        {
            //this bool make when timer hit 0 not effect attack animation
            isInAttack = true;

            //Dash Attack
            if(Input.GetKey(KeyCode.LeftShift) && isDashReady == true)
            {
                DashAttack();
            }

            //Jump Attack
            else if(Input.GetButton("Jump") && isJumpAttackReady == true)
            {
                isJumpAttackReady = false;
                isFallAttackReady = true;
                JumpAttack();
            }

            //make decition to Play "fallattack" of "falldirectattack"
            else if(Input.GetButton("Jump") && isFallAttackReady == true)
            {
                isFallAttackReady = false;
                
                //FallAttack when Player is moving
                if(PlayerMovement.isMove == true)
                {
                    FallAttack();
                }

                //FallDirectAttack when Player is not moving
                else
                {
                    FallDirectAttack();
                }
            }

            //Default closerange weapon AttackPattern in hand1
            else if(PlayerHandController.GetuseHand == 1f && PlayerHandController.GetSetObjType1.ToUpper() == "CLOSE_RANGE")
            {
                AttackPat_A1();
            }
            
            //Default closerange weapon AttackPattern in hand2
            else if(PlayerHandController.GetuseHand == 2f && PlayerHandController.GetSetObjType2.ToUpper() == "CLOSE_RANGE")
            {
                AttackPat_A2();
            }

            //Default closerange weapon AttackPattern in dualhand
            else if(PlayerHandController.GetuseHand == 3f && PlayerHandController.GetSetObjType1.ToUpper() == "CLOSE_RANGE" && PlayerHandController.GetSetObjType2.ToUpper() == "CLOSE_RANGE")
            {
                CloseRangeAttackDualPat();
            }
        }
        else if(ObjType == "LONG_RANGE")
        {
            //this bool make when timer hit 0 not effect attack animation
            isInAttack = true;

            //Default longrange weapon AttackPattern in hand1
            if(PlayerHandController.GetuseHand == 1f && PlayerHandController.GetSetObjType1.ToUpper() == "LONG_RANGE")
            {
                LongRangeAttackPat_A1();
            }

            // Default longrange weapon AttackPattern in hand2
            else if(PlayerHandController.GetuseHand == 2f && PlayerHandController.GetSetObjType2.ToUpper() == "LONG_RANGE")
            {
                LongRangeAttackPat_A2();
            }

            //Default longrange weapon AttackPattern in dualhand
            else if(PlayerHandController.GetuseHand == 3f && PlayerHandController.GetSetObjType1.ToUpper() == "LONG_RANGE" && PlayerHandController.GetSetObjType2.ToUpper() == "LONG_RANGE")
            {
                LongRangeAttackDualPat();
            }
        }
    }

    //Play JumpAttack
    void JumpAttack()
    {
        rb.velocity = Vector2.up * 7f;

        if(PlayerHandController.GetuseHand == 1f)
        {
            PlayerWeaponManager.SetAttackPat = 3f;
            PlayerWeaponManager.WeaponInHandManager();
            anima.SetTrigger("Attack_Arm1_Pat" + 3f);
            PlayerHand1.Invoke("DelayAttack",AttackCoolDown2);
        }
        else if(PlayerHandController.GetuseHand == 2f)
        {
            PlayerWeaponManager.SetAttackPat = 3f;
            PlayerWeaponManager.WeaponInHandManager();
            anima.SetTrigger("Attack_Arm2_Pat" + 3f);
            PlayerHand2.Invoke("DelayAttack",AttackCoolDown2);
        }
        else if(PlayerHandController.GetuseHand == 3f)
        {
            PlayerWeaponManager.SetAttackPat = 3f;
            PlayerWeaponManager.SpecialDualAttackManager(3f);
            anima.SetTrigger("Attack_Dual_Pat" + 3f);
            PlayerHand1.Invoke("DelayAttack",AttackCoolDown2);
            PlayerHand2.Invoke("DelayAttack",AttackCoolDown2);
        }
    }

    //Play FallAttack
    void FallAttack()
    {
        rb.velocity = Vector2.down * 15f;

        if(PlayerHandController.GetuseHand == 1f)
        {
            PlayerWeaponManager.SetAttackPat = 4f;
            PlayerWeaponManager.WeaponInHandManager();
            anima.SetTrigger("Attack_Arm1_Pat" + 4f);
            PlayerHand1.Invoke("DelayAttack",AttackCoolDown2);
        }
        else if(PlayerHandController.GetuseHand == 2f)
        {
            PlayerWeaponManager.SetAttackPat = 4f;
            PlayerWeaponManager.WeaponInHandManager();
            anima.SetTrigger("Attack_Arm2_Pat" + 4f);
            PlayerHand2.Invoke("DelayAttack",AttackCoolDown2);
        }
        else if(PlayerHandController.GetuseHand == 3f)
        {
            PlayerWeaponManager.SetAttackPat = 4f;
            PlayerWeaponManager.SpecialDualAttackManager(4f);
            anima.SetTrigger("Attack_Dual_Pat" + 4f);
            PlayerHand1.Invoke("DelayAttack",AttackCoolDown2);
            PlayerHand2.Invoke("DelayAttack",AttackCoolDown2);
        }
    }

    //Play FallDirectAttack
    void FallDirectAttack()
    {
        DashFx.SetActive(true);
        isFallDirectAttackReady = false;
        isFallDirect = true;
        rb.velocity = Vector2.down * 30f;

        if(PlayerHandController.GetuseHand == 1f)
        {
            anima.SetTrigger("Attack_Arm1_Pat" + 6f);
            anima.SetBool("isFallDirect", false);
            PlayerHand1.Invoke("DelayAttack",AttackCoolDown2);
        }
        else if(PlayerHandController.GetuseHand == 2f)
        {

            anima.SetTrigger("Attack_Arm2_Pat" + 6f);
            anima.SetBool("isFallDirect", false);
            PlayerHand2.Invoke("DelayAttack",AttackCoolDown2);
        }
        else if(PlayerHandController.GetuseHand == 3f)
        {
            anima.SetTrigger("Attack_Dual_Pat" + 6f);
            anima.SetBool("isFallDirect", false);
            PlayerHand1.Invoke("DelayAttack",AttackCoolDown2);
            PlayerHand2.Invoke("DelayAttack",AttackCoolDown2);
        }
    }

    //Play DashAttack
    void DashAttack()
    {
        isDash = true;
        isDashReady = false;
        DashFx.SetActive(true);

        if(PlayerHandController.GetuseHand == 1f)
        {
            PlayerWeaponManager.SetAttackPat = 5f;
            PlayerWeaponManager.WeaponInHandManager();
            anima.SetTrigger("Attack_Arm1_Pat" + 5f);
            PlayerHand1.Invoke("DelayAttack",AttackCoolDown2);
        }
        else if(PlayerHandController.GetuseHand == 2f)
        {
            PlayerWeaponManager.SetAttackPat = 5f;
            PlayerWeaponManager.WeaponInHandManager();
            anima.SetTrigger("Attack_Arm2_Pat" + 5f);
            PlayerHand2.Invoke("DelayAttack",AttackCoolDown2);
        }
        else if(PlayerHandController.GetuseHand == 3f)
        {
            PlayerWeaponManager.SetAttackPat = 5f;
            PlayerWeaponManager.SpecialDualAttackManager(5f);
            anima.SetTrigger("Attack_Dual_Pat" + 5f);
            PlayerHand1.Invoke("DelayAttack",AttackCoolDown2);
            PlayerHand2.Invoke("DelayAttack",AttackCoolDown2);
        }
        Invoke("ResetDash",0.1f);
        Invoke("ResetDashFx",0.2f);
    }

    #region Default Attack Pattern Section
    //Play Default closerange weapon AttackPattern in Hand1
    void AttackPat_A1()
    {
        if(!isAttack)
        {
            isAttack = true;
            AttackTimeReset();
        }

        pat++;
        print("Pat_A1 = " + pat);
        
        if(pat > MaxPat)
        {
            isAttack = false;
            pat = 0f;
            PlayerWeaponManager.SetAttackPat = pat;
            AttackTimer = 0f;
            PlayerHand1.Invoke("DelayAttack",AttackCoolDown1);
        }
        else if(pat <= MaxPat)
        {
            SoundManager.instance.PlaySelectAudio(7);
            anima.SetTrigger("Attack_Arm1_Pat" + pat);
            PlayerWeaponManager.SetAttackPat = pat;
            PlayerWeaponManager.WeaponInHandManager();
            PlayerHand1.Invoke("DelayAttack",0.15f);
        }
    }

    //Play Default closerange weapon AttackPattern in Hand2
    void AttackPat_A2()
    {
        if(!isAttack)
        {
            isAttack = true;
            AttackTimeReset();
        }

        pat++;
        print("Pat_A2 = " + pat);
        
        if(pat > MaxPat)
        {
            isAttack = false;
            pat = 0f;
            PlayerWeaponManager.SetAttackPat = pat;
            AttackTimer = 0f;
            PlayerHand2.Invoke("DelayAttack",AttackCoolDown1);
        }
        else if(pat <= MaxPat)
        {
            SoundManager.instance.PlaySelectAudio(7);
            anima.SetTrigger("Attack_Arm2_Pat" + pat);
            PlayerWeaponManager.SetAttackPat = pat;
            PlayerWeaponManager.WeaponInHandManager();
            PlayerHand2.Invoke("DelayAttack",0.1f);
        }
    }

    //Play Default closerange weapon AttackPattern in DualHand
    void CloseRangeAttackDualPat()
    {
        if(!isAttack)
        {
            isAttack = true;
            AttackTimeReset();
        }

        pat++;
        print("Pat_A3 = " + pat);
        
        if(pat > MaxPat)
        {
            isAttack = false;
            pat = 0f;
            AttackTimer = 0f;
            PlayerHand1.Invoke("DelayAttack",AttackCoolDown2);
            PlayerHand2.Invoke("DelayAttack",AttackCoolDown2);
        }
        else if(pat <= MaxPat)
        {
            anima.SetTrigger("Attack_Dual_Pat" + pat);
            PlayerWeaponManager.SetAttackPat = pat;
            PlayerWeaponManager.WeaponInHandManager();
        }
    }
    
    //Play Default longrange weapon AttackPattern in Hand1
    void LongRangeAttackPat_A1()
    {
        PlayerWeaponManager.WeaponInHandManager();
        PlayerHand1.Invoke("DelayAttack",0.1f);
    }

    //Play Default longrange weapon AttackPattern in Hand2
    void LongRangeAttackPat_A2()
    {
        PlayerWeaponManager.WeaponInHandManager();
        PlayerHand2.Invoke("DelayAttack",0.1f);
    }

    //Play Default longrange weapon AttackPattern in DualHand
    void LongRangeAttackDualPat()
    {
        PlayerWeaponManager.WeaponInHandManager();
        PlayerHand2.Invoke("DelayAttack",0.1f);
    }
    #endregion

    #region Guard Section
    //Play GuardAnimation in Hand1
    public void A1_Guard()
    {
        anima.SetBool("Arm1_Guard",true);
    }

    //Play GuardAnimation in Hand2
    public void A2_Guard()
    {
        anima.SetBool("Arm2_Guard",true);
    }

    //Play GuardAnimation in DualHand
    public void Dual_Guard()
    {
        anima.SetBool("Dual_Guard",true);
    }
    #endregion

    #region Reset or Delay Attack Section
    void ResetDash()
    {
        isInAttack = false;
        isDash = false;
    }
    void ResetDashFx()
    {
        DashFx.SetActive(false);
        DashCoolDown = FixDashCoolDown;
        isDashCoolDown = true;
    }
    public void ResetGuard()
    {
        anima.SetBool("Arm1_Guard",false);
        anima.SetBool("Arm2_Guard",false);
        anima.SetBool("Dual_Guard",false);
    }
    void ResetFallDirectAttack()
    {
        isInAttack = false;
        DashFx.SetActive(false);
        anima.SetBool("isFallDirect", true);
    }
    void AttackTimeReset()
    {
        AttackTimer = 0f;
    }
    public void RestAttack()
    {
        StopAllCoroutines();
        PlayerHand1.DelayAttack();
        PlayerHand2.DelayAttack();
        isAttack = false;
        pat = 0f;
        AttackTimer = 0f;
    }
    #endregion
}