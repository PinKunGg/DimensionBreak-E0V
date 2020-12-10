using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZone : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            var obj = FindObjectsOfType<Enemy>();

            if(obj.Length == 0)
            {
                this.GetComponent<Collider2D>().enabled = false;
            }
        }    
    }
}
