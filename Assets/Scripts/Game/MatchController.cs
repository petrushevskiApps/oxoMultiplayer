using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using PetrushevskiApps.UIManager;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class MatchController : MonoBehaviourPunCallbacks, IPunObservable
{
    public static UnityEvent MatchStart = new UnityEvent();
    public static UnityEvent MatchStartSynced = new UnityEvent();
    public static UnityBoolEvent MatchEnd = new UnityBoolEvent();
    public static UnityIntegerEvent RoundStarting = new UnityIntegerEvent();
    public static UnityEvent RoundStarted = new UnityEvent();
    public static UnityEvent RoundEnd = new UnityEvent();
    public static UnityIntegerEvent TurnChanged = new UnityIntegerEvent();
    
    private static GameObject board;
    public static MatchController LocalInstance;

    private const int MatchRounds = 3;

    private int round;

    public int Round
    {
        get => round;
        private set
        {
            round = value;
            Debug.Log($"Round incremented:: {round}");
        }
    }

    private int turn;
    public int Turn
    {
        get => turn;
        private set
        {
            turn = value;
            TurnChanged.Invoke(turn);
        }
    }
    
    private void Awake()
    {
        LocalInstance = this;
    }

    
    public void StartMatch()
    {
        CreateBoard();
        MatchStart.Invoke();
        NetworkManager.Instance.SendRpc(photonView, RPCs.RPC_START_MATCH, false);
    }
    
    [PunRPC]
    public void StartMatchSynced()
    {
        RoomController.Instance.LocalRpcBufferCount++;
        UIManager.Instance.OpenScreen<UIGameScreen>();
        StartRound();
        MatchStartSynced.Invoke();
    }
    
    private void StartRound()
    {
        Round++;
        Turn = (Round - 1) % 2;
        RoundStarting.Invoke(Round);
        Timer.Start(this, TimerKeys.ROUND_START_DELAY, 1.5f, () =>
        {
            RoundStarted.Invoke();
        });
    }
    
    public void EndRound(bool isRoundWon)
    {
        if(isRoundWon) UpdateScore();
        
        RoundEnd.Invoke();

        StartCoroutine(WaitRoundComplete());
    }

    IEnumerator WaitRoundComplete()
    {
        yield return new WaitWhile(() => RoomController.Instance.PlayersInRoom.Sum(player => player.Score) < Round);

        if (IsMatchCompleted())
        {
            EndMatch(IsLocalMatchWin());
        }
        else
        {
            StartRound();
        }
        
    }

    private bool IsMatchCompleted()
    {
        int max = RoomController.Instance.PlayersInRoom.Max(player => player.Score);
        int sum = RoomController.Instance.PlayersInRoom.Sum(player => player.Score);
        float winningPossibility = sum / (float) MatchRounds; // Winning possibilit > 0.5
        float scoreDistribution = max / (float)sum; // Distribution > 0.5 
        
        return (Round == MatchRounds) || (winningPossibility > 0.5f && scoreDistribution > 0.5f);
    }
    
    private void UpdateScore()
    {
        RoomController.Instance.LocalPlayer.Score++;
    }

    private bool IsLocalMatchWin()
    {
        NetworkPlayer matchWinner = RoomController.Instance.PlayersInRoom.OrderByDescending(x => x.Score).FirstOrDefault();
        return matchWinner == RoomController.Instance.LocalPlayer;
    }
    
    public void EndMatch(bool isLocalWin)
    {
        Round = 0;
        MatchEnd.Invoke(isLocalWin);
        UIManager.Instance.OpenScreen<UIEndScreen>();
        NetworkManager.Instance.ClearRpcs(photonView);
    }
    
    private void CreateBoard()
    {
        if (board == null && PhotonNetwork.IsMasterClient)
        {
            board = PhotonNetwork.InstantiateRoomObject("Board", Vector3.zero, Quaternion.identity);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

    public void IncrementTurn()
    {
        Turn++;
    }
}
