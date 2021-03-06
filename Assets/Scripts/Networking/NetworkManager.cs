﻿using System;
using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo.Utilities;
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
        
        [Header("Controllers")]
        [SerializeField] private ConnectionController connectionController;

        public ConnectionController ConnectionController => connectionController;
        public bool IsMasterClient => PhotonNetwork.IsMasterClient;
        

        [Tooltip("The maximum number of players per room.")]
        [SerializeField] private byte maxPlayersPerRoom = 4;

        public static NetworkManager Instance;

        
        private Dictionary<string, RoomInfo> cachedRoomsDictionary = new Dictionary<string, RoomInfo>();
        
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

        public void CreateRoom(string roomName)
        {
            if (PhotonNetwork.IsConnected)
            {
                StartCoroutine(DelayJoin(() =>
                {
                    PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom, PublishUserId = true});
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
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified
            // in OnJoinRandomFailed() and we'll create one.
            StartCoroutine(DelayJoin(() => { PhotonNetwork.JoinRandomRoom(); }));

        }

        IEnumerator DelayJoin(Action joinAction)
        {
            UIManager.Instance.OpenScreen<UILoadingScreen>();
            yield return new WaitForSeconds(1f);
            joinAction.Invoke();
        }
        
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN:: OnJoinRandomFailed() was called by PUN. " +
                      "No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom, PublishUserId = true});
        }
        
        public override void OnJoinedRoom()
        {
            Debug.Log("PUN:: OnJoinedRoom() called by PUN. Now this client is in a room.");
            PhotonNetwork.LoadLevel(1);
        }
        
        public override void OnLeftRoom()
        {
            // Prevent loading scene when application is quiting
            if (!GameManager.Instance.IsApplicationQuiting)
            {
                PhotonNetwork.LoadLevel(0);
            }
        }
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            Debug.Log("Room List Retrieved!! ");
            
            roomList.ForEach(room =>
            {
                if (room.RemovedFromList)
                {
                    if (cachedRoomsDictionary.ContainsKey(room.Name))
                    {
                        cachedRoomsDictionary.Remove(room.Name);
                        Debug.Log("Room: " + room.Name + " removed!!");
                    }
                }
                else
                {
                    if (!cachedRoomsDictionary.ContainsKey(room.Name))
                    {
                        cachedRoomsDictionary.Add(room.Name, room);
                        Debug.Log("Room: " + room.Name + " added!!");
                    }
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
            if (PhotonNetwork.IsMasterClient || overrideMaster)
            {
                pv.RPC(rpcMethodName, RpcTarget.AllBufferedViaServer, parameters);
                
                Debug.Log($"Buffered RPCs Count: {RoomController.Instance.LocalRpcBufferCount}");
            }
        }

        public void ClearRpcs(PhotonView pv)
        {
            RoomController.Instance.LocalRpcBufferCount = 0;
            
            Debug.Log($"Clean RPC Buffer");
            
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.RemoveRPCs(pv);
            }
        }
    }
}


