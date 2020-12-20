using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    public Image BlackFadeEnding;
    public static GM instance;
    public bool isEsc, isDemo, isEnding;
    public GameObject MainUI, PauseUI;
    float saveTimeScale;
    private void Awake()
    {
        if(instance != null)
        {
            print("Bruhhh");
        }
        else
        {
            instance = this;
        }
        if(BlackFadeEnding != null)
        {
            BlackFadeEnding.CrossFadeAlpha(1f,0f,true);
        }
    }
    private void Start()
    {
        if(PauseUI != null)
        {
            PauseUI.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if(BlackFadeEnding != null)
        {
            BlackFadeEnding.CrossFadeAlpha(0f,1f,false);
            Invoke("DisActiveFadeBlack",1.5f);
        }    
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        if(isDemo)
        {
            var boss = FindObjectsOfType<Boss>();

            if(boss.Length == 0 && isEnding == false)
            {
                isEnding = true;
                ActiveFadeBlack();
                BlackFadeEnding.CrossFadeAlpha(1f,1f,false);
                Invoke("FadeToEnding",1.5f);
            }
        }
    }
    void DisActiveFadeBlack()
    {
        BlackFadeEnding.gameObject.SetActive(false);
    }
    void ActiveFadeBlack()
    {
        BlackFadeEnding.gameObject.SetActive(true);
    }
    void FadeToEnding()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void Pause()
    {
        isEsc = !isEsc;
        
        if(isEsc)
        {
            MainUI.SetActive(false);
            PauseUI.SetActive(true);
            saveTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            MainUI.SetActive(true);
            PauseUI.SetActive(false);
            Time.timeScale = saveTimeScale;
        }
    }
}
