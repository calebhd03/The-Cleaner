using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;
using UnityEngine.TextCore.Text;

public class PauseMenu : MonoBehaviour
{
    public static bool IsGamePaused;
    public GameObject PauseMenuUI;
    public GameObject Background;
    public GameObject OptionsMenu;
    public Animator transition;
    public Character character;
    public float transitionTime = 1f;

    public String MixerName;

    private float playerVolume = 0f;
    private float enemyVolume = 0f;

    //returns true if paused
    //returns false if resume
    public bool toggle()
    {
        Debug.Log("PAUSED");
        if (IsGamePaused)
        {
            Resume();
            return false;
        }
        else
        {
            Pause();
            return true;
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Background.SetActive(false);
        OptionsMenu.SetActive(false);

        character.SwitchToPlayer();

        //Resume Gameplay
        Time.timeScale = 1f;
        IsGamePaused = false;
    }

    void Pause()
    {
        PauseMenuUI.SetActive(true);
        Background.SetActive(true);

        //Pause Gameplay
        Time.timeScale = 0f;
        IsGamePaused = true;
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
