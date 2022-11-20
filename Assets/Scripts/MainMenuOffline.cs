using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuOffline : MonoBehaviour
{
  
    public void PlayGame()
    {
        SceneManager.LoadScene("GameSceneOffline");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MenuOffline");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void BackToSignIn()
    {
        SceneManager.LoadScene("RegisterNewScene");
    }

    public void Start()
    {
        DontDestroyOnLoad(GameObject.Find("RedFish1"));
        DontDestroyOnLoad(GameObject.Find("GreenFish"));
        DontDestroyOnLoad(GameObject.Find("PurpleFish"));
        DontDestroyOnLoad(GameObject.Find("BlueFish"));
        DontDestroyOnLoad(GameObject.Find("OrangeFish"));
        DontDestroyOnLoad(GameObject.Find("BallFish"));
    }

}
