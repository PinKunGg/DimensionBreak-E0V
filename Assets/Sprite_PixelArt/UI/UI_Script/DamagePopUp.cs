using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopUp : MonoBehaviour
{
    Animator anima;
    public Text DamageTxt;

    private void Awake()
    {
        anima = GetComponentInChildren<Animator>();
        DamageTxt = anima.GetComponent<Text>();
    }
    private void Start()
    {
        AnimatorClipInfo[] clipInfo = anima.GetCurrentAnimatorClipInfo(0);
        Destroy(gameObject,clipInfo[0].clip.length);
    }

    public void SetDamagePopUp(float value)
    {
        DamageTxt.text = value.ToString();
    }
}
