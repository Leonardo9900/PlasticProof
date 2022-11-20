using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerOffline : MonoBehaviour
{
    public Text scoreText;
    public int maxPollution = 100;
    public int currentPollution;
    public PollutionBar pollBar;
    public GameObject gameOverUI;
    public static bool gameEnded;
    public GameObject helpPanel;
    public Toggle dontShowAgainToggle;
    public Text implementScoreText;
    public int mult;
    private bool X2bool;
    public float X2secs;
    private bool SLOWbool;
    public float SLOWsecs;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    public Text PowerUpText;
    private bool GamePaused;

    private static GameManagerOffline _instance;
    public static GameManagerOffline Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetFloat("HelpMenu") != 1f)
        {
            helpPanel.SetActive(true);
            this.PauseGame();
        }
        gameEnded = false;
        GamePaused = false;
        X2bool = false;
        currentPollution = 0;
        pollBar.SetMaxPollution(maxPollution);

    }



    void Update()
    {
        if (gameEnded)
        {
            return;
        }
        if (currentPollution == maxPollution)
        {
            EndGame();
        }
        if (X2bool)
        {
            PowerUpText.enabled = true;
            PowerUpText.color = new Color(255, 139, 0, 255);
            PowerUpText.text = "X2 ACTIVE";
            X2secs -= Time.smoothDeltaTime;
            if (X2secs >= 0)
            {
                mult = 2;
            }
            else
            {
                PowerUpText.text = "";
                mult = 1;
                X2bool = false;
            }
        }
        if (SLOWbool && !GamePaused)
        {
            PowerUpText.enabled = true;
            PowerUpText.color = new Color(244, 0, 160, 255);
            PowerUpText.text = "SLOW ACTIVE";
            SLOWsecs -= Time.smoothDeltaTime;
            if (SLOWsecs >= 0)
            {
                Time.timeScale = 0.5f;
            }
            else
            {
                PowerUpText.text = "";
                Time.timeScale = 1f;
                SLOWbool = false;
            }
        }
    }

    
    public void DontShowAgain()
    {
        PlayerPrefs.SetFloat("HelpMenu", 1f);
    }


    void EndGame()
    {
        gameEnded = true;
        gameOverUI.SetActive(true);
        float punteggio = float.Parse(scoreText.text);

    }

    public void UpdatePollution(int i)
    {
        currentPollution += i;
        pollBar.SetPollution(currentPollution);
        if (!gameEnded)
            SoundManagerScript.PlaySound("error");
    }

    public void UpdateScore(int i)
    {
        int punteggio = int.Parse(scoreText.text);
        if (i < 0)
            punteggio += i;
        else
            punteggio += mult * i;
        scoreText.text = punteggio.ToString();
        if (!gameEnded)
        {
            if (i > 0)
            {
                StartCoroutine(ShowMessage("+" + mult * i, 0.5f));
            }
            else
            {
                StartCoroutine(ShowMessage("" + mult * i, 0.5f));
                SoundManagerScript.PlaySound("error");
            }
        }

    }

    public void PauseGame()
    {
        GamePaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        GamePaused = false;
        Time.timeScale = 1;
    }

    IEnumerator ShowMessage(string message, float delay)
    {
        implementScoreText.text = message;
        implementScoreText.enabled = true;
        yield return new WaitForSeconds(delay);
        implementScoreText.enabled = false;
    }

    /*-----------POWER UPS--------------*/

    public void X2(int sec)
    {


        X2secs = sec;
        X2bool = true;

    }

    public void slow(float sec)
    {
        SLOWsecs = sec;
        SLOWbool = true;

    }

    public void clean()
    {
        StartCoroutine(ShowPoweUpText("CLEAN ACTIVE", 2f));
        Time.timeScale = 0f;
        var toDestroy = GameObject.FindGameObjectsWithTag("Unsorted");
        foreach (GameObject waste in toDestroy)
        {
            Destroy(waste);
        }
        toDestroy = GameObject.FindGameObjectsWithTag("Plastic");
        foreach (GameObject waste in toDestroy)
        {
            Destroy(waste);
        }
        toDestroy = GameObject.FindGameObjectsWithTag("Metal");
        foreach (GameObject waste in toDestroy)
        {
            Destroy(waste);
        }
        toDestroy = GameObject.FindGameObjectsWithTag("Glass");
        foreach (GameObject waste in toDestroy)
        {
            Destroy(waste);
        }
        Time.timeScale = 1f;

    }

    public void xpClean()
    {
        StartCoroutine(ShowPoweUpText("XPCLEAN ACTIVE", 2f));
        Time.timeScale = 0f;
        int punt = 0;
        var toDestroy = GameObject.FindGameObjectsWithTag("Unsorted");
        foreach (GameObject waste in toDestroy)
        {
            punt += 10;
            Destroy(waste);
        }
        toDestroy = GameObject.FindGameObjectsWithTag("Plastic");
        foreach (GameObject waste in toDestroy)
        {
            punt += 10;
            Destroy(waste);
        }
        toDestroy = GameObject.FindGameObjectsWithTag("Metal");
        foreach (GameObject waste in toDestroy)
        {
            punt += 10;
            Destroy(waste);
        }
        toDestroy = GameObject.FindGameObjectsWithTag("Glass");
        foreach (GameObject waste in toDestroy)
        {
            punt += 10;
            Destroy(waste);
        }
        Time.timeScale = 1f;
        UpdateScore(punt);

    }

    IEnumerator ShowPoweUpText(string message, float delay)
    {
        PowerUpText.color = new Color(0, 0, 254, 255);
        PowerUpText.text = message;
        PowerUpText.enabled = true;
        yield return new WaitForSeconds(delay);
        PowerUpText.enabled = false;
    }


}




