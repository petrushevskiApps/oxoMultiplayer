using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomController : MonoBehaviourPunCallbacks
{
    //Events
    public static PlayerRoomEvent PlayerEnteredRoom = new PlayerRoomEvent();
    public static PlayerRoomEvent PlayerExitedRoom = new PlayerRoomEvent();
    public static RoomStatusChangeEvent RoomStatusChange = new RoomStatusChangeEvent();
    
    public static string RoomName => PhotonNetwork.CurrentRoom.Name;
    public static bool IsRoomFull => PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount;

    // Players in Room
    public List<NetworkPlayer> PlayersInRoom => networkPlayers.Values.ToList();
    public NetworkPlayer LocalPlayer { get; private set; }
    public NetworkPlayer ActivePlayer => PlayersInRoom.FirstOrDefault(x => x.IsActive());
    
    public RoomStatus RoomCurrentStatus
    {
        get => roomCurrentStatus;
        private set
        {
            if (roomCurrentStatus != value)
            {
                roomCurrentStatus = value;
                RoomStatusChange.Invoke(roomCurrentStatus);
            }
        }
    }
    
    private Hashtable roomProperties;
    private Dictionary<string, NetworkPlayer> networkPlayers = new Dictionary<string, NetworkPlayer>();
    private RoomStatus roomCurrentStatus = RoomStatus.Waiting;
    
   
    // Called when Player enters room
    public void SetupRoomController()
    {
        PhotonNetwork.CurrentRoom.PlayerTtl = 30000; // 30 sec
        SetupRoomProperties();
        
        foreach(Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            CreateNetworkPlayer(player);
        }
    }
    
    private void SetupRoomProperties()
    {
        roomProperties = new Hashtable();
        roomProperties.Add(Keys.ROOM_TURN, 0);
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }
    public void SetRoomProperty(string KEY, object value)
    {
        roomProperties[KEY] = value;
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }
    public int GetRoomProperty(string KEY)
    {
        object result = 0;
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(KEY, out result);
        return (int) result;
    }
    
    public void CleanRoomController()
    {
        networkPlayers.Clear();
        RoomCurrentStatus = RoomStatus.Waiting;
    }
    
    private void SetRoomStatus(bool check = false)
    {
        bool roomReady = true;
        
        if (!IsRoomFull)
        {
            RoomCurrentStatus = RoomStatus.Waiting;
        }
        else
        {
            foreach (NetworkPlayer player in networkPlayers.Values)
            {
                roomReady &= player.IsReady;
            }
            
            RoomCurrentStatus = roomReady ? RoomStatus.Ready : RoomStatus.Full;
        }
    }
    private void CreateNetworkPlayer(Player player)
    {
        if (networkPlayers.ContainsKey(player.UserId)) return;
        
        NetworkPlayer netPlayer = new NetworkPlayer(player);
        netPlayer.PlayerStatusChange.AddListener(SetRoomStatus);
        networkPlayers.Add(player.UserId, netPlayer);
        
        if (player.IsLocal) LocalPlayer = netPlayer;
        PlayerEnteredRoom.Invoke(netPlayer);
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        CreateNetworkPlayer(newPlayer);
        SetRoomStatus();
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (!networkPlayers.ContainsKey(otherPlayer.UserId)) return;
        
        PlayerExitedRoom.Invoke(networkPlayers[otherPlayer.UserId]);
        networkPlayers.Remove(otherPlayer.UserId);
        SetRoomStatus();
        
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (networkPlayers.ContainsKey(targetPlayer.UserId))
        {
            networkPlayers[targetPlayer.UserId].UpdatePlayerStatuses(changedProps);
        }
    }
    
    public class PlayerRoomEvent : UnityEvent<NetworkPlayer>{}
    public class RoomStatusChangeEvent: UnityEvent<RoomStatus>{}

    public enum RoomStatus
    {
        Waiting,
        Full,
        Ready,
    }
}
