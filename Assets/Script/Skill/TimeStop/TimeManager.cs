using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    #region private variable
    private TimeStopSkill TimeStopSkill;
    private TimeStopEffect_GM TimeStopEffect_GM;
    private bool isSlowMo, isCountdownEnd;
    private float TimeStopCountDown, CoolDownTime;
    private static bool isTimeStop;
    #endregion

    #region SerializeField variable
    [SerializeField] private Text TimeStopCountDownTxt, TimeCoolDownTxt;
    [SerializeField] private float FixTimeStopCountDown = 5f, FixCoolDownTime = 10f;
    #endregion

    #region Class property
    public static bool GetTimeStop()
    {
        return isTimeStop;
    }
    #endregion
    
    //Get all component
    void Awake()
    {
        TimeStopSkill = GameObject.Find("Player").GetComponent<TimeStopSkill>();
        TimeStopEffect_GM = GameObject.Find("GM").GetComponent<TimeStopEffect_GM>();
    }

    //Prepare all setting
    private void Start()
    {
        TimeCoolDownTxt.text = "";
        TimeStopCountDownTxt.text = "";
    }
    private void Update()
    {
        if(isTimeStop == true)
        {
            TimeStopCountDown -= Time.deltaTime;
            TimeStopCountDownTxt.text = Mathf.Round(TimeStopCountDown).ToString();

            if(TimeStopCountDown <= 0  && isSlowMo)
            {
                CoolDownTime = FixCoolDownTime;
                isSlowMo = false;
                TimeStopSkill.DoSlowMotion();
            }
        }
        if(TimeStopSkill.GetSetisReadyToUse == false && isCountdownEnd == true)
        {
            CoolDownTime -= Time.deltaTime;
            TimeCoolDownTxt.text = Mathf.Round(CoolDownTime).ToString();

            if(CoolDownTime <= 0f)
            {
                TimeCoolDownTxt.text = "";
                TimeStopSkill.GetSetisReadyToUse = true;
                isCountdownEnd = false;
            }
        }
    }
    public void DoStopTime()
    {
        isTimeStop = !isTimeStop;

        if(isTimeStop)
        {
            print("Time delta before StopTime = " + Time.deltaTime);
            print("Time Fixdelta before StopTime = " + Time.fixedDeltaTime);
            TimeStopSkill.GetSetisReadyToUse = true;
            isSlowMo = true;
            TimeStopCountDown = FixTimeStopCountDown;
            StartCoroutine(TimeStopEffect_GM.StopTimeGM());
        }
        else
        {
            print("Time delta after StopTime = " + Time.deltaTime);
            print("Time Fixdelta after StopTime = " + Time.fixedDeltaTime);
            TimeStopCountDownTxt.text = "";
            CoolDownTime = FixCoolDownTime;
            TimeStopSkill.GetSetisReadyToUse = false;
            isCountdownEnd = true;
            var objects = FindObjectsOfType<TimeStopEffect>();
            for(var i = 0; i < objects.Length; i++)
            {
                objects[i].GetComponent<TimeStopEffect>().CountinueTimeMethod();
            }
            TimeStopCountDown = 30f;
            StartCoroutine(TimeStopEffect_GM.CountinueTimeGM());
        }
    }
}
