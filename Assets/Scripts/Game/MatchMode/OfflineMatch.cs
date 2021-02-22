using System;
using com.petrushevskiapps.Oxo;
using Photon.Pun;

public class OfflineMatch : MatchMode
{
    private readonly Action startMatch;
    
    public OfflineMatch(PhotonView photonView, Action startMatch) : base(photonView)
    {
        this.startMatch = startMatch;
    }
    
    public override void StartMatch()
    {
        startMatch.Invoke();
    }

    public override void RoundLost()
    {
        NetworkManager.Instance.RoomController.PlayersInRoom[1].IncrementScore();
        SendRoundCompletedRpc();
    }
}