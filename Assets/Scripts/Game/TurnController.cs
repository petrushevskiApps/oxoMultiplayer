using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    private List<Player> players = new List<Player>();
    public int turnCounter = 0;

    private void OnEnable()
    {
        BoardController.OnMatchEnded.AddListener(DeactivateAllPlayers);
    }

    private void OnDisable()
    {
        BoardController.OnMatchEnded.RemoveListener(DeactivateAllPlayers);
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
            int currentID = x.GetPlayerId();
            int activeID = GetPlayerTurnID() + 1;
            x.IsActive = currentID == activeID;
        });
        
    }

    private void DeactivateAllPlayers(int winnerId)
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
        return players.Count > 0 ? players[GetPlayerTurnID()] : null;
    }
    public int GetPlayerTurnID()
    {
        return (turnCounter % PhotonNetwork.CurrentRoom.PlayerCount);
    }

    public void Restart()
    {
        turnCounter = 0;
        SetActivePlayer();
    }
}
