using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = 1.6666f;

        if(screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = 20f;
        }

        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = 20f * differenceInSize;
        }
    }
}
