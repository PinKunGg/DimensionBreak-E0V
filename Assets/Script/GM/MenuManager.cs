using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject Loading;
    public GM GM;
    int SceneNum;
    private void Start()
    {
        Loading.SetActive(false);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.deltaTime;
        SceneNum = SceneManager.GetActiveScene().buildIndex;   
    }
    public void ResumeButton()
    {
        GM.Pause();
    }
    public void RetryButton()
    {
        Loading.SetActive(true);
        SceneManager.LoadSceneAsync(SceneNum);
    }
    public void MenuButton()
    {
        Loading.SetActive(true);
        SceneManager.LoadSceneAsync(0);
    }
    public void StageSelect()
    {
        Loading.SetActive(true);
        SceneManager.LoadSceneAsync(1);
    }
    public void Starge1Button()
    {
        Loading.SetActive(true);
        SceneManager.LoadSceneAsync(2);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
