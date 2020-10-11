using System;
using System.Collections;
using System.Collections.Generic;
using PetrushevskiApps.UIManager;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace com.petrushevskiapps.Oxo
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static PlayerRoomEvent PlayerEnteredRoom = new PlayerRoomEvent();
        public static PlayerRoomEvent PlayerExitedRoom = new PlayerRoomEvent();

        private Dictionary<string, RoomInfo> cachedRoomsDictionary = new Dictionary<string, RoomInfo>();
        private bool isApplicationQuiting = false;
        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        public void CreateRoom(string roomName)
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = maxPlayersPerRoom, PublishUserId = true});
            }
        }
        public void JoinRoom(string roomName)
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRoom(roomName);
            }
        }
        public void JoinRandomRoom()
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified
            // in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
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
//            SceneManager.LoadScene(1);
            UIManager.Instance.OpenScreen<UILobbyScreen>();
        }
        public override void OnLeftRoom()
        {
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                UIManager.Instance.OpenScreen<UIMainScreen>();
            }
            else
            {
                // Prevent loading scene when application is quiting
                if(!isApplicationQuiting) SceneManager.LoadScene(0);
            }
            
        }
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        
        public void StartMatch()
        {
            PhotonNetwork.LoadLevel(1);
        }
        public override void OnPlayerEnteredRoom(Photon.Realtime.Player player)
        {
            base.OnPlayerEnteredRoom(player);
            PlayerEnteredRoom.Invoke(player);
        }
        public override void OnPlayerLeftRoom(Photon.Realtime.Player player)
        {
            base.OnPlayerLeftRoom(player);
            PlayerExitedRoom.Invoke(player);
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

        public class PlayerRoomEvent : UnityEvent<Photon.Realtime.Player>
        {
        }

        

        private void OnApplicationQuit()
        {
            isApplicationQuiting = true;
        }
    }
}


