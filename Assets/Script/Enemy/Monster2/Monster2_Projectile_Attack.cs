using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster2_Projectile_Attack : MonoBehaviour
{
    #region private variable
    private Transform NewWorldSpawnPos, Player, LookAtPos;
    private float BulletSpeed = 25f, Damage;
    #endregion

    #region SerializeField variable
    [SerializeField] private Transform ProjectileSpawnPos;
    #endregion

    #region Class property
    public float SetProjectileDamage
    {
        set
        {
            Damage = value;
        }
    }
    #endregion
    
    //Get all component
    private void Awake()
    {
        Player = GameObject.Find("Player").transform;    
    }

    private void Update()
    {
        Vector3 dir = Player.position - ProjectileSpawnPos.position;
        float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg - 90f;
        ProjectileSpawnPos.rotation = Quaternion.Euler(0f,0f,angle);
    }
    public void SpawnProjectile1()
    {
        GameObject bullet = ObjectPoller.instance.SpawnFromPool("Mon2Projectile1",ProjectileSpawnPos.position, Quaternion.identity); //Call obj from Pooling and assign new position
        bullet.GetComponent<Projectile_Mon2>().SetStartPos = ProjectileSpawnPos.position;
        bullet.GetComponent<Projectile_Mon2>().SetValue(Damage,1.2f,1f,2f, 0f);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(ProjectileSpawnPos.up * BulletSpeed, ForceMode2D.Impulse);
    }

    public void SpawnProjectile2()
    {
        GameObject bullet = ObjectPoller.instance.SpawnFromPool("Mon2Projectile2",ProjectileSpawnPos.position, Quaternion.identity); //Call obj from Pooling and assign new position
        bullet.GetComponent<Projectile_Mon2>().SetStartPos = ProjectileSpawnPos.position;
        bullet.GetComponent<Projectile_Mon2>().SetValue(Damage,1f,2f,2f, 20f);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(ProjectileSpawnPos.up * BulletSpeed, ForceMode2D.Impulse);
    }
}
