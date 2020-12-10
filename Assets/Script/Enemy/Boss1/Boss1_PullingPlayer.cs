using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_PullingPlayer : MonoBehaviour
{
    private Vector2 IncommingHitPos;
    [SerializeField] private GameObject PullerTarget;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            IncommingHitPos = (other.transform.position - PullerTarget.transform.position).normalized;

            IncommingHitPos = new Vector2(PullerTarget.transform.position.x - other.transform.position.x, other.transform.position.y - PullerTarget.transform.position.y).normalized;
            other.transform.Translate(IncommingHitPos * 100f * Time.deltaTime);
        }
    }
}
