using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomControllerOnline : RoomController
{

    // Players in Room
    private List<Player> PlayersList => PhotonNetwork.CurrentRoom.Players.Values.ToList();

    protected override void OnRoomEntered()
    {
        base.OnRoomEntered();
        
        PlayersList.ForEach(CreateNetworkPlayer);
    }

    protected override void OnRoomExited()
    {
        base.OnRoomExited();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (players.ContainsKey(newPlayer.UserId)) return;
        CreateNetworkPlayer(newPlayer);
        SetRoomStatus();
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (!players.ContainsKey(otherPlayer.UserId)) return;
        
        PlayerExitedRoom.Invoke(players[otherPlayer.UserId]);
        players.Remove(otherPlayer.UserId);
        SetRoomStatus();
    }

    private void CreateNetworkPlayer(Player player)
    {
        NetworkPlayer netPlayer = new NetworkPlayer(player);
        players.Add(player.UserId, netPlayer);
        netPlayer.Init();

        if (player.IsLocal) LocalPlayer = netPlayer;
        
        PlayerEnteredRoom.Invoke(netPlayer);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (players.ContainsKey(targetPlayer.UserId))
        {
            ((NetworkPlayer)players[targetPlayer.UserId]).UpdatePlayerStatuses(changedProps);
        }
    }
    
    
}
