using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathFunnyText : MonoBehaviour
{
    public string[] StorageFunnyText;
    string FunnyText;
    int index;

    public string GetFunnyText
    {
        get
        {
            return FunnyText;
        }
    }
    public void FunnyTextGenerate()
    {
        index = 0;
        int rand = Random.Range(0,StorageFunnyText.Length);

        foreach(var item in StorageFunnyText)
        {
            if(rand == index)
            {
                FunnyText = StorageFunnyText[index];
            }
            else
            {
                index++;
            }
        }
    }
}
