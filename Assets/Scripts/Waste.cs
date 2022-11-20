using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waste : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Sand":
                Destroy(gameObject);
                break;
            case "TrashCan":
                Destroy(gameObject);
                break;
        }
    }
}
