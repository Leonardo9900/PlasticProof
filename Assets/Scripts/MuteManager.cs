using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteManager : MonoBehaviour
{

    private Toggle toggle;
    private bool mute;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        if (AudioListener.volume == 0)
            toggle.isOn = false;

        if (PlayerPrefs.GetFloat("mute") == 0)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }


    }

    void Update()
    {
        PlayerPrefs.GetFloat("mute");
    }

    public void ToggleAudioOnValueChanged(bool audioIn)
    {
        if (audioIn)
        {
            AudioListener.volume = 1;
            PlayerPrefs.SetFloat("mute", 1);
        }
        else
        {
            AudioListener.volume = 0;
            PlayerPrefs.SetFloat("mute", 0);
        }
    }



}
