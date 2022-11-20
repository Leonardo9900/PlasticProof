using UnityEngine;
using System;
using System.Collections;

public class FloatBehavior : MonoBehaviour
{
    float originalY;

    public float floatStrength = 1;// You can change this in the Unity Editor to 
    public int direction = 1;
    public float speed = 1f;
    public float delay = 10f;
    private float initialTime;

    void Start()
    {
        this.originalY = this.transform.position.y;
        initialTime = Time.time;
    }

    void Update()
    {
        if (Time.time >= delay + initialTime)
        {
            if (direction > 0)
            {
                transform.position = new Vector3(transform.position.x,
                originalY + ((float)Math.Sin(Time.time * speed) * floatStrength),
                transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x,
                originalY - ((float)Math.Sin(Time.time * speed) * floatStrength),
                transform.position.z);
            }
        }
    }
}