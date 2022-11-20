using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPatrol : MonoBehaviour
{
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    Vector2 targetPosition;
    SpriteRenderer sr;

    public float speed;

    void Start()
    {
        this.sr = GetComponent<SpriteRenderer>();
        this.targetPosition = GetRandomPosition();
        if (targetPosition.x < transform.position.x)
            sr.flipX = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameEnded == true)
        {
            this.enabled = false;
            return;
        }
        if ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }else
        {
            targetPosition = GetRandomPosition();
            if (targetPosition.x < transform.position.x)
                sr.flipX = true;
            else sr.flipX = false;
        }
    }

    Vector2 GetRandomPosition()
    {
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);
        return new Vector2(randomX, randomY);
    }
}
