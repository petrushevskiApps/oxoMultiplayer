﻿using System;
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

        public bool IsOnline
        {
            get => networkStatus;
            private set
            {
                if (networkStatus == value) return;
                networkStatus = value;
                NetworkStatusChange.Invoke(value);
            }
        }

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

//        private void OnFacebookLogin(string tokenString, string userId)
//        {
//            SetAuthAndConnect(new FacebookAuth());
//        }

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
        
        public override void OnConnected()
        {
            base.OnConnected();
            IsOnline = true;
        }

        
        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            IsOnline = false;

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


        
    }
}