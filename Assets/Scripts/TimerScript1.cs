using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript1 : MonoBehaviour
{
    public GameObject share;
    Image timerBar;
    public float maxtime = 10f;
    float timeLeft;
    float currCountdownValue;


    void Start()
    {
        
        timerBar = GetComponent<Image>();
        StartCoroutine(StartCountdown(maxtime));
        //timeLeft = maxtime;
    }

    void Update()
    {
      /*  if(timeLeft > 0)
        {
            timeLeft -= TimerScript1.timer.deltaTime;
            timerBar.fillAmount = timeLeft / maxtime;
        }
        else
        {
            
            TimerScript1.timer.timeScale = 0;
        }*/


        
    }


    public IEnumerator StartCountdown(float countdownValue)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            timerBar.fillAmount = (float) currCountdownValue / maxtime;
            yield return new WaitForSeconds(1f);
            currCountdownValue--;
        }
        GameObject.Find("GameManager").GetComponent<GameManager>().EndGame();
        
    }

}
