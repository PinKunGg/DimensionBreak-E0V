using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopEffect : MonoBehaviour
{
    #region private variable
    private Animator anima;
    private Rigidbody2D rb;
    private Vector2 VelocityOfObj;
    private float MagnitudeOfObj, TimeBeforeAffectedTimer;
    #endregion

    #region SerializeField variable
    [SerializeField] private bool isBeAffected, isStop;
    [SerializeField] private Collider2D AttackCollider;
    #endregion

    #region Class property
    public bool GetisStop
    {
        get
        {
            return isStop;
        }
    }
    #endregion
    
    //Get all component
    private void Awake()
    {
        anima = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        TimeBeforeAffectedTimer -= Time.deltaTime;

        if(TimeBeforeAffectedTimer <= 0f)
        {
            isBeAffected = true;
        }

        if(isBeAffected && TimeManager.GetTimeStop() == true && !isStop)
        {
            Debug.Log("ZaWaRuDo!");
            
            if(rb.velocity.magnitude >= 0f)
            {
                VelocityOfObj = rb.velocity.normalized; //records direction of movement
                MagnitudeOfObj = rb.velocity.magnitude; // records magitude of movement
            }

            if(anima != null)
            {
                //Freeze Animation
                anima.enabled = false;
            }
            if(AttackCollider != null)
            {
                //Disable AttackCollider
                AttackCollider.enabled = false;
            }

            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
            isStop = true;
        }
    }
    public void CountinueTimeMethod() //Make Obj Move Again;
    {
        Debug.Log("Time back to normal");
        isStop = false;
        rb.isKinematic = false;
        rb.velocity = VelocityOfObj * MagnitudeOfObj; //Adds back the recorded velocity when time continues

        if(this.CompareTag("PlayerAttack"))
        {
            if(MagnitudeOfObj == 0 && VelocityOfObj == Vector2.zero)
            {
                print("bruh, BulletBug");
                this.gameObject.SetActive(false);
            }
        }
        
        if(anima != null)
        {
            //UnFreeze Animation
            anima.enabled = true;
        }
        if(AttackCollider != null)
        {
            //Enable AttackCollider
            AttackCollider.enabled = true;
        }
    }
    public void SelfDeath()
    {
        isStop = false;
        isBeAffected = false;

        VelocityOfObj = Vector2.zero;
        MagnitudeOfObj = 0f;
        
        rb.isKinematic = false;

        if(anima != null)
        {
            //UnFreeze Animation
            anima.enabled = true;
        }
        if(AttackCollider != null)
        {
            //Enable AttackCollider
            AttackCollider.enabled = true;
        }
    }
}
