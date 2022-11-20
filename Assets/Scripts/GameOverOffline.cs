using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class GameOverOffline : MonoBehaviour
{
    public Text finalScore;
    public Text scoreText;
    public Button retryButton;


    // Start is called before the first frame update
    void OnEnable()
    {
        finalScore.text = scoreText.text;

    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
