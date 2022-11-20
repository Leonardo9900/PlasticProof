using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PollutionBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxPollution(int pollution)
    {
        slider.maxValue = pollution;
        slider.value = 0;
    }
    public void SetPollution (int pollution)
    {
        slider.value = pollution;
    }

    public void update()
    {
        if (slider.value == 100)
            SceneManager.LoadScene(1);
    }
}
