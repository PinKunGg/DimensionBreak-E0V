using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Type1 : MonoBehaviour
{
    public Weapon_SCAO s_Weapon;
    public Animator KevinSword_T1_Anima;
    public GameObject AttackHitFx;
    public float Damage, Knockback, PlayerAttackPat;
    private void Awake()
    {
        Damage = s_Weapon.Damage;
        Knockback = s_Weapon.Knoback;    
    }
    public void AttackPat(int Index, float patvalue)
    {
        SoundManager.instance.PlaySelectAudio(7);
        
        if(Index == 1)
        {
            KevinSword_T1_Anima.SetTrigger("Sword1_AttackPat" + patvalue);
        }
        if(Index == 2)
        {
            KevinSword_T1_Anima.SetTrigger("Sword2_AttackPat" + patvalue);
        }
    }
    public void AttackDualPat(int Index, float patvalue)
    {
        SoundManager.instance.PlaySelectAudio(7);
        
        if(Index == 2)
        {
            KevinSword_T1_Anima.SetTrigger("Sword2_Attack_Dual_Pat" + patvalue);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            SoundManager.instance.PlaySelectAudio(4);
            if(PlayerAttackPat == 1 || PlayerAttackPat == 2 || PlayerAttackPat == 5)
            {
                CameraShake.instance.ShakeCam(1f,0.1f);
            }
            else if(PlayerAttackPat == 3 || PlayerAttackPat == 4)
            {
                CameraShake.instance.ShakeCam(3f,0.1f);
            }

            Vector2 point = other.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);

            int RotRand = Random.Range(-90,90);
            GameObject HitPar = Instantiate(AttackHitFx,point,Quaternion.Euler(RotRand,90f,90f));
            Destroy(HitPar,1f);

            if(other.GetComponent<Boss>() == true)
            {
                other.GetComponent<Boss>().HpCal(Damage, 1f, PlayerAttackPat, 1f, Knockback);
            }
            else if(other.GetComponent<Enemy>() == true)
            {
                other.GetComponent<Enemy>().HpCal(Damage, 1f, PlayerAttackPat, 1f, Knockback);
            }
        }
        else if(other.gameObject.CompareTag("EnemyWeakPoint"))
        {
            SoundManager.instance.PlaySelectAudio(4);
            print("WeakPoint Hit");

            if(PlayerAttackPat == 1 || PlayerAttackPat == 2 || PlayerAttackPat == 5)
            {
                CameraShake.instance.ShakeCam(1f,0.1f);
            }
            else if(PlayerAttackPat == 3 || PlayerAttackPat == 4)
            {
                CameraShake.instance.ShakeCam(3f,0.1f);
            }
            
            Vector2 point = other.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);

            int RotRand = Random.Range(-90,90);
            GameObject HitPar = Instantiate(AttackHitFx,point,Quaternion.Euler(RotRand,90f,90f));
            Destroy(HitPar,1f);
            
            if(other.GetComponentInParent<Boss>() == true)
            {
                other.GetComponentInParent<Boss>().HpCal(Damage, 3f, PlayerAttackPat, 1f, Knockback);
            }
            else if(other.GetComponentInParent<Enemy>() == true)
            {
                other.GetComponentInParent<Enemy>().HpCal(Damage, 3f, PlayerAttackPat, 1f, Knockback);
            }
        }
    }
}
