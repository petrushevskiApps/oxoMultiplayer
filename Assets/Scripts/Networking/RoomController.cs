using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Properties;
using com.petrushevskiapps.Oxo.Utilities;
using Data;
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
    public static RoomStatusChangeEvent StatusChanged = new RoomStatusChangeEvent();
    public static UnityIntegerEvent TurnChanged = new UnityIntegerEvent();
    
    public static string RoomName => PhotonNetwork.CurrentRoom.Name;
    public static bool IsRoomFull => PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount;

    // Players in Room
    private List<Player> PlayersList => PhotonNetwork.CurrentRoom.Players.Values.ToList();
    public List<NetworkPlayer> PlayersInRoom => networkPlayers.Values.ToList();
    public NetworkPlayer LocalPlayer { get; private set; }
    public NetworkPlayer ActivePlayer => PlayersInRoom.FirstOrDefault(x => x.IsActive);
    
    public RoomStatus Status
    {
        get => Properties.GetProperty<RoomStatus>(Keys.ROOM_STATUS);
        private set
        {
            if (Status == value) return;
            
            StatusChanged.Invoke(value);
            Properties.Set(Keys.ROOM_STATUS, value).Update();
        }
    }

    public RoomState State
    {
        get => Properties.GetProperty<RoomState>(Keys.ROOM_STATE);
        private set
        {
            if(State == value) return;
            Properties.Set(Keys.ROOM_STATE, value);
        }
    }
    private Dictionary<string, NetworkPlayer> networkPlayers = new Dictionary<string, NetworkPlayer>();
    
    public INetworkProperties Properties { get; private set; }
    
    public static RoomController Instance;
    
    private void Awake()
    {
        Instance = this;
        MatchController.MatchStarted.AddListener(OnMatchStarted);
        MatchController.MatchEnded.AddListener(OnMatchEnded);
        Properties = new RoomProperties();
    }

    private void OnDestroy()
    {
        MatchController.MatchStarted.RemoveListener(OnMatchStarted);
        MatchController.MatchEnded.RemoveListener(OnMatchEnded);
    }

    private void Start()
    {
        PlayersList.ForEach(CreateNetworkPlayer);
    }

    private void OnMatchStarted()
    {
        Properties.Set(Keys.ROOM_TURN, 0).Update();
        State = RoomState.InGame;
    }
    
    private void OnMatchEnded(bool arg0)
    {
        Status = RoomStatus.Waiting;
        State = RoomState.InRoom;
    }
    
    private void SetRoomStatus(bool check = false)
    {
        bool roomReady = true;
        
        if (!IsRoomFull)
        {
            Status = RoomStatus.Waiting;
        }
        else
        {
            foreach (NetworkPlayer player in networkPlayers.Values)
            {
                roomReady &= player.IsReady;
            }
            
            Status = roomReady ? RoomStatus.Ready : RoomStatus.Full;
        }
    }
    
    private void CreateNetworkPlayer(Player player)
    {
        if (networkPlayers.ContainsKey(player.UserId)) return;
        
        NetworkPlayer netPlayer = new NetworkPlayer(player);
        netPlayer.ReadyStatusChanged.AddListener(SetRoomStatus);
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
    
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);
        
        if (propertiesThatChanged.ContainsKey(Keys.ROOM_TURN))
        {
            TurnChanged.Invoke((int)propertiesThatChanged[Keys.ROOM_TURN]);
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

    
}
