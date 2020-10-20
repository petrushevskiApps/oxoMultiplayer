using System;
using PetrushevskiApps.UIManager;
using Photon.Pun;
using UnityEngine;

public class MatchController : MonoBehaviourPunCallbacks, IPunObservable
{
    private static GameObject board;
    public static MatchController LocalInstance;

    private void Awake()
    {
        LocalInstance = this;
    }

    [PunRPC]
    public void StartMatch()
    {
        UIManager.Instance.OpenScreen<UIGameScreen>();
        CreateBoard();

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("StartMatch", RpcTarget.Others);
        }
    }

    public void EndMatch()
    {
        UIManager.Instance.OpenScreen<UIRoomScreen>();
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
