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
        int turn = (round - 1) % 2;
        RoomController.Instance.Properties.Set(Keys.ROOM_TURN, turn).Update();
    }
    
    public void IncrementTurn()
    {
        int turn = RoomController.Instance.Properties.GetProperty<int>(Keys.ROOM_TURN);
        RoomController.Instance.Properties.Set(Keys.ROOM_TURN, ++turn).Update();
    }

}
