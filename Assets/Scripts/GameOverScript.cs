using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class GameOverScript : MonoBehaviour
{
    public Text finalScore;
    public Text scoreText;
    public Button retryButton;
    public Text maxScore;
    public Leaderboard leaderboard;
    public GameObject HighScore;

    private void Awake()
    {
        if (!FB.IsInitialized)
        {
            FB.Init(() =>
            {
                if (FB.IsInitialized)
                    FB.ActivateApp();
                else
                    Debug.LogError("Couldn't initialize");
            },
            isGameShown =>
            {
                if (!isGameShown)
                    Time.timeScale = 0;
                else
                    Time.timeScale = 1;
            });
        }
        else
            FB.ActivateApp();
    }

    void Start()
    {
        maxScore.text = HighScore.GetComponent<HighScoreScript>().HighScoreText;
        if (maxScore.text.Equals("2"))
            maxScore.text = "0";
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        finalScore.text = scoreText.text;
        try
        {
            if (int.Parse(maxScore.text) < int.Parse(scoreText.text)) // abbiamo fatto il nuovo high score 
            {
                maxScore.text = scoreText.text;

            }
        }

        catch (FormatException e)
        {

        }

    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /*
    public void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => UnityEngine.Debug.LogError(error.GenerateErrorReport())
        );
    }

    public void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        
        int max = -1;
        foreach (var eachStat in result.Statistics)
        {
           
            if (eachStat.Value > max)
            {
                max = eachStat.Value;
                
            }
            maxScore.text = max.ToString();
        }
    }
    */
    /*public void FacebookShare1()
    {
        FB.ShareLink(
            new Uri("https://www.facebook.com/PlasticProof-107319014533106"),
            "PlasticProof",
            "My score was " + scoreText.text + " and my high score is "+ maxScore.text ,
            new Uri("https://scontent.ffco2-1.fna.fbcdn.net/v/t1.0-9/131224248_107319624533045_2363250741501142588_o.png?_nc_cat=109&ccb=2&_nc_sid=09cbfe&_nc_ohc=YtpYQpxvLkwAX_YNFV1&_nc_ht=scontent.ffco2-1.fna&oh=0ab433b1dfdef268c4516efa82f88465&oe=5FF88148"),
            callback: ShareCallback);

    }*/


    public void FacebookShare1()
    {
        FB.FeedShare(
            link: new Uri("https://www.facebook.com/plasticproof3"),
            linkName: "My score was:" +scoreText.text,
            mediaSource: "https://scontent.ffco2-1.fna.fbcdn.net/v/t1.0-9/131224248_107319624533045_2363250741501142588_o.png?_nc_cat=109&ccb=2&_nc_sid=09cbfe&_nc_ohc=YtpYQpxvLkwAX_YNFV1&_nc_ht=scontent.ffco2-1.fna&oh=0ab433b1dfdef268c4516efa82f88465&oe=5FF88148",
            callback: ShareCallback);

    }




    private void ShareCallback(IShareResult result)
    {
        if (result.Cancelled || !string.IsNullOrEmpty(result.Error))
            UnityEngine.Debug.Log( "Share feature error: " + result.Error);

        else if (!string.IsNullOrEmpty(result.PostId))
            UnityEngine.Debug.Log(result.PostId);

        else
            UnityEngine.Debug.Log("success");
    }

}
