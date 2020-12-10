using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand1 : MonoBehaviour
{
    #region Private variable
    private Vector3 difference, ArmPos;
    private Quaternion ArmRot;
    private PlayerMovement PlayerMovement;
    private PlayerHandController PlayerHandController;
    private PlayerController PlayerController;
    private PlayerAttackManager PlayerAttackManager;
    private SkillManager SkillManager;
    private bool isAttack = false, isGuard = false, canGuard = true;
    private float rotationZ, CloseRangeAttack_Dimp = -20f, LongRangeAttack_Dimp = -45f;
    public string ObjType;
    #endregion

    #region SerializeField variable
    [SerializeField] private PlayerHand2 PlayerHand2;
    [SerializeField] private Transform Player, ArmPivot;
    #endregion

    #region Class Property
    public bool GetisGuard
    {
        get
        {
            return isGuard;
        }
    }
    public bool SetisGuard
    {
        set
        {
            isGuard = value;
        }
    }
    public bool GetisAttack
    {
        get
        {
            return isAttack;
        }
    }
    public bool SetisAttack
    {
        set
        {
            isAttack = value;
        }
    }
    #endregion

    //Get all component
    void Awake()
    {
        PlayerController = GetComponentInParent<PlayerController>();
        SkillManager = GetComponentInParent<SkillManager>();
        PlayerMovement = GetComponentInParent<PlayerMovement>();
        PlayerHandController = GetComponentInParent<PlayerHandController>();
        PlayerAttackManager = GetComponentInParent<PlayerAttackManager>();
    }

    //Prepare all setting
    void Start()
    {
        ArmPos = ArmPivot.localPosition;
        ArmRot = transform.localRotation;
    }
    void FixedUpdate()
    {
        difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - ArmPivot.position;
        difference.Normalize();

        rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
    }
    void Update()
    {
        if(PlayerController.GetisDeath == false)
        {
            if ((PlayerHandController.GetuseHand == 1f || PlayerHandController.GetuseHand == 3f) && GM.instance.isEsc == false)
            {
                TypeOfObj();
            }
            else
            {
                ArmPivot.rotation = Quaternion.Euler(0f, 0f, 0f);
            }

            if (PlayerHandController.Weapon1 != null)
            {
                FlipGameObject();
            }
        }
    }
    public void SetupHand1Item()
    {
        if(PlayerHandController.GetSetObjType1 != null)
        {
            ObjType = PlayerHandController.GetSetObjType1.ToUpper();
        }
    }
    void FlipGameObject()
    {
        if(PlayerMovement.FlipPos.x > 0f)
        {
            PlayerHandController.Weapon1.GetComponent<SpriteRenderer>().sortingOrder = 3;
        }
        else
        {
            PlayerHandController.Weapon1.GetComponent<SpriteRenderer>().sortingOrder = -3;
        }

    }
    public void TypeOfObj()
    {
        if(ObjType == "CLOSE_RANGE")
        {
            if(PlayerController.GetisDamage == false)
            {
                AttackMove();
                GuardSkill();
            }
        }
        else if(ObjType == "LONG_RANGE")
        {
            LongRangeObj();

            if(PlayerController.GetisDamage == false)
            {
                AttackMove();
            }
        }
        else
        {
            ResetArm();
        }
    }
    public void CloseRangeObj()
    {
        if(Player.localScale.x > 0f)
        {
            ArmPivot.rotation = Quaternion.Euler(0f, 0f, rotationZ - CloseRangeAttack_Dimp);
        }
        else
        {
            ArmPivot.rotation = Quaternion.Euler(180f, 180f, rotationZ + CloseRangeAttack_Dimp);
        }
    }
    void LongRangeObj()
    {
        if(Player.localScale.x > 0f)
        {
            ArmPivot.rotation = Quaternion.Euler(0f, 0f, rotationZ - LongRangeAttack_Dimp);
        }
        else
        {
            ArmPivot.rotation = Quaternion.Euler(180f, 180f, rotationZ + LongRangeAttack_Dimp);
        }
    }
    public void ResetArm()
    {
        //ArmPivot.localPosition = new Vector2(ArmPos.x, ArmPos.y);
        ArmPivot.localRotation = Quaternion.Euler(ArmRot.x,ArmRot.y,ArmRot.z);
    }
    void AttackMove()
    {
        if(Input.GetButtonDown("Fire1") && isAttack == false)
        {
            canGuard = false;
            isAttack = true;

            if(ObjType == "CLOSE_RANGE" && isGuard == false)
            {
                PlayerAttackManager.AttackSelection(ObjType);
            }
            else if(ObjType == "LONG_RANGE")
            {
                PlayerAttackManager.AttackSelection(ObjType);
            }
        }
    }
    void GuardSkill()
    {
        if(Input.GetButtonDown("Fire2") && canGuard == true)
        {
            canGuard = false;
            isGuard = true;
            PlayerHand2.SetisGuard = true;
            if(PlayerHandController.GetuseHand == 1f)
            {
                PlayerAttackManager.A1_Guard();
                SkillManager.GuardSkill();
            }
            else if(PlayerHandController.GetuseHand == 3f)
            {
                PlayerAttackManager.Dual_Guard();
                SkillManager.GuardSkill();
            }
        }
        if(Input.GetButtonUp("Fire2"))
        {
            Invoke("DelayAttack",0.1f);
            canGuard = true;
            isGuard = false;
            PlayerHand2.SetisGuard = false;
            PlayerAttackManager.ResetGuard();
            SkillManager.GuardSkillDeActive();
            SkillManager.GuardRegen();
        }
        if(isGuard == true && !Input.GetButton("Fire2"))
        {
            DelayAttack();
            canGuard = true;
            isGuard = false;
            PlayerHand2.SetisGuard = false;
            PlayerAttackManager.ResetGuard();
            SkillManager.GuardSkillDeActive();
            SkillManager.GuardRegen();
        }
    }
    void ForceResetGuard()
    {
        DelayAttack();
        canGuard = true;
        isGuard = false;
        PlayerHand2.SetisGuard = false;
        PlayerAttackManager.ResetGuard();
        SkillManager.GuardSkillDeActive();
        SkillManager.GuardRegen();
    }
    public void DelayAttack()
    {
        canGuard = true;
        isAttack = false;
        PlayerWeaponManager.SetisDualAttack = false;
        PlayerAttackManager.isInAttack = false;
    }
}