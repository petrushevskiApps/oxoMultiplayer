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
        MatchController.RoundStarted.AddListener(OnRoundStarted);
    }
    private void OnDisable()
    {
        MatchController.RoundStarted.RemoveListener(OnRoundStarted);
    }
    
    private void OnRoundStarted(int round)
    {
        NetworkManager.Instance.RoomController.SetRoomProperty(Keys.ROOM_TURN, (round - 1) % 2);
    }
    
    public void IncrementTurn()
    {
        int turn = NetworkManager.Instance.RoomController.GetRoomProperty(Keys.ROOM_TURN);
        NetworkManager.Instance.RoomController.SetRoomProperty(Keys.ROOM_TURN, ++turn);
        Debug.Log("T1:: TurnController: IncrementTurn: " + (turn));
    }

}
