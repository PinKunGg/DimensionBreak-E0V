using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmongUs_EasterEgg : MonoBehaviour
{
    public GameObject AmongUs_ImposterTxt;

    private void Start()
    {
        AmongUs_ImposterTxt.SetActive(false);    
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            AmongUs_ImposterTxt.SetActive(true);
        }    
    }
}
