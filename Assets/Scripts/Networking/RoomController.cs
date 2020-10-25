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
    public static UnityIntegerEvent RoomTurnChange = new UnityIntegerEvent();
    
    public static string RoomName => PhotonNetwork.CurrentRoom.Name;
    public static bool IsRoomFull => PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount;

    // Players in Room
    public List<NetworkPlayer> PlayersInRoom => networkPlayers.Values.ToList();
    public NetworkPlayer LocalPlayer { get; private set; }
    public NetworkPlayer ActivePlayer => PlayersInRoom.FirstOrDefault(x => x.IsActive);
    
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
        
        
        foreach(Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            CreateNetworkPlayer(player);
        }
        SetupRoomProperties();
    }
    
    private void SetupRoomProperties()
    {
        roomProperties = new Hashtable {{Keys.ROOM_TURN, 0}};
        PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
    }
    public static void SetRoomProperty(string key, object value)
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable{{key,value}});
        Debug.Log(key + ": Value: " + value);
    }
    public static int GetRoomProperty(string key)
    {
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(key, out var result);
        return (int?) result ?? 0;
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

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        
        if (propertiesThatChanged.ContainsKey(Keys.ROOM_TURN))
        {
            RoomTurnChange.Invoke((int)propertiesThatChanged[Keys.ROOM_TURN]);
        }
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
