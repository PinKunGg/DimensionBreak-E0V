using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopSkill : MonoBehaviour
{
    #region private variable
    private GM s_GM;
    private TimeManager timeManager;
    private float HowLongSlowMotion = 2f;
    private float HowSlowMotionEffect = 0.05f;
    private bool isSlowMotion = false;
    private bool isReadyToUse = true;
    #endregion

    #region SerializeField variable
    [SerializeField] private GameObject TimeStopUI, TimeStopUI_CoolDown;
    #endregion

    #region Class property
    public bool GetSetisReadyToUse
    {
        get
        {
            return isReadyToUse;
        }
        set
        {
            isReadyToUse = value;
        }
    }
    #endregion
    
    //Get all component
    private void Awake()
    {
        s_GM = GameObject.Find("GM").GetComponent<GM>();
        timeManager = GameObject.Find("GM").GetComponent<TimeManager>();    
    }
    void Update()
    {
        if(s_GM.isEsc == false)
        {
            if(Input.GetKeyDown(KeyCode.E) && isReadyToUse)
            {
                isReadyToUse = false;
                DoSlowMotion();
            }

            Time.timeScale += (1f / HowLongSlowMotion) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

            if(isSlowMotion == true && Time.timeScale >= 0.8f)
            {
                isSlowMotion = false;
                timeManager.DoStopTime();
            }
            if(Time.timeScale == 1f && Time.fixedDeltaTime != Time.deltaTime)
            {
                Time.fixedDeltaTime = Time.deltaTime;
            }
        }

        if(isReadyToUse == true)
        {
            TimeStopUI_CoolDown.SetActive(false);
            TimeStopUI.SetActive(true);
        }
        else
        {
            TimeStopUI_CoolDown.SetActive(true);
            TimeStopUI.SetActive(false);
        }
    }

    public void DoSlowMotion()
    {
        isSlowMotion = true;
        Time.timeScale = HowSlowMotionEffect;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        SoundManager.instance.PlaySFX(8);
    }
}
