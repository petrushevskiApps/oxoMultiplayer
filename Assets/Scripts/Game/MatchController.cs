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

    
    public int Round
    {
        get => round;
        private set
        {
            if(round == value) return;
            round = value;
            Debug.Log($"Round incremented:: {round}");
        }
    }

    public int Turn
    {
        get => turn;
        set
        {
            if(turn == value) return;
            turn = value;
            TurnChanged.Invoke(turn);
        }
    }
    
    private int round;
    private int turn;
    
    private void Awake()
    {
        LocalInstance = this;
        NetworkManager.MasterSwitched.AddListener(OnMasterSwitched);
        
        if(!NetworkManager.IsMasterClient) return;
        StartMatch();
    }

    private void OnDestroy()
    {
        NetworkManager.MasterSwitched.RemoveListener(OnMasterSwitched);
    }

    private void OnMasterSwitched()
    {
        if (board == null)
        {
            board = FindObjectOfType<BoardController>().gameObject;
        }
    }


    private void StartMatch()
    {
        CreateBoard();
        MatchStart.Invoke();
        NetworkManager.Instance.SendRpc(photonView, RPCs.RPC_START_MATCH, false);
    }
    
    [PunRPC]
    public void StartMatchSynced()
    {
        NetworkManager.Instance.RoomController.LocalRpcBufferCount++;
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

    public void RoundWon()
    {
        NetworkManager.Instance.RoomController.LocalPlayer.Score++;
            
        if (NetworkManager.Instance.RoomController.IsSynced)
        {
            NetworkManager.Instance.SendRpc(photonView, RPCs.RPC_ROUND_COMPLETED, true);
        }
    }
    public void RoundTie()
    {
        if (NetworkManager.Instance.RoomController.IsSynced)
        {
            NetworkManager.Instance.SendRpc(photonView, RPCs.RPC_ROUND_COMPLETED, false);
        }
    }
    
    [PunRPC]
    private void WaitRoundCompletion()
    {
        NetworkManager.Instance.RoomController.LocalRpcBufferCount++;
        RoundEnd.Invoke();
        
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
        int max = NetworkManager.Instance.RoomController.PlayersInRoom.Max(player => player.Score);
        int sum = NetworkManager.Instance.RoomController.PlayersInRoom.Sum(player => player.Score);
        
        float winningPossibility = sum / (float) MatchRounds; // Winning probability > 0.5
        float scoreDistribution = max / (float)sum; // Distribution > 0.5 
        
        return winningPossibility > 0.5f && scoreDistribution >= 0.6f;
    }
   

    private bool IsLocalMatchWin()
    {
        NetworkPlayer matchWinner = NetworkManager.Instance.RoomController.PlayersInRoom.OrderByDescending(x => x.Score).FirstOrDefault();
        return matchWinner == NetworkManager.Instance.RoomController.LocalPlayer;
    }
    
    public void EndMatch(bool isLocalWin)
    {
        Round = 0;
        MatchEnd.Invoke(isLocalWin);
        NetworkManager.Instance.ClearRpcs(photonView);
        UIManager.Instance.OpenScreen<UIEndScreen>();
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

}
