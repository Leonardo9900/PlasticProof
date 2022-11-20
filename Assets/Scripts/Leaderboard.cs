using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class Leaderboard : MonoBehaviour
{
    public void SendLeaderboard(int score)
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<StatisticUpdate> {
        new StatisticUpdate { StatisticName = "TopScore", Value = score },
    }
        },
result => { UnityEngine.Debug.Log("User statistics updated"); },
error => { UnityEngine.Debug.LogError(error.GenerateErrorReport()); });
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
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
            UnityEngine.Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
    }
}
