using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashCan : MonoBehaviour
{
    public GameManager gameManager;
    public string color;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (color)
        {
            case "blue":
                if (collision.tag == "Plastic")
                {
                    gameManager.UpdateScore(10);
                }
                else
                {
                    int deltascore;
                    int currentscore = int.Parse(gameManager.scoreText.text);
                    if (collision.tag == "Fish")
                    {
                        deltascore = -10;
                    }
                    else
                    {
                        deltascore = -5;
                    }
                    if ((currentscore +deltascore) >= 0)
                         gameManager.UpdateScore(deltascore);
                }
                break;
            case "white":
                if (collision.tag == "Unsorted")
                {
                    gameManager.UpdateScore(10);
                }
                else
                {
                    int deltascore;
                    int currentscore = int.Parse(gameManager.scoreText.text);
                    if (collision.tag == "Fish")
                    {
                        deltascore = -10;
                    }
                    else
                    {
                        deltascore = -5;
                    }
                    if ((currentscore + deltascore) >= 0)
                        gameManager.UpdateScore(deltascore);
                }
                break;
            case "green":
                if (collision.tag == "Glass")
                {
                    gameManager.UpdateScore(10);
                }
                else
                {
                    int deltascore;
                    int currentscore = int.Parse(gameManager.scoreText.text);
                    if (collision.tag == "Fish")
                    {
                        deltascore = -10;
                    }
                    else
                    {
                        deltascore = -5;
                    }
                    if ((currentscore + deltascore) >= 0)
                        gameManager.UpdateScore(deltascore);
                }
                break;
            case "yellow":
                if (collision.tag == "Metal")
                {
                    gameManager.UpdateScore(10);
                }
                else
                {
                    int deltascore;
                    int currentscore = int.Parse(gameManager.scoreText.text);
                    if (collision.tag == "Fish")
                    {
                        deltascore = -10;
                    }
                    else
                    {
                        deltascore = -5;
                    }
                    if ((currentscore + deltascore) >= 0)
                        gameManager.UpdateScore(deltascore);
                }
                break;

        }
    }
}
