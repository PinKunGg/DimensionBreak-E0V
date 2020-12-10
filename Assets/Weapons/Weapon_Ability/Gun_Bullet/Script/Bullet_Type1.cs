using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Type1 : MonoBehaviour
{
    public static float Damage, Knockback;
    float DestroyDistance = 20f, PlayerAttackPat = 1;
    Vector2 StartPos;

    public Vector2 SetStartPos
    {
        set
        {
            StartPos = value;
        }
    }
    public GameObject ImpactEffect;
    void Update()
    {
        float DistanceFromStartToNow = Vector2.Distance(StartPos,transform.position);
        
        if(DistanceFromStartToNow > DestroyDistance)
        {
            this.gameObject.SetActive(false);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("PlayerAttack") && !other.gameObject.CompareTag("Weapons") && !other.gameObject.CompareTag("Player") && !other.gameObject.CompareTag("Platform") && !other.gameObject.CompareTag("DeathEnemy"))
        {
            GameObject ImpactFX = Instantiate(ImpactEffect,transform.position,Quaternion.identity);
            Destroy(ImpactFX,0.2f);
            this.gameObject.SetActive(false);
        }

        if(other.gameObject.CompareTag("Enemy"))
        {
            SoundManager.instance.PlaySelectAudio(6);
            CameraShake.instance.ShakeCam(0.5f,0.1f);

            if(other.GetComponentInParent<Boss>() == true)
            {
                other.GetComponentInParent<Boss>().HpCal(Damage, 1f, PlayerAttackPat, 2f, Knockback);
            }
            else if(other.GetComponentInParent<Enemy>() == true)
            {
                other.GetComponentInParent<Enemy>().HpCal(Damage, 1f, PlayerAttackPat, 2f, Knockback);
            }
        }
        else if(other.gameObject.CompareTag("EnemyWeakPoint"))
        {
            SoundManager.instance.PlaySelectAudio(6);
            CameraShake.instance.ShakeCam(0.5f,0.1f);

            if(other.GetComponentInParent<Boss>() == true)
            {
                other.GetComponentInParent<Boss>().HpCal(Damage, 4f, PlayerAttackPat, 1f, Knockback);
            }
            else if(other.GetComponentInParent<Enemy>() == true)
            {
                other.GetComponentInParent<Enemy>().HpCal(Damage, 4f, PlayerAttackPat, 1f, Knockback);
            }
        }
    }
}