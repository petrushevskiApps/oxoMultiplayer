using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
    private void Awake()
    {
        MatchController.MatchStart.AddListener(OnMatchStarted);
        MatchController.MatchEnd.AddListener(OnMatchEnded);
    }

    private void OnDestroy()
    {
        MatchController.MatchStart.RemoveListener(OnMatchStarted);
        MatchController.MatchEnd.RemoveListener(OnMatchEnded);
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
