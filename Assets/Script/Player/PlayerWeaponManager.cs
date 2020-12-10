using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponManager : MonoBehaviour
{
    #region private variable
    private float pat;
    #endregion

    #region SerializeField variable
    [SerializeField]private PlayerHandController PlayerHandController;
    [SerializeField]private Text BulletAmountTxt;
    [SerializeField]private PlayerHand1 PlayerHand1;
    [SerializeField]private PlayerHand2 PlayerHand2;
    [SerializeField]private PlayerController PlayerController;
    #endregion

    #region private static variable
    private static bool isDualAttack;
    #endregion

    #region Class Property
    public float SetAttackPat
    {
        set
        {
            pat = value;
        }
    }
    public static bool SetisDualAttack
    {
        set
        {
            isDualAttack = value;
        }
    }
    #endregion
    private void Update()
    {
        if(PlayerController.GetisDeath == false)
        {
            SetBulletTxt();
        }    
    }

    //Check what player hand is now using
    public void WeaponInHandManager()
    {
        if(PlayerHandController.GetuseHand == 1f)
        {
            WeaponInHand1();
        }
        else if(PlayerHandController.GetuseHand == 2f)
        {
            WeaponInHand2();
        }
        else if(PlayerHandController.GetuseHand == 3f)
        {
            DualWeaponHand3();
        }
    }
    
    //use hand 1
    void WeaponInHand1()
    {
        //use weapon type 1, 2
        if(PlayerHandController.GetSetWeaponIndex1 == 1 || PlayerHandController.GetSetWeaponIndex1 == 2)
        {
            PlayerHandController.Weapon1.GetComponent<Sword_Type1>().AttackPat(PlayerHandController.GetSetWeaponIndex1, pat);
            PlayerHandController.Weapon1.GetComponent<Sword_Type1>().PlayerAttackPat = pat;
        }
        //use weapon type 3
        else if(PlayerHandController.GetSetWeaponIndex1 == 3)
        {
            if(PlayerHandController.Weapon1.GetComponent<Gun_Type1>().BulletAmount <= 0)
            {
                PlayerHandController.Weapon1.GetComponent<Gun_Type1>().BulletAmount = 0f;
            }
            else
            {
                PlayerHandController.Weapon1.GetComponent<Gun_Type1>().AttackPat(PlayerHandController.GetSetWeaponIndex1);
                PlayerHandController.Weapon1.GetComponent<Gun_Type1>().BulletAmount--;
            }
        }
    }

    //use hand 2
    void WeaponInHand2()
    {
        //use weapon type 1,2
        if(PlayerHandController.GetSetWeaponIndex2 == 1 || PlayerHandController.GetSetWeaponIndex2 == 2)
        {
            PlayerHandController.Weapon2.GetComponent<Sword_Type1>().AttackPat(PlayerHandController.GetSetWeaponIndex2, pat);
            PlayerHandController.Weapon2.GetComponent<Sword_Type1>().PlayerAttackPat = pat;
        }

        //use weapon type 3
        else if(PlayerHandController.GetSetWeaponIndex2 == 3)
        {   
            if(PlayerHandController.Weapon2.GetComponent<Gun_Type1>().BulletAmount <= 0)
            {
                PlayerHandController.Weapon2.GetComponent<Gun_Type1>().BulletAmount = 0f;
            }
            else
            {
                PlayerHandController.Weapon2.GetComponent<Gun_Type1>().AttackPat(PlayerHandController.GetSetWeaponIndex2);
                PlayerHandController.Weapon2.GetComponent<Gun_Type1>().BulletAmount--;
            }
        }
    }

    //use dual hand
    void DualWeaponHand3()
    {
        //use weapon type 1, 2
        if(PlayerHandController.GetSetWeaponIndex1 == 1 || PlayerHandController.GetSetWeaponIndex1 == 2)
        {
            if(isDualAttack == false)
            {
                StartCoroutine(CloseRangeAttackDualPat());
            }
        }

        //use weapon type 3
        else if(PlayerHandController.GetSetWeaponIndex1 == 3)
        {
            if(isDualAttack == false)
            {
                LongRangeAttackDualPat();
            }
        }
    }

    //Play Default "CloseRange" Dual Attack Pattern
    IEnumerator CloseRangeAttackDualPat()
    {
        PlayerAttackManager.isInAttack = true;
        isDualAttack = true;

        if(pat == 1f)
        {
            if(PlayerHandController.GetSetWeaponIndex1 == 2)
            {
                PlayerHandController.Weapon1.GetComponent<Sword_Type1>().AttackDualPat(PlayerHandController.GetSetWeaponIndex1, pat);
                PlayerHandController.Weapon1.GetComponent<Sword_Type1>().PlayerAttackPat = pat;
            }
            yield return new WaitForSeconds(0.3f);
            if(PlayerHandController.GetSetWeaponIndex2 == 2)
            {
                PlayerHandController.Weapon2.GetComponent<Sword_Type1>().AttackDualPat(PlayerHandController.GetSetWeaponIndex2, pat);
                PlayerHandController.Weapon2.GetComponent<Sword_Type1>().PlayerAttackPat = pat;
            }
            yield return new WaitForSeconds(0.2f);
            PlayerHand1.DelayAttack();
            PlayerHand2.DelayAttack();
        }
        else if(pat == 2f)
        {
            if(PlayerHandController.GetSetWeaponIndex1 == 2)
            {
                PlayerHandController.Weapon1.GetComponent<Sword_Type1>().AttackDualPat(PlayerHandController.GetSetWeaponIndex1, pat);
                PlayerHandController.Weapon1.GetComponent<Sword_Type1>().PlayerAttackPat = pat;
            }
            if(PlayerHandController.GetSetWeaponIndex2 == 2)
            {   
                PlayerHandController.Weapon2.GetComponent<Sword_Type1>().AttackDualPat(PlayerHandController.GetSetWeaponIndex2, pat);
                PlayerHandController.Weapon2.GetComponent<Sword_Type1>().PlayerAttackPat = pat;
            }
            yield return new WaitForSeconds(0.3f);
            if(PlayerHandController.GetSetWeaponIndex1 == 2)
            {
                PlayerHandController.Weapon1.GetComponent<Sword_Type1>().AttackDualPat(PlayerHandController.GetSetWeaponIndex1, pat);
                PlayerHandController.Weapon1.GetComponent<Sword_Type1>().PlayerAttackPat = pat;
            }
            if(PlayerHandController.GetSetWeaponIndex2 == 2)
            {   
                PlayerHandController.Weapon2.GetComponent<Sword_Type1>().AttackDualPat(PlayerHandController.GetSetWeaponIndex2, pat);
                PlayerHandController.Weapon2.GetComponent<Sword_Type1>().PlayerAttackPat = pat;
            }
            yield return new WaitForSeconds(0.35f);
            PlayerHand1.DelayAttack();
            PlayerHand2.DelayAttack();
        }
    }

    //Play Default "LongRange" Dual Attack Pattern
    void LongRangeAttackDualPat()
    {
        PlayerAttackManager.isInAttack = true;
        isDualAttack = true;
                    
        if(PlayerHandController.Weapon1.GetComponent<Gun_Type1>().BulletAmount <= 0)
        {
            PlayerHandController.Weapon1.GetComponent<Gun_Type1>().BulletAmount = 0f;
        }
        else
        {
            PlayerHandController.Weapon1.GetComponent<Gun_Type1>().AttackPat(PlayerHandController.GetSetWeaponIndex1);
            PlayerHandController.Weapon2.GetComponent<Gun_Type1>().AttackPat(PlayerHandController.GetSetWeaponIndex2);
            PlayerHandController.Weapon1.GetComponent<Gun_Type1>().BulletAmount--;
            PlayerHandController.Weapon2.GetComponent<Gun_Type1>().BulletAmount--;
        }
    }

    //Show and Update Bullet Amount
    public void SetBulletTxt()
    {
        try
        {
            if(PlayerHandController.GetSetWeaponIndex1 == 3 && PlayerHandController.GetuseHand == 1f)
            {
                BulletAmountTxt.text = PlayerHandController.Weapon1.GetComponent<Gun_Type1>().BulletAmount.ToString();    
            }
            else if(PlayerHandController.GetSetWeaponIndex2 == 3 && PlayerHandController.GetuseHand == 2f)
            {
                BulletAmountTxt.text = PlayerHandController.Weapon2.GetComponent<Gun_Type1>().BulletAmount.ToString();
            }
            else if(PlayerHandController.GetSetWeaponIndex1 == 3 && PlayerHandController.GetSetWeaponIndex2 == 3 && PlayerHandController.GetuseHand == 3f)
            {
                BulletAmountTxt.text = PlayerHandController.Weapon1.GetComponent<Gun_Type1>().BulletAmount.ToString();
            }
        }
        catch
        {

        }
    }

    //Alternative "CloseRange" Dual Attack Pattern
    public void SpecialDualAttackManager(float value)
    {
        if(PlayerHandController.GetSetWeaponIndex1 == 1 || PlayerHandController.GetSetWeaponIndex2 == 2)
        {
            PlayerHandController.Weapon1.GetComponent<Sword_Type1>().AttackPat(PlayerHandController.GetSetWeaponIndex1, value);
            PlayerHandController.Weapon2.GetComponent<Sword_Type1>().AttackPat(PlayerHandController.GetSetWeaponIndex2, value);
            PlayerHandController.Weapon1.GetComponent<Sword_Type1>().PlayerAttackPat = pat;
            PlayerHandController.Weapon2.GetComponent<Sword_Type1>().PlayerAttackPat = pat;
        }
    }
}
