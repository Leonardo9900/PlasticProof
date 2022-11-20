using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandOffline : MonoBehaviour
{
    public GameManagerOffline gameManagerOffline;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Plastic":
            case "Unsorted":
            case "Glass":
            case "Metal":
                gameManagerOffline.UpdatePollution(10);
                break;
        }
    }
}
