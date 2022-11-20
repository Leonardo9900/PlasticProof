using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deployMasks : MonoBehaviour
{
    public float y = 3f;
    private Vector2 screenBounds;
    public GameObject faceMaskPrefab;
    public GameObject plasticBottlePrefab;
    public GameObject glassBottlePrefab;
    public GameObject metalCanPrefab;
    public float respawnTime = 2.3f;


    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        StartCoroutine(masks());
    }

    private void spawnMask()
    {
        List<GameObject> wastes = new List<GameObject> { faceMaskPrefab, plasticBottlePrefab, glassBottlePrefab, metalCanPrefab };
        GameObject toInstantiate = wastes[Random.Range(0, wastes.Count)];
        GameObject a = Instantiate(toInstantiate) as GameObject;
        a.transform.position = new Vector2(Random.Range(-screenBounds.x + 2.7f, screenBounds.x - 2.7f), y);
    }

    IEnumerator masks()
    {
        while (!GameManager.gameEnded)
        {
            if (respawnTime >= 0.8)
            {
                respawnTime = respawnTime - 0.008f;

            }

            yield return new WaitForSeconds(respawnTime);
            spawnMask();
        }
    }
}
