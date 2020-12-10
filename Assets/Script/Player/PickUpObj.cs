using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObj : MonoBehaviour
{
    #region SerializeField variable
    [SerializeField] private Weapon_SCAO Weapon_SCAO;
    [SerializeField] private float HoverSpeed = 1f;
    [SerializeField] private float PlusSpeed = 1f;
    [SerializeField] private float MinusSpeed = -1f;
    [SerializeField] private float HightPoint, LowPoint;
    [SerializeField] private bool EnableShake = true;
    #endregion

    #region private variable
    private PlayerMovement PlayerMovement;
    private PlayerHandController PlayerHandController;
    private Vector2 StartPos;
    #endregion

    //Get all component
    void Awake()
    {
        PlayerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        PlayerHandController = GameObject.Find("Player").GetComponent<PlayerHandController>();
    }

    //Prepare all setting
    void Start()
    {
        StartPos = transform.position;
        HightPoint = Random.Range(0.3f,0.5f);
        LowPoint = Random.Range(0.3f,0.5f);
    }

    //Delay collider check if not, delay when Player drop, it will pick up instantly
    private void OnEnable()
    {
        this.GetComponent<Collider2D>().enabled = false;
        Invoke("CanPickUp",1f);
    }

    //enable pickup collider
    void CanPickUp()
    {
        this.GetComponent<Collider2D>().enabled = true;
    }
    void Update()
    {
        //check if timeskill is enable
        if(TimeManager.GetTimeStop() == true)
        {
            if(EnableShake != false)
            {
                EnableShake = false;
            }
        }
        else
        {
            if(EnableShake != true)
            {
                EnableShake = true;
            }
        }

        //if true start Hover
        if(EnableShake)
        {
            HoverObj();
        }
    }
    void HoverObj()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + HoverSpeed * Time.deltaTime);

        if(transform.position.y >= StartPos.y + HightPoint) //HightPoint
        {
            HoverSpeed = MinusSpeed;
        }
        else if(transform.position.y <= StartPos.y - LowPoint) //LowPoint
        {
            HoverSpeed = PlusSpeed;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(PlayerHandController.GetItemNumberInHand == 0f)
            {
                PlayerHandController.SetWeapon1Prefab = Weapon_SCAO.Obj_Prefab;
                PlayerHandController.GetSetWeaponIndex1 = Weapon_SCAO.WeaponIndex;
                PlayerHandController.SetWeapon1Use = Weapon_SCAO.icon_Use;
                PlayerHandController.SetWeapon1NotUse = Weapon_SCAO.icon_NotUse;
                PlayerHandController.SetDual1 = Weapon_SCAO.DualWeapon;
                PlayerHandController.GetSetObjType1 = Weapon_SCAO.ObjectType;
                PlayerHandController.Weapon1DropPrafab = Weapon_SCAO.Drop_Prefab;
            }
            else if(PlayerHandController.GetItemNumberInHand == 1f)
            {
                PlayerHandController.SetWeapon2Prefab = Weapon_SCAO.Obj_Prefab;
                PlayerHandController.GetSetWeaponIndex2 = Weapon_SCAO.WeaponIndex;
                PlayerHandController.SetWeapon2Use = Weapon_SCAO.icon_Use;
                PlayerHandController.SetWeapon2NotUse = Weapon_SCAO.icon_NotUse;
                PlayerHandController.SetDual2 = Weapon_SCAO.DualWeapon;
                PlayerHandController.GetSetObjType2 = Weapon_SCAO.ObjectType;
                PlayerHandController.Weapon2DropPrafab = Weapon_SCAO.Drop_Prefab;
            }
            
            PlayerHandController.AttatchItemInHand();

            if(PlayerHandController.GetItemNumberInHand <= 2f)
            {
                Destroy(gameObject);
            }
        }
    }
}
