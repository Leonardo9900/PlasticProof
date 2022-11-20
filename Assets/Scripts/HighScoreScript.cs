using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreScript : MonoBehaviour
{
    public string HighScoreText;
    // Start is called before the first frame update
    void Start()
    {
        GetStatistics();
    }

    public void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => UnityEngine.Debug.LogError(error.GenerateErrorReport())
        );
    }

    public void OnGetStatistics(GetPlayerStatisticsResult result)
    {

        int max = -1;
        foreach (var eachStat in result.Statistics)
        {

            if (eachStat.Value > max)
            {
                max = eachStat.Value;

            }
            HighScoreText = max.ToString();
        }
    }
}
