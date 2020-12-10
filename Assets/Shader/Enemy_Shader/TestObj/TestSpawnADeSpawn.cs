using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawnADeSpawn : MonoBehaviour
{
    [SerializeField] Material Mat1;
    [SerializeField] SpriteRenderer Rend1;
    [SerializeField] Animator anima;

    bool isSpawnFx, isDeSpawnFx;
    float FadeValue;

    private void Start()
    {
        Mat1 = Rend1.material;
        Mat1.SetFloat("_Fade",0f);
        //isSpawnFx = true;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            isSpawnFx = true;
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            isDeSpawnFx = true;
        } 
        else if(Input.GetKeyDown(KeyCode.C))
        {
            Animation1();
        }
        if(isSpawnFx)
        {
            FadeValue = Mathf.Clamp01(FadeValue + 1f * Time.deltaTime);
            Mat1.SetFloat("_Fade",FadeValue);

            if(FadeValue >= 1f)
            {
                isSpawnFx = false;
            }
        }
        else if(isDeSpawnFx)
        {
            FadeValue = Mathf.Clamp01(FadeValue - 0.5f * Time.deltaTime);
            Mat1.SetFloat("_Fade",FadeValue);;

            if(FadeValue <= 0f)
            {
                isDeSpawnFx = false;
            }
        }
    }

    void Animation1()
    {
        anima.SetTrigger("TestObj_Anima1");
    }
}
