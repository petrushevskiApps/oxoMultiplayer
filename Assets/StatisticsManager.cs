using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    private void Awake()
    {
        MatchController.MatchStarted.AddListener(OnMatchStarted);
        MatchController.MatchEnded.AddListener(OnMatchEnded);
    }

    private void OnDestroy()
    {
        MatchController.MatchStarted.RemoveListener(OnMatchStarted);
        MatchController.MatchEnded.RemoveListener(OnMatchEnded);
    }

    private void OnMatchStarted()
    {
        PlayerDataController.IncreasePlayedGames();
    }
    private void OnMatchEnded(bool isWin)
    {
        if (isWin)
        {
            PlayerDataController.IncreaseWonGames();
        }
        else
        {
            PlayerDataController.IncreaseLostGames();
        }
    }

    
}
