﻿using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace com.petrushevskiapps.Oxo
{
    public class DefaultAuth : IAuth
    {
        private bool isConnecting;
        
        public IEnumerator Connect()
        {
            // Initiate the connection to the server.
            while (!PhotonNetwork.IsConnected)
            {
                // #Critical, we must first and foremost connect to Photon Online Server.
                
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = Application.version;
                yield return new WaitForSeconds(3f);
            }
        }

        public bool IsConnecting()
        {
            return isConnecting;
        }
    }
}