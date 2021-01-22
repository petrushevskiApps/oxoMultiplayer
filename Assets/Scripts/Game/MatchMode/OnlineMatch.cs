using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using Photon.Pun;
using UnityEngine;

public class OnlineMatch : MatchMode
{
    public OnlineMatch(PhotonView photonView) : base(photonView){}

    public override void StartMatch()
    {
        NetworkManager.Instance.RoomController.SendRpc(PhotonView, RPCs.RPC_START_MATCH, false);
    }

    public override void RoundLost()
    {
        Debug.Log("Local player lost the match! RPC Called");
    }
}