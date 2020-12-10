using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    public Light2D GlobalLight;
    public List<GameObject> LightObj;
    public bool StartDayTime;

    private void Start()
    {
        if(StartDayTime)
        {
            StartCoroutine(SunRise());
        }
        else
        {
            StartCoroutine(SunSet());
        }
    }
    void SetLight(bool value)
    {
        foreach(var item in LightObj)
        {
            if(item.GetComponentInChildren<Light2D>().enabled = value)
            {
                break;
            }
            else
            {
                item.GetComponentInChildren<Light2D>().enabled = value;
            }
        }
    }
    IEnumerator SunSet() //Time 2 Night
    {
        while(GlobalLight.intensity >= 0.5f)
        {
            GlobalLight.intensity -= 0.005f;

            if(GlobalLight.intensity <= 1f)
            {
                SetLight(true);
            }
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(NightTime());
        StopCoroutine(SunSet());
    }
    IEnumerator NightTime() //Stay In NightTime
    {
        yield return new WaitForSeconds(120f);
        StartCoroutine(SunRise());
    }
    IEnumerator SunRise() //Time 2 Day
    {
        while(GlobalLight.intensity <= 1.5f)
        {
            GlobalLight.intensity += 0.005f;

            if(GlobalLight.intensity >= 1f)
            {
                SetLight(false);
            }

            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(DayTime());
        StopCoroutine(SunRise());
    }
    IEnumerator DayTime() //Stay In DayTime
    {
        yield return new WaitForSeconds(120f);
        StartCoroutine(SunSet());
    }
}
