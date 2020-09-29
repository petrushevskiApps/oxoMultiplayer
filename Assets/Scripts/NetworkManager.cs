using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace com.petrushevskiapps.Oxo
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static UnityEvent PlayerEnteredRoom = new UnityEvent();
        private List<RoomInfo> cachedRoomsList = new List<RoomInfo>();
    
        
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
                PhotonNetwork.CreateRoom(roomName);
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
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }
        public override void OnJoinedRoom()
        {
            Debug.Log("PUN:: OnJoinedRoom() called by PUN. Now this client is in a room.");
            
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                // #Critical
                // Load the Room Level.
                PhotonNetwork.LoadLevel("Lobby");
            }
        }
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public void SetNetworkUsername(string userName)
        {
            PhotonNetwork.NickName = userName;
        }
        
        
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }
        private void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel(1);
        }

        public void StartMatch()
        {
            PhotonNetwork.LoadLevel(2);
        }
        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", "test"); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                LoadArena();
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            if (cachedRoomsList != null)
            {
                cachedRoomsList.Clear();
                cachedRoomsList = roomList;
            }
        }

        public bool IsRoomExisting(string roomName)
        {
            if (cachedRoomsList != null && cachedRoomsList.Count > 0)
            {
                return cachedRoomsList.Exists(room => room.Name.Equals(roomName));
            }
            else return false;
        }
    }
}


