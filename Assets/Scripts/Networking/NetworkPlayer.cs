
using System;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

public class NetworkPlayer
{
    public UnityBoolEvent PlayerStatusChange = new UnityBoolEvent();
    
    public int PlayerId { get; }
    public string Nickname => player.NickName;
    public string UserId => player.UserId;
    public bool IsReady { get; private set; }
    public TileType PlayerSymbol { get; }
    
    
    private Player player;
    private int playerTurnId;
    private Hashtable playerProperties;
    
    public bool IsActive()
    {
        int turn = NetworkManager.Instance.RoomController.GetRoomProperty(Keys.ROOM_TURN);
        return turn % PhotonNetwork.CurrentRoom.PlayerCount == playerTurnId;
    }
    
    
    public NetworkPlayer(Player player)
    {
        IsReady = false;
        this.player = player;
        
        PlayerId = player.ActorNumber;
        PlayerSymbol = (TileType) PlayerId;
        playerTurnId = PlayerId - 1;
        
        if (player.IsLocal)
        {
            SetupPlayerProperties();
        }
        
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

    private void SetupPlayerProperties()
    {
        playerProperties = new Hashtable();
        playerProperties.Add(Keys.PLAYER_READY_KEY, false);
        playerProperties.Add(Keys.PLAYER_MATCH_ID, playerTurnId);
        player.SetCustomProperties(playerProperties);
    }
    public void ChangePlayerProperty(string KEY, int value)
    {
        playerProperties[KEY] = value;
        player.SetCustomProperties(playerProperties);
    }
    public int GetPlayerProperty(string KEY)
    {
        object result = 0;
        player.CustomProperties.TryGetValue(KEY, out result);
        return (int) result;
    }
        
    public void ChangePlayerProperty(string KEY, bool value)
    {
        playerProperties[KEY] = value;
        player.SetCustomProperties(playerProperties);
    }
}
