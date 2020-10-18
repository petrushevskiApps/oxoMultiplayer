
using System;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using ExitGames.Client.Photon;
using UnityEngine.Events;

public class NetworkPlayer
{
    private Photon.Realtime.Player player;

    public string Nickname => player.NickName;
    public string UserId => player.UserId;
    public bool IsReady { get; private set; } = false;
    
    public UnityBoolEvent PlayerStatusChange = new UnityBoolEvent();
    
    public NetworkPlayer(Photon.Realtime.Player player)
    {
        IsReady = false;
        this.player = player;
        UpdatePlayerStatuses(player.CustomProperties);
    }
    
    public void UpdatePlayerStatuses(Hashtable statuses)
    {
        if (player.IsMasterClient)
        {
            IsReady = true;
        }
        else if (statuses.ContainsKey("playerReady"))
        {
            IsReady = (bool)statuses["playerReady"];
        }
        else
        {
            IsReady = false;
        }
        
        PlayerStatusChange.Invoke(IsReady);
    }

}
