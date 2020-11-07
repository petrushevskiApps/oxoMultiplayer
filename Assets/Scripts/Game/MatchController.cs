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
    public static UnityEvent MatchStarted = new UnityEvent();
    public static UnityBoolEvent MatchEnded = new UnityBoolEvent();
    public static UnityIntegerEvent RoundStarted = new UnityIntegerEvent();
    public static UnityEvent RoundEnded = new UnityEvent();

    private static GameObject board;
    public static MatchController LocalInstance;

    private const int MatchRounds = 3;

    public int Round { get; private set; }
    
    private void Awake()
    {
        LocalInstance = this;
        RoomController.PlayerExitedRoom.AddListener(OnPlayerExited);
    }

    private void OnDestroy()
    {
        RoomController.PlayerExitedRoom.RemoveListener(OnPlayerExited);
    }

    private void OnPlayerExited(NetworkPlayer player)
    {
        UIManager.Instance.OpenPopup<UITimerPopup>().InitializePopup(player.Nickname);
    }

    [PunRPC]
    public void StartMatch()
    {
        CreateBoard();
        UIManager.Instance.OpenScreen<UIGameScreen>();
        MatchStarted.Invoke();
        StartRound();
    }

    private void StartRound()
    {
        Round++;
        RoundStarted.Invoke(Round);
    }
    
    public void EndRound(bool isRoundWon)
    {
        if(isRoundWon) UpdateScore();

        RoundEnded.Invoke();

        StartCoroutine(WaitRoundComplete());
    }

    IEnumerator WaitRoundComplete()
    { 
        yield return new WaitWhile(() => RoomController.Instance.PlayersInRoom.Sum(player => player.Score) != Round);
        Debug.Log($"WaitRoundComplete:: END ROUND:: {Round} DELAYED");
        
        if (IsMatchCompleted())
        {
            EndMatch();
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
        Debug.Log("T1:: Match Winner:: " + matchWinner.Nickname);
        return matchWinner == RoomController.Instance.LocalPlayer;
    }
    
    private void EndMatch()
    {
        Round = 1;
        MatchEnded.Invoke(IsLocalMatchWin());
        UIManager.Instance.OpenScreen<UIEndScreen>();
    }

    public void EndMatch(bool isLocalWin)
    {
        Round = 1;
        MatchEnded.Invoke(isLocalWin);
        UIManager.Instance.OpenScreen<UIEndScreen>();
    }
    
    private void CreateBoard()
    {
        if (board == null && PhotonNetwork.IsMasterClient)
        {
            board = PhotonNetwork.InstantiateRoomObject("Board", Vector3.zero, Quaternion.identity);
        }
    }

    private void DestroyBoard()
    {
        if (board != null && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(board);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
