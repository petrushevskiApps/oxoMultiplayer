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
    
    public static UnityEvent LocalRpcBufferCountUpdated = new UnityEvent();

    // Players in Room
    private List<Player> PlayersList => PhotonNetwork.CurrentRoom.Players.Values.ToList();
    public List<IPlayer> PlayersInRoom => players.Values.ToList();
    public IPlayer LocalPlayer  { get; private set; }
    public IPlayer AiPlayer     { get; private set; }
    public IPlayer ActivePlayer => PlayersInRoom.FirstOrDefault(x => x.IsActive());
    public bool IsSynced => LocalRpcBufferCount == RoomRpcBufferedCount;

    private RoomStatus Status
    {
        get => Properties.GetProperty<RoomStatus>(Keys.ROOM_STATUS);

        set
        {
            if (Status == value) return;
           
            Properties.Set(Keys.ROOM_STATUS, value).Sync();
        }
    }

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
    
    public RoomProperties Properties { get; private set; }
    
    private int localRpcBufferCount;
    private Dictionary<string, IPlayer> players;
    
    private void Awake()
    {
        NetworkManager.JoinedRoom.AddListener(OnRoomEntered);
        NetworkManager.LeftRoom.AddListener(OnRoomExited);
    }

    private void OnDestroy()
    {
        NetworkManager.JoinedRoom.RemoveListener(OnRoomEntered);
        NetworkManager.LeftRoom.RemoveListener(OnRoomExited);
    }

    private void OnRoomEntered()
    {
        MatchController.MatchStart.AddListener(OnMatchStarted);
        MatchController.MatchEnd.AddListener(OnMatchEnded);
        
        players = new Dictionary<string, IPlayer>();
        
        Properties = new RoomProperties();
        Properties.SetPlayerTTL(RoomProperties.PLAYER_TTL_DEFAULT);
        
        PlayersList.ForEach(CreateNetworkPlayer);
        
        if (NetworkManager.Instance.ConnectionController.PlayOffline)
        {
            CreateAiPlayer();
        }
    }

    private void OnRoomExited()
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
    
    private void SetRoomStatus()
    {
        if (players.Values.Count < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            Status = RoomStatus.Waiting;
        }
        else
        {
            if(Status == RoomStatus.Ready) return;
            Status = RoomStatus.Ready;
            NetworkManager.Instance.ChangeScene(SceneTypes.Game);
        }
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
        
        if (player.IsLocal) LocalPlayer = netPlayer;
        
        PlayerEnteredRoom.Invoke(netPlayer);
    }

    private void CreateAiPlayer()
    {
        AiPlayer = new AiPlayer(2);
        players.Add(AiPlayer.GetId().ToString(), AiPlayer);
        PlayerEnteredRoom.Invoke(AiPlayer);
        SetRoomStatus();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (players.ContainsKey(targetPlayer.UserId))
        {
            ((NetworkPlayer)players[targetPlayer.UserId]).UpdatePlayerStatuses(changedProps);
        }
    }
    
    
    public void SendRpc(PhotonView pv, string rpcMethodName, bool overrideMaster, params object[] parameters)
    {
        if (!PhotonNetwork.IsMasterClient && !overrideMaster) return;
        
        pv.RPC(rpcMethodName, RpcTarget.AllBufferedViaServer, parameters);
                
        Debug.Log($"Buffered RPCs Count: {LocalRpcBufferCount}");
    }
    
    public void ClearRpcs(PhotonView pv)
    {
        LocalRpcBufferCount = 0;
            
        Debug.Log($"Clean RPC Buffer");
            
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.RemoveRPCs(pv);
        }
    }
    
    
    public class PlayerRoomEvent : UnityEvent<IPlayer>{}
    
}
