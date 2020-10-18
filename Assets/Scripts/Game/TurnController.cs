using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    private int turnCounter = 0;
    private List<Player> players = new List<Player>();
    
    private void OnEnable()
    {
        BoardController.MatchEnded.AddListener(DeactivateAllPlayers);
    }

    private void OnDisable()
    {
        BoardController.MatchEnded.RemoveListener(DeactivateAllPlayers);
    }

    public void IncrementTurn()
    {
        turnCounter++;
        SetActivePlayer();
    }

    private void SetActivePlayer()
    {
        players.ForEach(x =>
        {
            int currentId = x.PlayerID;
            int activeId = GetPlayerTurnId() + 1;
            x.IsActive = currentId == activeId;
        });
        
    }

    private void DeactivateAllPlayers(bool isWin)
    {
        players.ForEach(x => x.IsActive = false);
    }
    public void AddPlayer(Player player)
    {
        players.Add(player);
        players.Sort();
        players.Reverse();
        SetActivePlayer();
    }
    
    public Player GetActivePlayer()
    {
        return players.Count > 0 ? players[GetPlayerTurnId()] : null;
    }
    
    private int GetPlayerTurnId() => turnCounter % PhotonNetwork.CurrentRoom.PlayerCount;
}
