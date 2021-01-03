using System;
using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo.Utilities;
using Data;
using PetrushevskiApps.UIManager;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace com.petrushevskiapps.Oxo
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static UnityEvent MasterSwitched = new UnityEvent();
        public static UnityEvent JoinRandomFailed = new UnityEvent();
        public static UnityEvent JoinedRoom = new UnityEvent();
        public static UnityEvent LeftRoom = new UnityEvent();
        
        [Tooltip("The maximum number of players per room.")]
        [Header("Parameters")]
        [SerializeField] private byte maxPlayersPerRoom = 4;

        [Header("Controllers")]
        [SerializeField] private ConnectionController connectionController;
        [SerializeField] private RoomController roomController;
        
        public ConnectionController ConnectionController => connectionController;
        public RoomController RoomController => roomController;
        
        public static bool IsMasterClient => PhotonNetwork.IsMasterClient;
        
        public static NetworkManager Instance;

        private Dictionary<string, RoomInfo> cachedRoomsDictionary = new Dictionary<string, RoomInfo>();

        [SerializeField] private RoomConfiguration configuration;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void CreateRoom(string roomName = null)
        {
            if (PhotonNetwork.IsConnected)
            {
                StartCoroutine(DelayJoin(() =>
                {
                    RoomOptions roomOptions = new RoomOptions();
                    roomOptions.MaxPlayers = maxPlayersPerRoom;
                    roomOptions.PublishUserId = true;
                    roomOptions.CustomRoomPropertiesForLobby = configuration.GetPropertiesNames();
                    roomOptions.CustomRoomProperties = configuration.GetConfigHashtable();
                    PhotonNetwork.CreateRoom(roomName, roomOptions);
                }));
            }
        }
        
        public void JoinRoom(string roomName)
        {
            if (PhotonNetwork.IsConnected)
            {
                StartCoroutine(DelayJoin(() =>
                {
                    PhotonNetwork.JoinRoom(roomName);
                }));
            }
        }
        
        public void JoinRandomRoom()
        {
            // #Critical we need at this point to attempt joining a Random Room.
            // If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            StartCoroutine(DelayJoin(() => { PhotonNetwork.JoinRandomRoom(); }));
        }
        public void JoinRandomRoom(RoomConfiguration configuration)
        {
            // #Critical we need at this point to attempt joining a Random Room.
            // If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            this.configuration = configuration;
            
            StartCoroutine(DelayJoin(() =>
            {
                PhotonNetwork.JoinRandomRoom(configuration.GetConfigHashtable(), maxPlayersPerRoom);
            }));
        }
        
        IEnumerator DelayJoin(Action joinAction)
        {
            yield return new WaitForSeconds(1f);
            joinAction.Invoke();
        }
        
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            JoinRandomFailed.Invoke();
            
            Debug.Log("PUN:: OnJoinRandomFailed() was called by PUN. " +
                      "No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            CreateRoom();
        }
        
        public override void OnJoinedRoom()
        {
            Debug.Log("PUN:: OnJoinedRoom() called by PUN. Now this client is in a room.");
            
            JoinedRoom.Invoke();
        }
        
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        
        public override void OnLeftRoom()
        {
            LeftRoom.Invoke();
            // Prevent loading scene when application is quiting
            if (!GameManager.Instance.IsApplicationQuiting)
            {
                ChangeScene(SceneTypes.Menu);
            }
        }

        public void ChangeScene(SceneTypes sceneType)
        {
            if(PhotonNetwork.InRoom && !PhotonNetwork.IsMasterClient) return;
            
            PhotonNetwork.LoadLevel((int)sceneType);
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            Debug.Log("Room List Retrieved!! ");
            
            roomList.ForEach(room =>
            {
                if (room.RemovedFromList)
                {
                    if (!cachedRoomsDictionary.ContainsKey(room.Name)) return;
                    cachedRoomsDictionary.Remove(room.Name);
                    Debug.Log("Room: " + room.Name + " removed!!");
                }
                else
                {
                    if (cachedRoomsDictionary.ContainsKey(room.Name)) return;
                    cachedRoomsDictionary.Add(room.Name, room);
                    Debug.Log("Room: " + room.Name + " added!!");
                    Debug.Log("Room Is Open: " + roomList[0].IsOpen);
                }
            });
        }

        public bool IsRoomExisting(string roomName)
        {
            return cachedRoomsDictionary.ContainsKey(roomName);
        }
        
        public void SetNetworkUsername(string userName)
        {
            PhotonNetwork.NickName = userName;
        }
        
        
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);
            MasterSwitched.Invoke();
        }
        
        public void SendRpc(PhotonView pv, string rpcMethodName, bool overrideMaster, params object[] parameters)
        {
            if (!PhotonNetwork.IsMasterClient && !overrideMaster) return;
            pv.RPC(rpcMethodName, RpcTarget.AllBufferedViaServer, parameters);
                
            Debug.Log($"Buffered RPCs Count: {RoomController.LocalRpcBufferCount}");
        }

        public void ClearRpcs(PhotonView pv)
        {
            RoomController.LocalRpcBufferCount = 0;
            
            Debug.Log($"Clean RPC Buffer");
            
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.RemoveRPCs(pv);
            }
        }
    }
}



