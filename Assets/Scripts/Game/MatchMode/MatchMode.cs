using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using Photon.Pun;

public abstract class MatchMode
{
    protected PhotonView PhotonView { get; }
    
    protected MatchMode(PhotonView photonView)
    {
        PhotonView = photonView;
    }

    public abstract void StartMatch();
    public abstract void RoundLost();

    public void RoundWon()
    {
        NetworkManager.Instance.RoomController.LocalPlayer.IncrementScore();
        SendRoundCompletedRpc();
    }
    
    public void RoundTie()
    {
        SendRoundCompletedRpc();
    }
    
    protected void SendRoundCompletedRpc()
    {
        if (NetworkManager.Instance.RoomController.IsSynced)
        {
            NetworkManager.Instance.RoomController.SendRpc(PhotonView, RPCs.RPC_ROUND_COMPLETED, true);
        }
    }
}