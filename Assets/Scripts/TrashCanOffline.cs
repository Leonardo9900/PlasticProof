
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashCanOffline : MonoBehaviour
{
    public GameManagerOffline gameManagerOffline;
    public string color;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (color)
        {
            case "blue":
                if (collision.tag == "Plastic")
                {
                    gameManagerOffline.UpdateScore(10);
                }
                else
                {
                    int deltascore;
                    int currentscore = int.Parse(gameManagerOffline.scoreText.text);
                    if (collision.tag == "Fish")
                    {
                        deltascore = -10;
                    }
                    else
                    {
                        deltascore = -5;
                    }
                    if ((currentscore + deltascore) >= 0)
                        gameManagerOffline.UpdateScore(deltascore);
                }
                break;
            case "white":
                if (collision.tag == "Unsorted")
                {
                    gameManagerOffline.UpdateScore(10);
                }
                else
                {
                    int deltascore;
                    int currentscore = int.Parse(gameManagerOffline.scoreText.text);
                    if (collision.tag == "Fish")
                    {
                        deltascore = -10;
                    }
                    else
                    {
                        deltascore = -5;
                    }
                    if ((currentscore + deltascore) >= 0)
                        gameManagerOffline.UpdateScore(deltascore);
                }
                break;
            case "green":
                if (collision.tag == "Glass")
                {
                    gameManagerOffline.UpdateScore(10);
                }
                else
                {
                    int deltascore;
                    int currentscore = int.Parse(gameManagerOffline.scoreText.text);
                    if (collision.tag == "Fish")
                    {
                        deltascore = -10;
                    }
                    else
                    {
                        deltascore = -5;
                    }
                    if ((currentscore + deltascore) >= 0)
                        gameManagerOffline.UpdateScore(deltascore);
                }
                break;
            case "yellow":
                if (collision.tag == "Metal")
                {
                    gameManagerOffline.UpdateScore(10);
                }
                else
                {
                    int deltascore;
                    int currentscore = int.Parse(gameManagerOffline.scoreText.text);
                    if (collision.tag == "Fish")
                    {
                        deltascore = -10;
                    }
                    else
                    {
                        deltascore = -5;
                    }
                    if ((currentscore + deltascore) >= 0)
                        gameManagerOffline.UpdateScore(deltascore);
                }
                break;

        }
    }
}
