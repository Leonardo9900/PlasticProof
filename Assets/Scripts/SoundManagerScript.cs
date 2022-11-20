using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManagerScript : MonoBehaviour
{

    public GameObject SoundManager;
    public Toggle audioToggle;
    private static SoundManagerScript _instance;
    public static AudioClip errorSound; //!
    public static AudioClip powerupSound;
    public static AudioClip coinSound;
    static AudioSource audioSrc; //!



    void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.SoundManager);
        else
        {
            DontDestroyOnLoad(SoundManager);
            _instance = this;
        }
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "error":
                audioSrc.PlayOneShot(errorSound);
                break;

            case "powerup":
                audioSrc.PlayOneShot(powerupSound);
                break;
            case "coin":
                audioSrc.PlayOneShot(coinSound);
                break;
        }
    }


    void Start() { 

        errorSound = Resources.Load<AudioClip>("error");
        powerupSound = Resources.Load<AudioClip>("powerup");
        coinSound = Resources.Load<AudioClip>("coin");
        audioSrc = GetComponent<AudioSource>();

        if (AudioListener.volume == 0)
            audioToggle.isOn = false;
        if (PlayerPrefs.GetFloat("mute") == 0)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }


}
