using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Animator transition;
    public Character characterScript;

    public GameObject finalScreenCanvas;
    public Slider LetterSlider;
    public TextMeshProUGUI letterText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI deathText;
    public TextMeshProUGUI accuracyText;
    public TextMeshProUGUI glowChargesText;
    public ParticleSystem letterParticle;

    [Header("Target numbers")]
    public float expectedTimeTaken;
    public float expectedHitsTaken;

    [Header("Score Readout")]
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
        finalScreenCanvas.SetActive(false);
        //LetterSlider = finalScreenCanvas.GetComponent<Slider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
            levelOver();
    }

    public void levelOver()
    {
        characterScript.SwitchToUI();

        finalScreenCanvas.SetActive(true);

        endingTime = Time.time;

        CalculateScore();

        ChangeText();

        ChangeScoreSlider();
    }

    public void RestartLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        //play animation
        transition.SetTrigger("Start");

        //wait
        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadScene("Level 01");

        //load scene
        Time.timeScale = 1f;

        //Reinable player input
        characterScript.SwitchToPlayer();
    }

    void CalculateScore()
    {
        float enemyScorePercentage = .4f;
        float hitsScorePercentage = .4f;
        float timeScorePercentage = .2f;

        float enemiesScore = (float)(totalScuttleKilled) / totalEnemies * enemyScorePercentage;
        enemiesScore = Mathf.Clamp(enemiesScore, 0f, enemyScorePercentage);

        float damageScore = expectedHitsTaken / damageTaken * hitsScorePercentage;
        damageScore = Mathf.Clamp(damageScore, 0f, hitsScorePercentage);

        float timeScore = (float)expectedTimeTaken/ (endingTime - startingTime) * timeScorePercentage;
        timeScore = Mathf.Clamp(timeScore, 0f, timeScorePercentage);

        Debug.Log("e " + enemiesScore + " d " + damageScore + " t " + timeScore);

        finalScore = enemiesScore + damageScore + timeScore;
    }

    void ChangeText()
    {
        double totalSeconds = endingTime- startingTime;
        float minutes = TimeSpan.FromSeconds(totalSeconds).Minutes;
        float secs = TimeSpan.FromSeconds(totalSeconds).Seconds;
        //string.Format("{1:00}:{2:00}", minutes, secs
        timeText.text = timeText.text + minutes + ":" + secs;
        enemiesKilledText.text = enemiesKilledText.text + totalScuttleKilled + "/" + totalEnemies;
        deathText.text = deathText.text + deaths;
        accuracyText.text = accuracyText.text + Mathf.Round(((float)bulletsHit/bulletsFired) * 100.0f) *.01f + "%";
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

        //Instantiate(letterParticle, letterText.transform.position, Quaternion.identity);
    }
}
