using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathUI : MonoBehaviour
{
    public DeathFunnyText DeathFunnyText;
    public Text FunnyText;

    private void OnEnable()
    {
        DeathFunnyText.FunnyTextGenerate();

        FunnyText.text = DeathFunnyText.GetFunnyText;    
    }
}
