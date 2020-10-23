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
        CreateBoard();
        UIManager.Instance.OpenScreen<UIGameScreen>();
        MatchStarted.Invoke();
        StartRound();
        SendRpc("StartMatch");
    }

    public void StartRound()
    {
        Round++;
        RoundStarted.Invoke(Round);
    }

    public void EndRound()
    {
        UpdateScore();
        
        StartCoroutine(Delay(() => RoundEnded.Invoke()));

        StartCoroutine(WaitRoundComplete());
    }

    IEnumerator WaitRoundComplete()
    {
        yield return new WaitWhile(() => NetworkManager.Instance.RoomController.PlayersInRoom.Sum(player => player.Score) != Round);
        if (Round == MatchRounds)
        {
            EndMatch();
        }
        else StartRound();
    }
    private void UpdateScore()
    {
        NetworkPlayer winner = NetworkManager.Instance.RoomController.LocalPlayer;
        
        if (winner.IsActive())
        {
            winner.Score++;
        }
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
        DestroyBoard();
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
