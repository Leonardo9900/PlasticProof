using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsOffline : MonoBehaviour
{

    public void OpenCredits()
    {
        StartCoroutine(Countdown1(29f));
    }

    IEnumerator Countdown1(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Find("CreditsMenu").SetActive(false);
        SceneManager.LoadScene("MenuOffline");
    }
}
