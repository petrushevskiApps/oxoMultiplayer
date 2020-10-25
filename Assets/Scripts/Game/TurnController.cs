using System;
using System.Collections.Generic;
using System.Linq;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using Photon.Pun;
using UnityEngine;

public class TurnController : MonoBehaviour
{
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
        RoomController.SetRoomProperty(Keys.ROOM_TURN, (round - 1) % 2);
    }
    
    public void IncrementTurn()
    {
        int turn = RoomController.GetRoomProperty(Keys.ROOM_TURN);
        RoomController.SetRoomProperty(Keys.ROOM_TURN, ++turn);
        Debug.Log("T1:: TurnController: IncrementTurn: " + (turn));
    }

}
