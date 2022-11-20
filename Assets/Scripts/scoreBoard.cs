using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class scoreBoard : MonoBehaviour
{


    public string nome;

    private void Start()
    {
        GetPlayerDisplayName();
    }


    public void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest();
        request.StartPosition = 0;
        request.StatisticName = "TopScore";
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardSuccess, OnPlayFabError);
    }

    public void GetLeaderBoardAroundPlayer()
    {
        var request = new GetLeaderboardAroundPlayerRequest();
        request.StatisticName = "TopScore";
        request.MaxResultsCount = 1;
        request.PlayFabId = null;
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerSuccess, OnPlayFabError);
    }

    private void OnPlayFabError(PlayFabError obj)
    {
        UnityEngine.Debug.Log("Something went wrong");
    }

    

    private void OnLeaderboardSuccess(GetLeaderboardResult obj)
    {
        
        PrintLeaderboard(obj);
     
    }

    private void OnLeaderboardAroundPlayerSuccess(GetLeaderboardAroundPlayerResult obj)
    {
        GameObject.Find("PosPlayer").GetComponent<Text>().text = (obj.Leaderboard[0].Position + 1).ToString();
        GameObject.Find("usernamePlayer").GetComponent<Text>().text = obj.Leaderboard[0].DisplayName;
        GameObject.Find("ScorePlayer").GetComponent<Text>().text = obj.Leaderboard[0].StatValue.ToString();

      
    }

    private void GetPlayerDisplayName()
    {
        var request = new GetAccountInfoRequest();
        request.PlayFabId = null;
        PlayFabClientAPI.GetAccountInfo(request, OnGetPlayerDisplayNameSuccess, OnPlayFabError);

    }

    private void OnGetPlayerDisplayNameSuccess(GetAccountInfoResult obj)
    {
        nome =  obj.AccountInfo.TitleInfo.DisplayName;
  
     
    }

    private void PrintLeaderboard(GetLeaderboardResult obj)
    {
        bool app=false;
        
        for (int i = 0; i<obj.Leaderboard.Count; i++)
          {
            var appoggioU = GameObject.Find("username" + (i + 1)).GetComponent<Text>();
            var appoggioS = GameObject.Find("Score" + (i + 1)).GetComponent<Text>();
            var appoggioP = GameObject.Find("Pos" + (i + 1)).GetComponent<Text>();

            appoggioU.text = obj.Leaderboard[i].DisplayName;
            appoggioS.text = obj.Leaderboard[i].StatValue.ToString();
           
            if (obj.Leaderboard[i].DisplayName == nome) {
                app = true;
               // UnityEngine.Debug.Log(app);
                appoggioU.color = Color.red;
                appoggioS.color = Color.red;
                appoggioP.color =   Color.red;
            }
            
            
            

    }
        if (app == false)
        {
            GetLeaderBoardAroundPlayer();
        }


    }
    

}
