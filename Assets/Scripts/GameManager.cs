using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Animator anim;
    public GameObject scroll;
    public Text scoreText;
    public int maxPollution = 10;
    public int currentPollution;
    public PollutionBar pollBar;
    public GameObject gameOverUI;
    public Leaderboard leaderboard;
    public static bool gameEnded;
    public GameObject helpPanel;
    public Toggle dontShowAgainToggle;
    public Text implementScoreText;
    public int mult;
    private bool X2bool;
    public float X2secs;
    private bool SLOWbool;
    public float SLOWsecs;
    public GameObject storeManager;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    public Button button6;
    public Text OwnedX2_5S;
    public Text OwnedX2_10S;
    public Text OwnedSLOW_5S;
    public Text OwnedSLOW_10S;
    public Text OwnedCLEAN;
    public Text OwnedXPCLEAN;
    private Dictionary<string, int> PowerUps;
    private bool GamePaused;
    private bool secondtry; // variabile booleana per la seconda chance
    public Text coinText;
    public GameObject coin;
    public GameObject share;
    private bool powerupActive;
    public Text powerText;
    public Image image1;
    public Image image2;

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
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
        //anim = scroll.GetComponent<Animator>();
        if (PlayerPrefs.GetFloat("HelpMenu") != 1f)
        {
            helpPanel.SetActive(true);
            this.PauseGame();
        }
        secondtry = false;
        gameEnded = false;
        GamePaused = false;
        X2bool = false;
        currentPollution = 0;
        pollBar.SetMaxPollution(maxPollution);

        button1.interactable = false;
        button2.interactable = false;
        button3.interactable = false;
        button4.interactable = false;
        button5.interactable = false;
        button6.interactable = false;

        UpdateInventory1();
 
    }

 

    void Update()
    {
        if (gameEnded)
        {
            return;
        }
        if (currentPollution == maxPollution)
        {
            if (!secondtry && PlayerPrefs.GetFloat("Money")>= 100f)
            {
                share.SetActive(false);
                gameOverUI.SetActive(true);
            }
            else
                EndGame();
        }
        if (X2bool)
        {
            
            //PowerUpText.enabled = true;;
            X2secs -= Time.smoothDeltaTime;
            if (X2secs >= 0)
            {
                mult = 2;
            }
            else
            {
                mult = 1;
                X2bool = false;
            }
        }
       /* else
        {
            scroll.SetActive(false);
        }*/
        if (SLOWbool && !GamePaused)
        {
            //PowerUpText.enabled = true;
            SLOWsecs -= Time.smoothDeltaTime;
            if (SLOWsecs >= 0)
            {
                Time.timeScale = 0.5f;
            }
            else
            {
                Time.timeScale = 1f;
                SLOWbool = false;
            }
        }
        /*else
        {
            scroll.SetActive(false); 
        }*/
    }

    public void UpdateInventory1()
    {
        GetUserInventoryRequest request = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(request, result =>
        {

           
            PowerUps = new Dictionary<string, int>();
            for (int i = 0; i < result.Inventory.Count; i++)
            {
                if (!(PowerUps.ContainsKey(result.Inventory[i].DisplayName)))
                {
                    PowerUps.Add(result.Inventory[i].DisplayName, 0);
                }
                PowerUps[result.Inventory[i].DisplayName]++;
            }
            foreach (KeyValuePair<string, int> coppia in PowerUps)
            {
                if (coppia.Key.Equals("X2/5S"))
                    button1.interactable = true;
                if (coppia.Key.Equals("X2/10S"))
                    button2.interactable = true;
                if (coppia.Key.Equals("SLOW/5S"))
                    button3.interactable = true;
                if (coppia.Key.Equals("SLOW/10S"))
                    button4.interactable = true;
                if (coppia.Key.Equals("CLEAN"))
                    button5.interactable = true;
                if (coppia.Key.Equals("XPCLEAN"))
                    button6.interactable = true;

                try
                {
                    GameObject.Find("Owned" + coppia.Key).GetComponent<Text>().text = coppia.Value.ToString();
                }
                catch (Exception e)
                {

                }
            }
               

        }, OnPlayFabError);

    }


    public void DontShowAgain()
    {
        PlayerPrefs.SetFloat("HelpMenu", 1f);
    }


    public void EndGame()
    {
        try
        {
            GameObject.Find("ResumeButton").SetActive(false);
            GameObject.Find("ContinueText").SetActive(false);
            GameObject.Find("Image1").SetActive(false);
        }
        catch (Exception e)
        {

        }
        share.SetActive(true);
        float punteggio = float.Parse(scoreText.text);
        coin.SetActive(true);
        SoundManagerScript.PlaySound("coin");
        coinText.text = ((int)Mathf.Ceil(punteggio / 10f)).ToString();
        coinText.gameObject.SetActive(true);
        gameEnded = true;
        gameOverUI.SetActive(true);
        leaderboard.SendLeaderboard(int.Parse(scoreText.text));
        AddCurrency((int)Mathf.Ceil(punteggio / 10f));

    }

    public void UpdatePollution(int i)
    {
        currentPollution += i;
        pollBar.SetPollution(currentPollution);
        if (!gameOverUI.activeInHierarchy)
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

    public void OnPlayFabError(PlayFabError obj)
    {
        UnityEngine.Debug.Log("There is a problem ");
    }

    private void AddCurrency(int money)
    {
        AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest();
        request.VirtualCurrency = "MN"; //put your virtual currency code here
        request.Amount = money; //put the amount in here
        PlayFabClientAPI.AddUserVirtualCurrency(request, result => { UnityEngine.Debug.Log("Monete aggiunte"); }, error => { UnityEngine.Debug.Log("" + error.Error); });
    }

    /*-----------POWER UPS--------------*/

    public void X2(int sec)
    {
        //scroll.SetActive(true);
        StartCoroutine(ShowPoweUpText("X2 ACTIVE",5f,new Color(255,139,0,255)));
        SoundManagerScript.PlaySound("powerup");

        int count = PowerUps["X2/" + sec + "S"];
        UnityEngine.Debug.Log("COUNT:" + count.ToString());
       
        X2secs = sec;
        X2bool = true;

        if (count == 1)
        {
            if (sec == 5)
            {
                button1.interactable = false;
               OwnedX2_5S.text = "0";
            }

            if (sec == 10)
            {
                button2.interactable = false;
                OwnedX2_10S.text = "0";
            }
                
        }
        storeManager.GetComponent<StoreManager>().RemovePowerUp("X2/" + sec + "S");
        UpdateInventory1();

    }

    public void slow(float sec)
    {
        //scroll.SetActive(true);
        StartCoroutine(ShowPoweUpText("SLOW ACTIVE", 5f, new Color(244, 0,160, 255)));
        powerupActive = true;
        SoundManagerScript.PlaySound("powerup");
        UnityEngine.Debug.Log("" + (int)(sec * 2f));
        int count = PowerUps["SLOW/" + (int)(sec*2f) + "S"];

        SLOWsecs = sec;
        SLOWbool = true;

        if (count == 1)
        {
            if (sec*2f == 5)
            {
                button3.interactable = false;
                OwnedSLOW_5S.text = "0";
            }
               
            if (sec*2f == 10)
            {
                button4.interactable = false;
                OwnedSLOW_10S.text = "0";
            }
                
        }
        storeManager.GetComponent<StoreManager>().RemovePowerUp("SLOW/" + (int)(sec*2f) + "S");
        UpdateInventory1();
    }

    public void clean()
    {
        //scroll.SetActive(true);
        StartCoroutine(ShowPoweUpText("CLEAN", 5f, new Color(0, 176, 254, 204)));
        SoundManagerScript.PlaySound("powerup");
       // StartCoroutine(ShowPoweUpText("CLEAN ACTIVE", 2f));
        int count = PowerUps["CLEAN"];
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

        if (count == 1)
        {
            button5.interactable = false;
            OwnedCLEAN.text = "0";
        }

        storeManager.GetComponent<StoreManager>().RemovePowerUp("CLEAN");
        UpdateInventory1();
    }

    public void xpClean()
    {
        //scroll.SetActive(true);
        StartCoroutine(ShowPoweUpText("XP CLEAN",5f, new Color(0, 176, 254, 204)));
        SoundManagerScript.PlaySound("powerup");
        //StartCoroutine(ShowPoweUpText("XPCLEAN ACTIVE", 2f));
        int count = PowerUps["XPCLEAN"];
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

        if (count == 1)
        {
            button6.interactable = false;
            OwnedXPCLEAN.text = "0";
        }

        storeManager.GetComponent<StoreManager>().RemovePowerUp("XPCLEAN");
        UpdateInventory1();
    }

    IEnumerator ShowPoweUpText(string message, float delay,Color color)
    {
        powerText.text = message;
        scroll.SetActive(true);
        image2.color = color;
      yield return new WaitForSeconds(delay);
        scroll.SetActive(false);

    }


    public void ContinueGame()
    {

        secondtry = true;
        SubtractMoney();
        currentPollution = 50;
        pollBar.SetPollution(currentPollution);
        gameOverUI.SetActive(false);
        ResumeGame();
        cleanAfterResume();

    }

    public void SubtractMoney()
    {
        var request = new SubtractUserVirtualCurrencyRequest
        {
            Amount = 50,

            VirtualCurrency = "MN"

        };
        PlayFabClientAPI.SubtractUserVirtualCurrency(request, result =>
        {
            UnityEngine.Debug.Log("100 monete sottratte");
        }, error =>
        {
            UnityEngine.Debug.Log("ERROR!100 monete non sottratte");
        });
    }


    public void cleanAfterResume()
    {
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

    public void Deactivate()
    {
        scroll.SetActive(false);
    }

}




