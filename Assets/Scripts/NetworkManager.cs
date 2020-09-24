using System;
using System.Collections;
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

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        string gameVersion = "1";
        
        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        private bool isConnecting = false;
        
        public void SetupNetwork()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients
            // in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
            Connect();
        }
        
        /// <summary>
        /// Start the connection process.
        /// Connect this application instance to Photon Cloud Network
        /// </summary>
        public void Connect()
        {
            // Initiate the connection to the server.
            if (!PhotonNetwork.IsConnected)
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
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
        
        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                Debug.Log("PUN:: OnConnectedToMaster() was called by PUN");
                isConnecting = false;
            }
        }

        private Coroutine reconnectCoroutine;
        
        public override void OnDisconnected(DisconnectCause cause)
        {
            isConnecting = false;
            GameManager.Instance.NetworkChecker.OnOnline.AddListener(StartReconnectingCoroutine);
            Debug.LogWarningFormat("PUN:: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        private void OnDestroy()
        {
            GameManager.Instance.NetworkChecker.OnOnline.RemoveListener(StartReconnectingCoroutine);
            
            if (reconnectCoroutine != null)
            {
                StopCoroutine(reconnectCoroutine);
            }
        }

        private void StartReconnectingCoroutine()
        {
            reconnectCoroutine = StartCoroutine(TryToReconnect());
        }
        private IEnumerator TryToReconnect()
        {
            while (!PhotonNetwork.Reconnect())
            {
                yield return new WaitForSeconds(1f);
            }
            GameManager.Instance.NetworkChecker.OnOnline.RemoveListener(StartReconnectingCoroutine);
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
        
        public void SetNetworkUsername(string userName)
        {
            PhotonNetwork.NickName = userName;
        }
        
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
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

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", "test"); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            }
        }
        
    }
}


