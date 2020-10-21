using System;
using System.Collections.Generic;
using System.Linq;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using Photon.Pun;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    private int turnCounter = 0;
    
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
        int turn = NetworkManager.Instance.RoomController.GetRoomProperty(Keys.ROOM_TURN);
        NetworkManager.Instance.RoomController.SetRoomProperty(Keys.ROOM_TURN, ++turn);
        Debug.Log("TurnController: Count: " + (turn));
    }


    private void DeactivateAllPlayers(bool isWin)
    {
        // No players turn
        NetworkManager.Instance.RoomController.SetRoomProperty(Keys.ROOM_TURN, 1);
    }

}
