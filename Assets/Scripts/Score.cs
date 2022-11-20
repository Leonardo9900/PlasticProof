using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text scoreText;
    public Text finalScore;
    // Start is called before the first frame update
    void Start()
    {
        finalScore.text = scoreText.text;
    }

}
