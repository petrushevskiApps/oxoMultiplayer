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
    public static UnityEvent RpcBufferCountUpdated = new UnityEvent();
    public static UnityEvent LocalRpcBufferCountUpdated = new UnityEvent();
    
    public static string RoomName => PhotonNetwork.CurrentRoom.Name;
    public static int MaxPlayers => PhotonNetwork.CurrentRoom.MaxPlayers;

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
            
           
            Properties.Set(Keys.ROOM_STATUS, value).Sync();
        }
    }

    public bool IsSynced => LocalRpcBufferCount == RoomRpcBufferedCount;
    
    private int localRpcBufferCount;
    
    public int LocalRpcBufferCount
    {
        get => localRpcBufferCount;
        set
        {
            localRpcBufferCount = value;
            LocalRpcBufferCountUpdated.Invoke();
            Debug.Log($"RPC LOCAL COUNT:: {localRpcBufferCount}");
            // Sync room buffered count when count increased or restarted.
            if (localRpcBufferCount > RoomRpcBufferedCount || localRpcBufferCount == 0)
            {
                RoomRpcBufferedCount = localRpcBufferCount;
            }
        }
    }
    
    private int RoomRpcBufferedCount
    {
        get => Properties.GetProperty<int>(Keys.RPC_BUFFERED_COUNT);
        set
        {
            if (RoomRpcBufferedCount == value) return;
            
            Properties.Set(Keys.RPC_BUFFERED_COUNT, value).Sync();
        }
    }
    
    private Dictionary<string, NetworkPlayer> networkPlayers = new Dictionary<string, NetworkPlayer>();
    
    public RoomProperties Properties { get; private set; }
    
    public static RoomController Instance;
    
    private void Awake()
    {
        Instance = this;
        MatchController.MatchEnd.AddListener(OnMatchEnded);
        MatchController.MatchStart.AddListener(OnMatchStarted);
        
        Properties = new RoomProperties();
        Properties.SetPlayerTTL(RoomProperties.PLAYER_TTL_DEFAULT);
        PlayersList.ForEach(CreateNetworkPlayer);
    }

    private void OnDestroy()
    {
        MatchController.MatchStart.RemoveListener(OnMatchStarted);
        MatchController.MatchEnd.RemoveListener(OnMatchEnded);
    }
    
    private void OnMatchStarted()
    {
        Properties.SetPlayerTTL(RoomProperties.PLAYER_TTL_IN_GAME);
    }
    
    private void OnMatchEnded(bool arg0)
    {
        Properties.SetPlayerTTL(RoomProperties.PLAYER_TTL_DEFAULT);
        Status = RoomStatus.Waiting;
    }
    
    private void SetRoomStatus(bool check = false)
    {
        if (networkPlayers.Values.Count < MaxPlayers)
        {
            Status = RoomStatus.Waiting;
        }
        else
        {
            bool isReady = networkPlayers.Values.Aggregate(true, (roomReady, player) => roomReady && player.IsReady);
            
            Status = isReady ? RoomStatus.Ready : RoomStatus.Full;
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
    
    public override void OnRoomPropertiesUpdate(Hashtable changedProperties)
    {
        base.OnRoomPropertiesUpdate(changedProperties);
        
        if (changedProperties.ContainsKey(Keys.RPC_BUFFERED_COUNT))
        {
            RpcBufferCountUpdated.Invoke();
            Properties.Updated(Keys.RPC_BUFFERED_COUNT);
        }
        if (changedProperties.ContainsKey(Keys.ROOM_STATUS))
        {
            StatusChanged.Invoke((RoomStatus)changedProperties[Keys.ROOM_STATUS]);
            Properties.Updated(Keys.RPC_BUFFERED_COUNT);
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
