using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.petrushevskiapps.Oxo;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomController : MonoBehaviourPunCallbacks
{
    public static PlayerRoomEvent PlayerEnteredRoom = new PlayerRoomEvent();
    public static PlayerRoomEvent PlayerExitedRoom = new PlayerRoomEvent();
    public static RoomStatusChangeEvent RoomStatusChange = new RoomStatusChangeEvent();
    
    public static string RoomName => PhotonNetwork.CurrentRoom.Name;
    public static bool IsRoomFull => PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount;
    
    private Dictionary<string, NetworkPlayer> players = new Dictionary<string, NetworkPlayer>();

    public List<NetworkPlayer> GetPlayersInRoom => players.Values.ToList();
    public bool IsRoomReady => RoomCurrentStatus == RoomStatus.Ready;

    private RoomStatus roomCurrentStatus = RoomStatus.Waiting;
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

    // Called when Player enters room
    public void SetupRoomController()
    {
        foreach(Photon.Realtime.Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            NetworkPlayer netPlayer = new NetworkPlayer(player);
            netPlayer.PlayerStatusChange.AddListener(SetRoomStatus);
            players.Add(player.UserId, netPlayer);
        }
    }

    public void CleanRoomController()
    {
        NetworkManager.Instance.SetupPlayerProperties();
        players.Clear();
        RoomCurrentStatus = RoomStatus.Waiting;
    }
    
    private void SetRoomStatus(bool check = false)
    {
        bool roomReady = true;
        
        if (!IsRoomFull)
        {
            roomReady = false;
        }
        else
        {
            foreach (NetworkPlayer player in players.Values)
            {
                roomReady &= player.IsReady;
            }
        }
        
        if (roomReady)
        {
            RoomCurrentStatus = RoomStatus.Ready;
        }
        else
        {
            RoomCurrentStatus = IsRoomFull ? RoomStatus.Full : RoomStatus.Waiting;
        }
    }
    
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (players.ContainsKey(newPlayer.UserId)) return;
        
        NetworkPlayer netPlayer = new NetworkPlayer(newPlayer);
        netPlayer.PlayerStatusChange.AddListener(SetRoomStatus);
        players.Add(newPlayer.UserId, netPlayer);
        PlayerEnteredRoom.Invoke(netPlayer);
        SetRoomStatus();
        
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        if (!players.ContainsKey(otherPlayer.UserId)) return;
        PlayerExitedRoom.Invoke(players[otherPlayer.UserId]);
        players.Remove(otherPlayer.UserId);
        SetRoomStatus();
        
    }

    
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        if (players.ContainsKey(targetPlayer.UserId))
        {
            players[targetPlayer.UserId].UpdatePlayerStatuses(changedProps);
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
