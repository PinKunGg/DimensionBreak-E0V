using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Type1 : MonoBehaviour
{
    public Weapon_SCAO s_Weapon;
    public Animator KevinGun_T1_Anima;
    public GameObject Bullet_Type1_Prefab, Fx;
    Transform BulletSpawnPos, NewWorldSpawnPos;
    Vector3 WorldSpawnPos;
    Quaternion WorldSpawnRot;
    float rotationZ, RandomPosNum, BulletSpeed = 30f;
    bool isReload;
    public float Damage, Knockback, RandomPos, FixBulletAmount, BulletAmount;
    private void Awake()
    {
        Damage = s_Weapon.Damage;
        Knockback = s_Weapon.Knoback;
        FixBulletAmount = s_Weapon.FixBullet;    
    }
    void OnEnable()
    {
        Fx.SetActive(false);
        BulletAmount = 0f;
    }
    public void AttackPat(int value)
    {
        if(value == 3)
        {
            if(BulletAmount > 0)
            {
                SpawnBullet();

                Fx.SetActive(true);
                Invoke("DelayFx",0.1f);
            }
        }
    }
    void DelayFx()
    {
        Fx.SetActive(false);
    }
    private void Update()
    {
        if(BulletAmount <= 0 && isReload == false)
        {
            isReload = true;
            KevinGun_T1_Anima.SetTrigger("Reload");
            Invoke("ReloadBullet",0.6f);
        }
        if(Input.GetKeyDown(KeyCode.R) && isReload == false)
        {
            BulletAmount = 0f;
        }
    }
    void ReloadBullet()
    {
        BulletAmount = FixBulletAmount;
        isReload = false;
    }
    void SpawnBullet()
    {
        SoundManager.instance.PlaySelectAudio(5);
        Bullet_Type1.Damage = Damage;
        Bullet_Type1.Knockback = Knockback;

        BulletSpawnPos = gameObject.transform.Find("BulletSpawn"); //Find "BulletSpawn" Position in PreFab GameObject
        NewWorldSpawnPos = BulletSpawnPos.transform; //Assign Prefab Position to World Position

        float RandomPosNum = Random.Range(-0.1f,0.1f); //Random Position y
        RandomPos = NewWorldSpawnPos.position.y + RandomPosNum; //Assign Random Position y to World Position

        GameObject bullet = ObjectPoller.instance.SpawnFromPool("BulletType1",new Vector2(NewWorldSpawnPos.position.x, RandomPos), NewWorldSpawnPos.rotation); //Call obj from Pooling and assign new position
        bullet.GetComponent<Bullet_Type1>().SetStartPos = NewWorldSpawnPos.position;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(BulletSpawnPos.up * BulletSpeed, ForceMode2D.Impulse);
    }
}
