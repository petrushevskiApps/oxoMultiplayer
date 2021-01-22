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
    public static readonly UnityEvent         MatchStart       = new UnityEvent();
    public static readonly UnityEvent         MatchStartSynced = new UnityEvent();
    public static readonly UnityBoolEvent     MatchEnd         = new UnityBoolEvent();
    
    public static readonly UnityIntegerEvent  RoundStarting = new UnityIntegerEvent();
    public static readonly UnityEvent         RoundStarted  = new UnityEvent();
    public static readonly UnityBoolEvent     RoundEnded    = new UnityBoolEvent();
    public static readonly UnityEvent         RoundCompletedEvent      = new UnityEvent();
    
    public static readonly UnityIntegerEvent  TurnChanged = new UnityIntegerEvent();
    
    public BoardController Board { get; private set; }
    
    public static MatchController LocalInstance;

    public WinCondition<ITile> WinCondition { get; private set; }

    private MatchMode matchMode;
    
    private int round;
    private int turn;
    
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
        private set
        {
            if(turn == value) return;
            turn = value;
            TurnChanged.Invoke(turn);
        }
    }
  
    private void Awake()
    {
        LocalInstance = this;
        NetworkManager.MasterSwitched.AddListener(OnMasterSwitched);
        
        SetupMatch();
        StartMatch();
    }

    private void OnDestroy()
    {
        NetworkManager.MasterSwitched.RemoveListener(OnMasterSwitched);
    }
    
    private void SetupMatch()
    {
        int rows = NetworkManager.Instance.RoomController.Properties.GetProperty<int>("r");
        
        SetupMode();
        SetupWinCondition(rows);
        CreateBoard();
    }
    
    private void SetupMode()
    {
        if (NetworkManager.Instance.ConnectionController.PlayOffline)
        {
            matchMode = new OfflineMatch(photonView, StartMatchSynced);
        }
        else matchMode = new OnlineMatch(photonView);
    }
    
    private void SetupWinCondition(int winStrike)
    {
        WinCondition = new WinCondition<ITile>(winStrike);
        WinCondition.SetupExtractStrategies();
    }
    
    private void CreateBoard()
    {
        if (Board != null || !PhotonNetwork.IsMasterClient) return;
        Board = PhotonNetwork.InstantiateRoomObject("Board", Vector3.zero, Quaternion.identity).GetComponent<BoardController>();
    }
    
    private void StartMatch()
    {
        if(!NetworkManager.IsMasterClient) return;
        MatchStart.Invoke();
        matchMode.StartMatch();
    }
    
    [PunRPC]
    private void StartMatchSynced()
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

    public void EndTurn(int playerId, int tileId, ITile[,] table)
    {
        if (WinCondition.IsRoundWon(playerId, tileId, table))
        {
            EndRound(playerId);
        }
        else if (WinCondition.IsRoundTie(table))
        {
            matchMode.RoundTie();
        }
        else
        {
            Turn++;
        }
    }
    
    private void EndRound(int winnerId)
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        bool isRoundWon = NetworkManager.Instance.RoomController.LocalPlayer.GetId() == winnerId;
        
        RoundEnded.Invoke(isRoundWon);
        
        Timer.Start(this, TimerKeys.ROUND_ENDED, 0.5f, ()=>
        {
            PhotonNetwork.IsMessageQueueRunning = true;
            if(isRoundWon) matchMode.RoundWon();
            else matchMode.RoundLost();
        });
    }
    
    [PunRPC]
    private void RoundCompleted()
    {
        NetworkManager.Instance.RoomController.LocalRpcBufferCount++;
        RoundCompletedEvent.Invoke();
        
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
        int max = NetworkManager.Instance.RoomController.PlayersInRoom.Max(player => player.GetScore());
        int sum = NetworkManager.Instance.RoomController.PlayersInRoom.Sum(player => player.GetScore());
        
        float winningPossibility = sum / (float) WinCondition.WinStrike; // Winning probability > 0.5
        float scoreDistribution = max / (float)sum; // Distribution > 0.5 
        
        return winningPossibility > 0.5f && scoreDistribution >= 0.6f;
    }
   
    private bool IsLocalMatchWin()
    {
        IPlayer matchWinner = NetworkManager.Instance.RoomController.PlayersInRoom.OrderByDescending(x => x.GetScore()).FirstOrDefault();
        return matchWinner == NetworkManager.Instance.RoomController.LocalPlayer;
    }
    
    public void EndMatch(bool isLocalWin)
    {
        Round = 0;
        MatchEnd.Invoke(isLocalWin);
        NetworkManager.Instance.RoomController.ClearRpcs(photonView);
        UIManager.Instance.OpenScreen<UIEndScreen>();
    }

    private void OnMasterSwitched()
    {
        if (Board == null)
        {
            Board = FindObjectOfType<BoardController>().GetComponent<BoardController>();
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }

}