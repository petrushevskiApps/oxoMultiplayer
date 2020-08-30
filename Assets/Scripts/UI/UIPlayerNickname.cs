using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace com.petrushevskiapps.Oxo
{
    public class PlayerNickname : MonoBehaviour
    {
        // Store the PlayerPref Key to avoid typos
        const string playerNamePrefKey = "PlayerName";

        private InputField inputFieldUI;
        
        private void Awake()
        {
            inputFieldUI = GetComponent<InputField>();
            inputFieldUI.onValueChanged.AddListener(SetPlayerName);
        }

        void Start () 
        {
            string defaultName = string.Empty;
            InputField inputField = this.GetComponent<InputField>();
            if (inputField!=null)
            {
                if (PlayerPrefs.HasKey(playerNamePrefKey))
                {
                    inputField.text = PlayerPrefs.GetString(playerNamePrefKey);
                }
            }
            PhotonNetwork.NickName =  defaultName;
        }

        public void SetPlayerName(string value)
        {
            // #Important
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }
            PhotonNetwork.NickName = value;
            PlayerPrefs.SetString(playerNamePrefKey,value);
        }
    }
}


