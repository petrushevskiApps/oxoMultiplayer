using System.Collections;
using Facebook.Unity;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace com.petrushevskiapps.Oxo
{
    public class FacebookAuth : IAuth
    {
        private bool isConnecting;
        
        public IEnumerator Connect()
        {
            string aToken = AccessToken.CurrentAccessToken.TokenString;
            string facebookId = AccessToken.CurrentAccessToken.UserId;
            PhotonNetwork.AuthValues = new AuthenticationValues();
            PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Facebook;
            PhotonNetwork.AuthValues.UserId = facebookId; // alternatively set by server
            PhotonNetwork.AuthValues.AddAuthParameter("username", facebookId);
            PhotonNetwork.AuthValues.AddAuthParameter("token", aToken);
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = Application.version;
            yield break;
        }

        public bool IsConnecting()
        {
            return isConnecting;
        }
    }
}