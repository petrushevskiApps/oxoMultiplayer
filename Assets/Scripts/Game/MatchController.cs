using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using PetrushevskiApps.UIManager;
using Photon.Pun;
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
    }

    [PunRPC]
    public void StartMatch()
    {
        SendRpc("StartMatch");
        CreateBoard();
        UIManager.Instance.OpenScreen<UIGameScreen>();
        MatchStarted.Invoke();
        StartRound();
    }
 
    public void StartRound()
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
        yield return new WaitWhile(() => NetworkManager.Instance.RoomController.PlayersInRoom.Sum(player => player.Score) != Round);
        Debug.Log("WaitRoundComplete:: END ROUND:: " +  Round + " DELAYED");
        
        if (IsMatchCompleted())
        {
            EndMatch();
        }
        else StartRound();
    }

    private bool IsMatchCompleted()
    {
        int max = NetworkManager.Instance.RoomController.PlayersInRoom.Max(player => player.Score);
        int sum = NetworkManager.Instance.RoomController.PlayersInRoom.Sum(player => player.Score);
        float winningPossibility = sum / (float) MatchRounds; // Winning possibilit > 0.5
        float scoreDistribution = max / (float)sum; // Distribution > 0.5 
        
        return (Round == MatchRounds) || (winningPossibility > 0.5f && scoreDistribution > 0.5f);
    }
    private void UpdateScore()
    {
        NetworkManager.Instance.RoomController.LocalPlayer.Score++;
    }

    private bool IsLocalMatchWin()
    {
        NetworkPlayer matchWinner = NetworkManager.Instance.RoomController.PlayersInRoom.OrderByDescending(x => x.Score).FirstOrDefault();
        Debug.Log("T1:: Match Winner:: " + matchWinner.Nickname);
        return matchWinner == NetworkManager.Instance.RoomController.LocalPlayer;
    }
    
    private void EndMatch()
    {
        Round = 0;
        MatchEnded.Invoke(IsLocalMatchWin());
        UIManager.Instance.OpenScreen<UIEndScreen>();
    }
    
    private void CreateBoard()
    {
        if (BoardController.LocalInstance == null && PhotonNetwork.IsMasterClient)
        {
            board = PhotonNetwork.InstantiateRoomObject("Board", Vector3.zero, Quaternion.identity);
        }
    }

    private void DestroyBoard()
    {
        if (BoardController.LocalInstance != null && PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(board);
        }
    }

    private void SendRpc(string rpcMethodName)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(rpcMethodName, RpcTarget.Others);
        }
    }
    
    private IEnumerator Delay(Action delayedAction)
    {
        yield return new WaitForSeconds(0.5f);
        delayedAction.Invoke();
    }
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
