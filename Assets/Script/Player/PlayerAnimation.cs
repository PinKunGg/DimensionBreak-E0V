using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    #region Private variable
    private Animator anima;
    private PlayerHandController PlayerHandController;
    private PlayerController PlayerController;
    private PlayerMovement PlayerMovement;
    private PlayerHand1 PlayerHand1;
    private PlayerHand2 PlayerHand2;
    private PlayerAttackManager PlayerAttackManager;
    #endregion

    //Get all component
    void Awake()
    {
        anima = GetComponentInChildren<Animator>();
        PlayerHandController = GetComponent<PlayerHandController>();
        PlayerController = GetComponent<PlayerController>();
        PlayerMovement = GetComponent<PlayerMovement>();
        PlayerHand1 = GetComponentInChildren<PlayerHand1>();
        PlayerHand2 = GetComponentInChildren<PlayerHand2>();
        PlayerAttackManager = GetComponent<PlayerAttackManager>();
    }

    //Prepare all setting
    void Start()
    {
        anima.SetBool("isFall",true);
        SetIdelAnima();
    }

    public void SetIdelAnima()
    {
        anima.SetBool("isIdle", true);
    }
    public void NotIdle()
    {
        anima.SetBool("isIdle", false);
    }
    public void WalkAnima(float value)
    {
        anima.SetFloat("isWalk",value);
    }
    public IEnumerator JumpAnima()
    {
        anima.SetBool("isJump",true);
        yield return new WaitForSeconds(0.3f);
        anima.SetBool("isJump",false);
    }
    public void StartFallAnima()
    {
        anima.SetBool("isFall",true);
    }
    public void StopFallAnima()
    {
        anima.SetBool("isFall",false);
    }
    public void TakeDamageAnima()
    {
        anima.SetTrigger("isDamage");
        PlayerController.SetisDamge = true;
        CancelInvoke();
        Invoke("ResetisDamage",0.5f);
    }
    void ResetisDamage()
    {
        PlayerController.SetisDamge = false;
    }
    public void DeathAnimation()
    {
        anima.SetTrigger("isDeath");
    }
}
