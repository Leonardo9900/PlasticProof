using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;

public class MainMenu : MonoBehaviour
{
    public GameObject panel;
    public GameObject creditsMenu;
    public GameObject creditsScroller;
    public GameObject mainMenu;
    public GameObject fish1;
    public GameObject fish2;
    public GameObject fish3;
    public GameObject fish4;
    public GameObject fish5;
    public GameObject fish6;
    private bool verificata;
    public Text email;
    private MainMenu instance;
    private bool registrazione;
    public GameObject panelNomeIndisponibile;

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene1");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu1");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }


    public void Start()
    {
        DontDestroyOnLoad(GameObject.Find("RedFish1"));
        DontDestroyOnLoad(GameObject.Find("GreenFish"));
        DontDestroyOnLoad(GameObject.Find("PurpleFish"));
        DontDestroyOnLoad(GameObject.Find("BlueFish"));
        DontDestroyOnLoad(GameObject.Find("OrangeFish"));
        DontDestroyOnLoad(GameObject.Find("BallFish"));

        if (PlayerPrefs.GetFloat("Registrazione") == 1f)
        {
            registrazione = true;
            AddMoney();
            PlayerPrefs.SetFloat("Registrazione", 0f);
        }
        

        GetPlayerDisplayName();
        email.text = PlayerPrefs.GetString("Email");
        verificata = true;
        AggiornaMail();
        Verifica();
        CheckTitleDisplayName();
    }

    public void Verifica()
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = PlayerPrefs.GetString("PlayFabId"),
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowContactEmailAddresses = true
            }
        },
               result2 =>
               {
                   try
                   {
                       string app = result2.PlayerProfile.ContactEmailAddresses[0].VerificationStatus.ToString();
                       if (app == "Unverified" || app == "Pending")
                       {
                           verificata = false;
                       }
                   }
                   catch (ArgumentOutOfRangeException e)
                   {
                       verificata = false;
                   }
               },
               OnPlayFabError);


    }


    public void AggiornaMail()
    {

        var request = new AddOrUpdateContactEmailRequest
        {
            EmailAddress = PlayerPrefs.GetString("Email")
        };
        UnityEngine.Debug.Log("Email " + PlayerPrefs.GetString("Email"));
        UnityEngine.Debug.Log("Display Name : " + PlayerPrefs.GetString("TitleDisplayName"));
        PlayFabClientAPI.AddOrUpdateContactEmail(request, result1 =>
        {
            UnityEngine.Debug.Log("The player's account has been updated with a contact email");

        }, OnPlayFabError);
    }

    private void Update()
    {
        if (!verificata)
            panel.SetActive(true);

        VerificaNomeCorretto();

        if (creditsMenu.activeInHierarchy)
        {
            UnityEngine.Debug.Log(creditsScroller.transform.position.y);
            if (creditsScroller.transform.position.y == 4177)
            {
                creditsMenu.SetActive(false);
                mainMenu.SetActive(true);
                fish1.SetActive(true);
                fish2.SetActive(true);
                fish3.SetActive(true);
                fish4.SetActive(true);
                fish5.SetActive(true);
                fish6.SetActive(true);
            }
        }


    }


    private void OnPlayFabError(PlayFabError obj)
    {
        UnityEngine.Debug.Log(obj.GenerateErrorReport());
    }


    private void GetPlayerDisplayName()
    {
        var request = new GetAccountInfoRequest();
        request.PlayFabId = null;
        PlayFabClientAPI.GetAccountInfo(request, OnGetPlayerDisplayNameSuccess, error =>
        {
            UnityEngine.Debug.Log(error.GenerateErrorReport());
        });

    }

    private void OnGetPlayerDisplayNameSuccess(GetAccountInfoResult obj)
    {
        PlayerPrefs.SetString("TitleDisplayName", obj.AccountInfo.TitleInfo.DisplayName);
        UnityEngine.Debug.Log(PlayerPrefs.GetString("TitleDisplayName"));
    }

    public void AddMoney()
    {
        var request = new AddUserVirtualCurrencyRequest
        {
            Amount = 10,

            VirtualCurrency = "MN"

        };
        PlayFabClientAPI.AddUserVirtualCurrency(request, result =>
         {
             UnityEngine.Debug.Log("10 monete aggiunte");
         }, error =>
         {
             UnityEngine.Debug.Log("ERROR!10 monete non aggiunte");
         });
    }


    public void CheckTitleDisplayName()
    {
        if (PlayerPrefs.GetString("TitleDisplayName").Equals(""))
        {

            GameObject.Find("PlayButton").GetComponent<Button>().interactable=false;
            GameObject.Find("LeaderBoardButton").GetComponent<Button>().interactable = false;
            GameObject.Find("StoreButton").GetComponent<Button>().interactable = false;
            GameObject.Find("CreditsButton").GetComponent<Button>().interactable = false;
            panelNomeIndisponibile.SetActive(true);
        }

    }

    public void VerificaNomeCorretto()
    {
        if (PlayerPrefs.GetFloat("NomeCambiato") == 1f)
        {
            GameObject.Find("PlayButton").GetComponent<Button>().interactable = true;
            GameObject.Find("LeaderBoardButton").GetComponent<Button>().interactable = true;
            GameObject.Find("StoreButton").GetComponent<Button>().interactable = true;
            GameObject.Find("CreditsButton").GetComponent<Button>().interactable = true;
            PlayerPrefs.SetFloat("NomeCambiato", 0f);
        }
    }

    public void SetVariable() {
        PlayerPrefs.SetFloat("ComeBack", 1f);
    }


}
