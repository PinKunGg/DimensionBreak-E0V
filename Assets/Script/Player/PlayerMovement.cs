using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region private variable
    private Rigidbody2D rb;
    private PlayerAnimation PlayerAnima;
    private PlayerAttackManager PlayerAttackManager;
    private PlayerController PlayerController;
    public Vector2 FlipPos, Arm1Pos, Arm2Pos;
    private Transform PlayerTrans;
    private float jumpForce;
    #endregion

    #region public variable
    public float MaxJumpCount = 1f;
    public Transform PlayerArm1, PlayerArm2;
    public SpriteRenderer PlayerGraphicBody,PlayerGraphicArm1,PlayerGraphicArm2, B_Storage1, B_Storage2, W_Storage1, W_Storage2;
    #endregion

    #region public static variable
    public static float speed, JumpCount;
    public static bool isMove, isFalling, isPlayWalkSound;
    #endregion

    #region Class Property
    public static float GetPlayerSpeed()
    {
        return speed;
    }
    public Rigidbody2D GetPlayerRb2d()
    {
        return rb;
    }
    #endregion

    //Get all component
    void Awake()
    {
        PlayerController = GetComponent<PlayerController>();
        PlayerAttackManager = GetComponent<PlayerAttackManager>();
        PlayerAnima = GetComponent<PlayerAnimation>();
        rb = GetComponent<Rigidbody2D>();
        jumpForce = Mathf.Pow(rb.mass,3f);
        PlayerTrans = GetComponent<Transform>();
    }
    
    //Prepare all setting
    void Start()
    {
        Arm1Pos = PlayerArm1.localPosition;
        Arm2Pos = PlayerArm2.localPosition;
        JumpCount = MaxJumpCount;
    }
    private void Update()
    {

        //If player is not death or game is not pause do this
        if(PlayerController.GetisDeath == false && GM.instance.isEsc == false)
        {
            if(PlayerController.GetisDamage  == false)
            {
                Movement();
            }
            else
            {
                rb.velocity = new Vector2(0f,rb.velocity.y);
            }

            //Flip player character
            FlipPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            if(FlipPos.x > 0f)
            {
                PlayerTrans.transform.localScale = new Vector3(1f,1f,1f);

                PlayerGraphicBody.sortingOrder = 0;

                //Arm1 - Font
                PlayerArm1.localPosition = new Vector2 (Arm1Pos.x,Arm1Pos.y);
                PlayerGraphicArm1.sortingOrder = 4;
                B_Storage1.sortingOrder = 1;
                W_Storage1.sortingOrder = 2;

                //Arm2 - Back
                PlayerArm2.localPosition = new Vector2 (Arm2Pos.x,Arm2Pos.y);
                PlayerGraphicArm2.sortingOrder = -4;
                B_Storage2.sortingOrder = -1;
                W_Storage2.sortingOrder = -2;
            }
            else
            {
                PlayerTrans.transform.localScale = new Vector3(-1f,1f,1f);

                PlayerGraphicBody.sortingOrder = 0;
            
                //Arm1 - Back
                PlayerArm1.localPosition = new Vector2 (Arm2Pos.x,Arm2Pos.y);
                PlayerGraphicArm1.sortingOrder = -4;
                B_Storage1.sortingOrder = -1;
                W_Storage1.sortingOrder = -2;

                //Arm2 - Font
                PlayerArm2.localPosition = new Vector2 (Arm1Pos.x,Arm1Pos.y);
                PlayerGraphicArm2.sortingOrder = 4;
                B_Storage2.sortingOrder = 1;
                W_Storage2.sortingOrder = 2;
            }
        }
    }

    //Make player move
    void Movement()
    {
        float h = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(h * speed, rb.velocity.y);
        
        if(Input.GetKey(KeyCode.D))
        {
            PlayerAnima.NotIdle();
            PlayerAnima.WalkAnima(1f);
            isMove = true;
        }
        else if(Input.GetKey(KeyCode.A))
        {
            PlayerAnima.NotIdle();
            PlayerAnima.WalkAnima(-1f);
            isMove = true;
        }
        else
        {
            PlayerAnima.SetIdelAnima();
            PlayerAnima.WalkAnima(0f);
            isMove = false;
            rb.velocity = new Vector2(0f,rb.velocity.y);
        }

        if(Input.GetButtonDown("Jump") && JumpCount > 0f)
        {
            PlayerAttackManager.isJumpAttackReady = true;
            JumpCount--;
            StartCoroutine(PlayerAnima.JumpAnima());
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Check if player touch "ground" thing or not
        if(other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform"))
        {
            PlayerAttackManager.isFallDirectAttackReady = true;
            PlayerAttackManager.isFallAttackReady = false;
            PlayerAttackManager.isJumpAttackReady = false;
            JumpCount = MaxJumpCount;
            isFalling = false;
            
            //Check player only touch top side or ground only
            for(int i = 0; i < other.contacts.Length; i++)
            {
                if(Vector2.Angle(other.contacts[i].normal,Vector2.up) <= 30f)
                {
                    PlayerAnima.StopFallAnima();
                }
            }
        }
    }
    void OnCollisionStay2D(Collision2D other)
    {
        //Animation Error Tarp
        if(other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform"))
        {
            if(isFalling == true)
            {
                isFalling = false;
                PlayerAnima.StopFallAnima();
            }

            //Play walk sound
            if((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && isPlayWalkSound == false)
            {
                isPlayWalkSound = true;
                Invoke("DelayWalkSound",0.2f);
                SoundManager.instance.PlaySelectAudio(9);
            }
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        //Check if player leave the ground or not
        if(other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform"))
        {
            PlayerAnima.StartFallAnima();
            isFalling = true;
        }
    }

    //Make walk sound delay to play another sound
    void DelayWalkSound()
    {
        isPlayWalkSound = false;
    }
}
