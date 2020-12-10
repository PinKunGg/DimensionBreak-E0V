using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    PlatformEffector2D effector2D;
    float waitTime, SinglewaitTime;

    private void Start()
    {
        waitTime = 1f;
        SinglewaitTime = 0.3f;
        effector2D = GetComponent<PlatformEffector2D>();    
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.S))
        {
            waitTime = 1f;
        }
        if(Input.GetKey(KeyCode.S))
        {
            if(waitTime <= 0)
            {
                effector2D.rotationalOffset = 180f;
                waitTime = 1f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
        if(Input.GetButtonDown("Jump"))
        {
            effector2D.rotationalOffset = 0f;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(Input.GetKey(KeyCode.S))
            {
                if(SinglewaitTime <= 0)
                {
                    effector2D.rotationalOffset = 180f;
                    SinglewaitTime = 0.3f;
                }
                else
                {
                    SinglewaitTime -= Time.deltaTime;
                }
            }
            if(Input.GetButtonDown("Jump"))
            {
                effector2D.rotationalOffset = 0f;
            }
        }    
    }
}
