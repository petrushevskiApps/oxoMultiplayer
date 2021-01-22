using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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


        [Header("Controllers")]
        [SerializeField] private ConnectionController connectionController;
        [SerializeField] private RoomController roomController;
        
        public ConnectionController ConnectionController => connectionController;
        public RoomController RoomController => roomController;
        
        public static bool IsMasterClient => PhotonNetwork.IsMasterClient;

        
        
        public static NetworkManager Instance;

        private Dictionary<string, RoomInfo> cachedRoomsDictionary = new Dictionary<string, RoomInfo>();

        private RoomConfiguration configuration;
        
        
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
            
            PlayerDataController.UsernameChanged.AddListener(SetNetworkUsername);
        }

        private void OnDestroy()
        {
            PlayerDataController.UsernameChanged.RemoveListener(SetNetworkUsername);
        }

        private void Start()
        {
            SetNetworkUsername(PlayerDataController.Username);
        }

        public void CreateRoom(string roomName = null)
        {
            if (PhotonNetwork.IsConnected)
            {
                Timer.Start(this, "CreateRoom", 1, ()=>
                {
                    PhotonNetwork.CreateRoom(roomName, GetRoomOptions());
                });
            }
        }

        private RoomOptions GetRoomOptions()
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = configuration.maxPlayers;
            roomOptions.PublishUserId = configuration.publishUserId;
            roomOptions.CustomRoomPropertiesForLobby = configuration.GetPropertiesNames();
            roomOptions.CustomRoomProperties = configuration.GetConfigHashtable();
            return roomOptions;
        }
        
        public void JoinRoom(string roomName)
        {
            if (PhotonNetwork.IsConnected)
            {
                Timer.Start(this, "JoinRoom", 1, ()=>
                {
                    PhotonNetwork.JoinRoom(roomName);
                });
            }
        }
        public override void OnCustomAuthenticationFailed(string debugMessage)
        {
            Debug.LogErrorFormat("Error authenticating to Photon using Facebook: {0}", debugMessage);
        }
        
        public void JoinRandomRoomQuick()
        {
            Timer.Start(this, "JoinRoomQuick", 1, ()=>
            {
                PhotonNetwork.JoinRandomRoom();
            });
        }
        
        public void JoinRandomRoom()
        {
            // #Critical we need at this point to attempt joining a Random Room.
            // If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            
            Timer.Start(this, "JoinRandomRoom", 1, ()=>
            {
                PhotonNetwork.JoinRandomRoom(configuration.GetConfigHashtable(), configuration.maxPlayers);
            });
        }

        public void SetConfiguration(RoomConfiguration configuration)
        {
            this.configuration = configuration;
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
        
        private void SetNetworkUsername(string userName)
        {
            PhotonNetwork.NickName = userName;
        }
        
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);
            MasterSwitched.Invoke();
        }
        
        

        
    }
}



