using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static System.TimeZoneInfo;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsGamePaused;
    public GameObject PauseMenuUI;
    public GameObject Background;
    public GameObject OptionsMenu;
    public Animator transition;
    public float transitionTime = 1f;

    public void toggle()
    {
        Debug.Log("PAUSED");
        if (IsGamePaused)
            Resume();
        else
            Pause();
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Background.SetActive(false);
        OptionsMenu.SetActive(false);

        Time.timeScale = 1f;

        IsGamePaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Background.SetActive(true);

        Time.timeScale = 0f;

        IsGamePaused= true;
    }
    public void Menu()
    {
        StartCoroutine(LoadLevel(0));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        //play animation
        transition.SetTrigger("Start");

        //wait
        yield return new WaitForSecondsRealtime(transitionTime);

        //load scene
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelIndex);
    }
}
