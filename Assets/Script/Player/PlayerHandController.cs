using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
    #region private variable
    private PlayerMovement PlayerMovement;
    private PlayerAttackManager PlayerAttackManager;
    private PlayerController PlayerController;
    private PlayerHand1 PlayerHand1;
    private PlayerHand2 PlayerHand2;
    private float PatternCheck = 1f;
    private float useHand = 1f; //1 -> Arm1, 2 -> Arm2 , 3 -> if same weapons dual Arm
    private float MaxPattern = 3f;
    private float itemNumInHand = 0f;
    private bool haveItemInHand, SpawnWeaponInHand1, SpawnWeaponInHand2, isDeathDrop;
    private string ObjType1, ObjType2;
    private bool Dual1, Dual2;
    private Sprite WeaponInHand1_Use, WeaponInHand1_NotUse, WeaponInHand2_Use, WeaponInHand2_NotUse;
    [SerializeField] private GameObject Weapon1Prefab,Weapon2Prefab,InHandObj1,InHandObj2;
    private int Weapon1_Index,Weapon2_Index;
    #endregion

    #region public variable    
    public static GameObject Weapon1, Weapon2, Weapon1DropPrafab, Weapon2DropPrafab;
    #endregion
    
    #region SerializeField variable
    [SerializeField] private GameObject BulletShow;
    [SerializeField] private SpriteRenderer B_Storage1, B_Storage2, W_Storage1, W_Storage2;
    #endregion

    #region Class Property
    #region Get Property
    public float GetuseHand
    {
        get
        {
            return useHand;
        }
    }
    public float GetItemNumberInHand
    {
        get
        {
            return itemNumInHand;
        }
    }
    #endregion
    #region Set Property
    public GameObject SetWeapon1Prefab
    {
        set
        {
            Weapon1Prefab = value;
        }
    }
    public GameObject SetWeapon2Prefab
    {
        set
        {
            Weapon2Prefab = value;
        }
    }
    public Sprite SetWeapon1Use
    {
        set
        {
            WeaponInHand1_Use = value;
        }
    }
    public Sprite SetWeapon1NotUse
    {
        set
        {
            WeaponInHand1_NotUse = value;
        }
    }
    public Sprite SetWeapon2Use
    {
        set
        {
            WeaponInHand2_Use = value;
        }
    }
    public Sprite SetWeapon2NotUse
    {
        set
        {
            WeaponInHand2_NotUse = value;
        }
    }
    public bool SetDual1
    {
        set
        {
            Dual1 = value;
        }
    }
    public bool SetDual2
    {
        set
        {
            Dual2 = value;
        }
    }
    #endregion
    #region GetSet Property
    public int GetSetWeaponIndex1
    {
        get
        {
            return Weapon1_Index;
        }
        set
        {
            Weapon1_Index = value;
        }
    }
    public int GetSetWeaponIndex2
    {
        get
        {
            return Weapon2_Index;
        }
        set
        {
            Weapon2_Index = value;
        }
    }
    public string GetSetObjType1
    {
        get
        {
            return ObjType1;
        }
        set
        {
            ObjType1 = value;
        }
    }
    public string GetSetObjType2
    {
        get
        {
            return ObjType2;
        }
        set
        {
            ObjType2 = value;
        }
    }
    #endregion
    #endregion

    //Get all component
    void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
        PlayerMovement = GetComponentInParent<PlayerMovement>();
        PlayerHand1 = GetComponentInChildren<PlayerHand1>();
        PlayerHand2 = GetComponentInChildren<PlayerHand2>();
        PlayerAttackManager = GetComponent<PlayerAttackManager>();
    }

    //Prepare all setting
    private void Start()
    {
        BulletShow.SetActive(false);
        isDeathDrop = false;
    }
    void Update()
    {
        if(PlayerController.GetisDeath == false)
        {
            SwitchWeaponInaHand();
            DropWeaponInHand();
        }
        else if(PlayerController.GetisDeath == true && isDeathDrop == false)
        {
            isDeathDrop = true;
            InHandObj1.SetActive(true);
            InHandObj2.SetActive(true);

            DropWeapon1Prefab();
            DropWeapon2Prefab();
        }
    }

    //Setup item
    public void AttatchItemInHand()
    {
        haveItemInHand = true;
        itemNumInHand++;

        InHandWeapon();
    }
    void InHandWeapon()
    {
        if(useHand == 1f)
        {   
            //Setup item in hand1
            InHandObj1.SetActive(true);
            InHandObj2.SetActive(false);
            PlayerHand1.SetupHand1Item();

            if(ObjType1 != null)
            {
                if(ObjType1.ToUpper() == "CLOSE_RANGE")
                {
                    //Set hand1 position for "close range" weapon
                    BulletShow.SetActive(false);
                    InHandObj1.transform.localPosition = new Vector2(0.08f,0.12f);
                    InHandObj1.transform.localRotation = Quaternion.Euler(0f,0f,-200f);

                    //spawn weapon
                    if(SpawnWeaponInHand1 == false)
                    {
                        SpawnWeaponInHand1 = true;
                        Weapon1 = Instantiate(Weapon1Prefab,InHandObj1.transform.position, InHandObj1.transform.rotation);
                        Weapon1.transform.parent = InHandObj1.transform;
                        Weapon1.transform.localScale = new Vector2(1f,1f);
                    }
                
                    B_Storage1.sprite = WeaponInHand1_Use;
                }
                if(ObjType1.ToUpper() == "LONG_RANGE")
                {
                    //Set hand1 position for "long range" weapon
                    BulletShow.SetActive(true);
                    InHandObj1.transform.localPosition = new Vector2(0.08f,0.05f);
                    InHandObj1.transform.localRotation = Quaternion.Euler(0f,0f,0f);

                    //spawn weapon
                    if(SpawnWeaponInHand1 == false)
                    {
                        SpawnWeaponInHand1 = true;
                        Weapon1 = Instantiate(Weapon1Prefab,InHandObj1.transform.position, InHandObj1.transform.rotation);
                        Weapon1.transform.parent = InHandObj1.transform;
                        Weapon1.transform.localScale = new Vector2(1f,1f);
                        Weapon1.GetComponent<Gun_Type1>().BulletAmount = 0f;
                    }

                    W_Storage1.sprite = WeaponInHand1_Use;
                }
            }
            
            if(ObjType2 != null)
            {
                //Set sprite for weapon2 in notuse mode
                if(ObjType2.ToUpper() == "CLOSE_RANGE")
                {
                    B_Storage2.sprite = WeaponInHand2_NotUse;
                }
                if (ObjType2.ToUpper() == "LONG_RANGE")
                {
                    W_Storage2.sprite = WeaponInHand2_NotUse;
                }
            }
            else
            {
                return;
            }
        }
        else if(useHand == 2f)
        {
            //Set item in hand2
            InHandObj1.SetActive(false);
            InHandObj2.SetActive(true);
            PlayerHand2.SetupHand2Item();

            if(ObjType2 != null)
            {
                if(ObjType2.ToUpper() == "CLOSE_RANGE")
                {   
                    //Set hand2 position for "close range" weapon
                    BulletShow.SetActive(false);
                    InHandObj2.gameObject.transform.localPosition = new Vector2(0.03f,0.15f);
                    InHandObj2.gameObject.transform.localRotation = Quaternion.Euler(0f,0f,-200f);

                    //spawn weapon
                    if(SpawnWeaponInHand2 == false)
                    {
                        SpawnWeaponInHand2 = true;
                        Weapon2 = Instantiate(Weapon2Prefab,InHandObj2.transform.position, InHandObj2.transform.rotation);
                        Weapon2.transform.parent = InHandObj2.transform;
                        Weapon2.transform.localScale = new Vector2(1f,1f);
                    }

                    B_Storage2.sprite = WeaponInHand2_Use;
                }
                if (ObjType2.ToUpper() == "LONG_RANGE")
                {
                    //Set hand2 position for "long range" weapon
                    BulletShow.SetActive(true);
                    InHandObj2.gameObject.transform.localPosition = new Vector2(0.03f,0.08f);
                    InHandObj2.gameObject.transform.localRotation = Quaternion.Euler(0f,0f,0f);

                    //spawn weapon
                    if(SpawnWeaponInHand2 == false)
                    {
                        SpawnWeaponInHand2 = true;
                        Weapon2 = Instantiate(Weapon2Prefab,InHandObj2.transform.position, InHandObj2.transform.rotation);
                        Weapon2.transform.parent = InHandObj2.transform;
                        Weapon2.transform.localScale = new Vector2(1f,1f);
                        Weapon2.GetComponent<Gun_Type1>().BulletAmount = 0f;
                    }

                    W_Storage2.sprite = WeaponInHand2_Use;
                }
            }

            if(ObjType1 != null)
            {
                //Set sprite for weapon1 in not use mode
                if(ObjType1.ToUpper() == "CLOSE_RANGE")
                {
                    B_Storage1.sprite = WeaponInHand1_NotUse;
                }
                if(ObjType1.ToUpper() == "LONG_RANGE")
                {
                    W_Storage1.sprite = WeaponInHand1_NotUse;
                }
            }
            else
            {
                return;
            }
        }
        else if(useHand == 3)
        {
            //Set item in dual hand if it could
            if(ObjType1.ToUpper() == "CLOSE_RANGE")
            {
                InHandObj1.SetActive(true);
                B_Storage1.sprite = WeaponInHand1_Use;
            }
            if(ObjType1.ToUpper() == "LONG_RANGE")
            {
                BulletShow.SetActive(true);
                InHandObj1.SetActive(true);
                W_Storage1.sprite = WeaponInHand1_Use;
                Weapon1.GetComponent<Gun_Type1>().BulletAmount = 0f;
            }
            if(ObjType2.ToUpper() == "CLOSE_RANGE")
            {
                InHandObj2.SetActive(true);
                B_Storage2.sprite = WeaponInHand2_Use;
            }
            if (ObjType2.ToUpper() == "LONG_RANGE")
            {
                BulletShow.SetActive(true);
                InHandObj2.SetActive(true);
                W_Storage2.sprite = WeaponInHand2_Use;
                Weapon2.GetComponent<Gun_Type1>().BulletAmount = 0f;
            }
        }
    }
    void SwitchWeaponInaHand()
    {
        if((Input.GetKeyDown(KeyCode.F)) && haveItemInHand == true && PlayerHand1.GetisGuard == false && PlayerHand2.GetisGuard == false)
        {
            print("PatternCheck = " + PatternCheck);
            PatternCheck++;
            PlayerAttackManager.RestAttack();
            
            if(PatternCheck == MaxPattern)
            {
                if(Dual1 == true && Dual2 == true && ObjType1 == ObjType2)
                {
                    useHand = PatternCheck;
                }
                else
                {
                    PatternCheck = 1f;
                    useHand = PatternCheck;
                }
            }
            else if (PatternCheck < MaxPattern)
            {
                useHand = PatternCheck;
            }
            else if (PatternCheck > MaxPattern)
            {
                PatternCheck = 1f;
                useHand = PatternCheck;
            }

            InHandWeapon();
        }
    }
    void DropWeaponInHand()
    {
        if(Input.GetKeyDown(KeyCode.Z) && haveItemInHand == true)
        {
            if(useHand == 1f)
            {
                DropWeapon1Prefab();
            }
            else if(useHand == 2f)
            {
                DropWeapon2Prefab();
            }
        }

        if(ObjType1 == null && ObjType2 == null)
        {
            haveItemInHand = false;
            itemNumInHand = 0f;
        }
        else if(ObjType1 == null)
        {
            itemNumInHand = 0f;
        }
        else if(ObjType2 == null)
        {
            itemNumInHand = 1f;
        }
        else if(ObjType2 != null)
        {
            itemNumInHand = 2f;
        }
    }
    void DropWeapon1Prefab()
    {
        if(Weapon1DropPrafab != null)
        {
            Instantiate(Weapon1DropPrafab,new Vector2(transform.position.x + Random.Range(-0.5f,0.5f),transform.position.y + Random.Range(0.3f,0.5f)),Quaternion.identity);
            Weapon1DropPrafab = null;
            Destroy(Weapon1);
            SpawnWeaponInHand1 = false;
            B_Storage1.sprite = null;
            W_Storage1.sprite = null;
            WeaponInHand1_Use = null;
            WeaponInHand1_NotUse = null;
            ObjType1 = null;
            PlayerHand1.ObjType = null;
            Dual1 = false;
            BulletShow.SetActive(false);
            PlayerHand1.ResetArm();
        }
    }
    void DropWeapon2Prefab()
    {
        if(Weapon2DropPrafab != null)
        {
            Instantiate(Weapon2DropPrafab,new Vector2(transform.position.x + Random.Range(-0.5f,0.5f),transform.position.y + Random.Range(0.3f,0.5f)),Quaternion.identity);
            Weapon2DropPrafab = null;
            Destroy(Weapon2);
            SpawnWeaponInHand2 = false;
            B_Storage2.sprite = null;
            W_Storage2.sprite = null;
            WeaponInHand2_Use = null;
            WeaponInHand2_NotUse = null;
            ObjType2 = null;
            PlayerHand2.ObjType2 = null;
            Dual2 = false;
            BulletShow.SetActive(false);
            PlayerHand2.ResetArm();
        }
    }
}
