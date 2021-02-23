using System.Collections.Generic;
using System.Linq;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Properties;
using com.petrushevskiapps.Oxo.Utilities;
using Data;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public abstract class RoomController : MonoBehaviourPunCallbacks
{
    //Events
    public static PlayerRoomEvent PlayerEnteredRoom = new PlayerRoomEvent();
    public static PlayerRoomEvent PlayerExitedRoom = new PlayerRoomEvent();
    public static UnityEvent LocalRpcBufferCountUpdated = new UnityEvent();

    public List<IPlayer> PlayersInRoom => players.Values.ToList();
    public IPlayer LocalPlayer  { get; protected set; }
    public IPlayer ActivePlayer => PlayersInRoom.FirstOrDefault(x => x.IsActive());
    public bool IsSynced => LocalRpcBufferCount == RoomRpcBufferedCount;
    public RoomProperties Properties { get; private set; }
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
    
    protected RoomStatus Status
    {
        get => Properties.GetProperty<RoomStatus>(Keys.ROOM_STATUS);

        set
        {
            if (Status == value) return;
            Properties.Set(Keys.ROOM_STATUS, value).Sync();
        }
    }

    protected Dictionary<string, IPlayer> players = new Dictionary<string, IPlayer>();

    
    
    private int RoomRpcBufferedCount
    {
        get => Properties.GetProperty<int>(Keys.RPC_BUFFERED_COUNT);
        set
        {
            if (RoomRpcBufferedCount == value) return;
            
            Properties.Set(Keys.RPC_BUFFERED_COUNT, value).Sync();
        }
    }
    
    private int localRpcBufferCount;
    
    
    protected void Awake()
    {
        NetworkManager.JoinedRoom.AddListener(OnRoomEntered);
        NetworkManager.LeftRoom.AddListener(OnRoomExited);
    }

    protected void OnDestroy()
    {
        NetworkManager.JoinedRoom.RemoveListener(OnRoomEntered);
        NetworkManager.LeftRoom.RemoveListener(OnRoomExited);
    }

    protected virtual void OnRoomEntered()
    {
        MatchController.MatchStart.AddListener(OnMatchStarted);
        MatchController.MatchEnd.AddListener(OnMatchEnded);

        players.Clear();
        
        Properties = new RoomProperties();
        Properties.SetPlayerTTL(RoomProperties.PLAYER_TTL_DEFAULT);
        
    }
    
    protected virtual void OnRoomExited()
    {
        MatchController.MatchStart.RemoveListener(OnMatchStarted);
        MatchController.MatchEnd.RemoveListener(OnMatchEnded);

        foreach(KeyValuePair<string, IPlayer> player in players)
        {
            player.Value.Clear();
        }

    }

    protected virtual void OnMatchStarted()
    {
        Properties.SetPlayerTTL(RoomProperties.PLAYER_TTL_IN_GAME);
    }

    protected virtual void OnMatchEnded(bool arg0)
    {
        Properties.SetPlayerTTL(RoomProperties.PLAYER_TTL_DEFAULT);
        Status = RoomStatus.Waiting;
    }
    
    protected void SetRoomStatus()
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
