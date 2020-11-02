using System;
using System.Collections;
using com.petrushevskiapps.Oxo.Utilities;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace com.petrushevskiapps.Oxo
{
    public class ConnectionController : MonoBehaviourPunCallbacks
    {
        public UnityBoolEvent OnNetworkStatusChange = new UnityBoolEvent();
        public UnityEvent OnOnline = new UnityEvent();
        public UnityEvent OnOffline = new UnityEvent();
        
        public bool IsOnline => currNetworkStatus;
        public bool IsOffline => !currNetworkStatus;
        
        private bool prevNetworkStatus = false;
        private bool currNetworkStatus = false;
        private bool isConnecting = false;
        private string gameVersion = "1";
        private bool masterConnectionEstablished = false;
        
        private Coroutine connectingCoroutine;
        private Coroutine reconnectCoroutine;
        
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            connectingCoroutine = StartCoroutine(Connect());
        }
        
        private IEnumerator Connect()
        {
            // Initiate the connection to the server.
            while (!PhotonNetwork.IsConnected)
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
                yield return new WaitForSeconds(3f);
            }
        }
        
        private IEnumerator Reconnect()
        {
            while (!PhotonNetwork.IsConnectedAndReady)
            {
                if (PhotonNetwork.NetworkingClient.State == ClientState.Disconnected)
                {
                    if (!PhotonNetwork.ReconnectAndRejoin())
                    {
                        Debug.LogError("ReconnectAndRejoin failed, trying Reconnect");
                        if (!PhotonNetwork.Reconnect())
                        {
                            Debug.LogError("Reconnect failed, trying ConnectUsingSettings");
                            if (!PhotonNetwork.ConnectUsingSettings())
                            {
                                Debug.LogError("ConnectUsingSettings failed");
                            }
                        }
                    }
                }
               
                yield return new WaitForSeconds(3f);
            }
        }
        
        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                Debug.Log("PUN:: OnConnectedToMaster() was called by PUN");
                isConnecting = false;
                masterConnectionEstablished = true;
                StopCoroutines();
            }
            
            PhotonNetwork.JoinLobby(TypedLobby.Default);
            Debug.Log("Lobby Joined!");
        }
        
        public override void OnConnected()
        {
            base.OnConnected();
            UpdateNetworkStatus(true);
            OnOnline.Invoke();
        }

        
        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            PlayerDataController.ClearLastRoom(this);
            UpdateNetworkStatus(false);
            OnOffline.Invoke();

            if (masterConnectionEstablished)
            {
                if (reconnectCoroutine == null)
                {
                    reconnectCoroutine = StartCoroutine(Reconnect());
                }
            }
            else
            {
                if (connectingCoroutine == null)
                {
                    connectingCoroutine = StartCoroutine(Connect());
                }
            }
            
        }
        private void UpdateNetworkStatus(bool netStatus)
        {
            currNetworkStatus = netStatus;
            if (netStatus != prevNetworkStatus)
            {
                prevNetworkStatus = netStatus;
                OnNetworkStatusChange.Invoke(netStatus);
            }
        }
        
        private void OnDestroy()
        {
            StopCoroutines();
        }

        private void StopCoroutines()
        {
            if (reconnectCoroutine != null)
            {
                StopCoroutine(reconnectCoroutine);
            }
            if (connectingCoroutine != null)
            {
                StopCoroutine(connectingCoroutine);
            }
        }
    }
}