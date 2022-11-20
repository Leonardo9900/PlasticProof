using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sand : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Plastic":
            case "Unsorted":
            case "Glass":
            case "Metal":
                gameManager.UpdatePollution(10);
                break;
        }
    }
}
