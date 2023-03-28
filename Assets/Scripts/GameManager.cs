using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject finalScreenCanvas;
    public Slider LetterSlider;
    public TextMeshProUGUI letterText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI glowChargesText;
    public ParticleSystem letterParticle;

    public int totalScuttleKilled = 0;
    public int totalEnemies = 0;
    public int bulletsFired = 0;
    public int bulletsHit = 0;
    public int damageTaken = 0;
    public int deaths = 0;
    public int glowChargesThrown = 0;
    public float startingTime;
    public float endingTime;

    public float finalScore;


    // Start is called before the first frame update
    void Start()
    {
        startingTime = Time.time;
        totalEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
        //LetterSlider = finalScreenCanvas.GetComponent<Slider>();
    }
    private void OnTriggerEnter()
    {
        levelOver();
    }

    public void levelOver()
    {
        endingTime = Time.time;

        CalculateScore();

        ChangeText();

        ChangeScoreSlider();
    }

    void CalculateScore()
    {
        finalScore = (float)(totalScuttleKilled) / totalEnemies;
    }

    void ChangeText()
    {
        double totalSeconds = endingTime- startingTime;
        float minutes = TimeSpan.FromSeconds(totalSeconds).Minutes;
        float secs = TimeSpan.FromSeconds(totalSeconds).Seconds;
        //string.Format("{1:00}:{2:00}", minutes, secs
        timeText.text = timeText.text + totalSeconds;
        enemiesKilledText.text = enemiesKilledText.text + totalScuttleKilled + "/" + totalEnemies;
        deathText.text = deathText.text + deaths;
        accuracyText.text = accuracyText.text + ((float)bulletsHit/bulletsFired) + "%";
        glowChargesText.text = glowChargesText.text + glowChargesThrown;
}

    void ChangeScoreSlider()
    {
        //Start anim for stats popup
        finalScreenCanvas.GetComponent<Animator>().SetTrigger("TextPopUp");

        //lerp to score
        StartCoroutine(MovingSlider());
    }

    IEnumerator MovingSlider()
    {
        float time = 0;
        float nextThreshold = .2f;
        while (time<=1f)
        {
            float value = Mathf.Lerp(0, finalScore, time);
            LetterSlider.value = value;


            if(value >= nextThreshold)
            {
                ChangeScoreLetter(value);
                nextThreshold += .2f;
            }

            yield return null;
            time += Time.deltaTime;
        }

        LetterSlider.value = finalScore;
        ChangeScoreLetter(finalScore);
    }


    void ChangeScoreLetter(float value)
    {
        if(value >= 1f)
            letterText.text = "S";

        else if (value >= .8f)
            letterText.text = "A";

        else if (value >= .6f)
            letterText.text = "B";

        else if (value >= .4f)
            letterText.text = "C";

        else if (value >= .2f)
            letterText.text = "D";

        else
            letterText.text = "F";

        Instantiate(letterParticle, letterText.transform.position, Quaternion.identity);
    }
}
