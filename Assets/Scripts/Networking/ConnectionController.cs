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
        public UnityBoolEvent NetworkStatusChange = new UnityBoolEvent();

        private Action ExecuteOnDisconnected;
        private Action ExecuteOnConnected;
        public bool IsConnected
        {
            get => networkStatus;
            private set
            {
                if (networkStatus == value) return;
                networkStatus = value;
                NetworkStatusChange.Invoke(value);
            }
        }

        public bool PlayOffline { get; private set; }

        private bool networkStatus = false;

        private string gameVersion = "1";
        private bool isConnecting = false;
        private bool masterConnectionEstablished = false;
        
        private Coroutine connectingCoroutine;
        private Coroutine reconnectCoroutine;

        private IAuth authStrategy;
        
        private void Awake()
        {
            //            GameManager.Instance.FacebookService.LoginCompleted.AddListener(OnFacebookLogin);
            
            SetAuthAndConnect(new DefaultAuth());
            PhotonNetwork.AutomaticallySyncScene = true;

        }


        private void OnDestroy()
        {
            StopCoroutines();
        }
        
        public void SetAuthAndConnect(IAuth auth)
        {
            authStrategy = auth;
            Connect();
        }
        
        private void Connect()
        {
            connectingCoroutine = StartCoroutine(authStrategy.Connect(gameVersion));
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

            reconnectCoroutine = null;
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
        
        public void SetOfflineMode(bool offlineMode, Action OnModeReady)
        {
            if (PlayOffline == offlineMode)
            {
                OnModeReady.Invoke();
                return;
            }

            PlayOffline = offlineMode;

            if (!IsConnected && offlineMode)
            {
                PhotonNetwork.OfflineMode = PlayOffline;
                OnModeReady.Invoke();
            }
            else if (IsConnected && offlineMode)
            {
                ExecuteOnDisconnected = () =>
                {
                    PhotonNetwork.OfflineMode = PlayOffline;
                    OnModeReady.Invoke();
                    ExecuteOnDisconnected = null;
                };
                
                PhotonNetwork.Disconnect();
            }
            else if (!IsConnected && !offlineMode)
            {
                PhotonNetwork.OfflineMode = PlayOffline;
                Connect();
                ExecuteOnConnected = () =>
                {
                    OnModeReady.Invoke();
                    ExecuteOnConnected = null;
                };
            }
            else if (IsConnected && !offlineMode)
            {
                PhotonNetwork.OfflineMode = PlayOffline;
                OnModeReady.Invoke();
            }
        }

        public override void OnConnected()
        {
            base.OnConnected();
            IsConnected = true;
            
            if(ExecuteOnConnected != null)
            {
                ExecuteOnConnected.Invoke();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            IsConnected = false;

            if (PlayOffline && ExecuteOnDisconnected != null)
            {
                ExecuteOnDisconnected.Invoke();
                return;
            }

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
                    Connect();
                }
            }
            
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


        //        private void OnFacebookLogin(string tokenString, string userId)
        //        {
        //            SetAuthAndConnect(new FacebookAuth());
        //        }

    }
}