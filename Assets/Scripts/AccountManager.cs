using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;

public class AccountManager : MonoBehaviour
{

    
    public InputField nome;
    public Text StatusText;

    private PlayFabAuthService _AuthService = PlayFabAuthService.Instance;
    void Start()
    {
        GetPlayerDisplayName();
        nome.enabled = false;
    }
    // Update is called once per frame
    public void LogOut()
    {
        _AuthService.UnlinkSilentAuth();
        _AuthService.ClearRememberMe();
        _AuthService.AuthType = Authtypes.None;
        PlayFabClientAPI.ForgetAllCredentials();
        PlayFabManager.IsLoggedIn = false;
        SceneManager.LoadScene(0);
    }


    public void ChangeUsername()
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(
                   new UpdateUserTitleDisplayNameRequest
                   {
                       DisplayName = nome.text
                       

                   }, UpdateResult =>
                   {
                       PlayerPrefs.SetString("TitleDisplayName", UpdateResult.DisplayName);
                       UnityEngine.Debug.Log("The player's display name is now: " + UpdateResult.DisplayName);
                       PlayerPrefs.SetFloat("NomeCambiato", 1f);
                   },error =>
                   {
                       GameObject.Find("ErrorText").GetComponent<Text>().text = "Name not available!";
                       
                       GameObject.Find("InputField").GetComponent<InputField>().text = PlayerPrefs.GetString("TitleDisplayName");
                   });
    }
    private void GetPlayerDisplayName()
    {
        var request = new GetAccountInfoRequest();
        request.PlayFabId = null;
        PlayFabClientAPI.GetAccountInfo(request, OnGetPlayerDisplayNameSuccess, error =>
        {
            StatusText.text = error.GenerateErrorReport();
        });

    }

    private void OnGetPlayerDisplayNameSuccess(GetAccountInfoResult obj)
    {
        nome.text = obj.AccountInfo.TitleInfo.DisplayName;
    }

    public void ResetErrorText()
    {
        GameObject.Find("ErrorText").GetComponent<Text>().text = "";
    }

    public void CheckTitleDisplayName()
    {
        if (PlayerPrefs.GetString("TitleDisplayName").Equals(""))
        {
            GameObject.Find("Help").GetComponent<Button>().interactable = false;
            
        }

    }

    public void VerificaNomeCorretto()
    {
        if (PlayerPrefs.GetFloat("NomeCambiato") == 1f)
        {
            GameObject.Find("Help").GetComponent<Button>().interactable = true;
        }
    }



}
