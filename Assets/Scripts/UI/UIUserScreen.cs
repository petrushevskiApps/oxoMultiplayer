using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace com.petrushevskiapps.Oxo
{
    public class UIUserScreen : MonoBehaviour
    {
        [SerializeField] private Button saveBtn;

        [SerializeField] private InputField inputField;
        
        
        // Store the PlayerPref Key to avoid typos
        const string playerNamePrefKey = "PlayerName";


        private void Awake()
        {
            saveBtn.interactable = false;
            saveBtn.onClick.AddListener(SaveChanges);
            inputField.onValueChanged.AddListener(EnableSaveButton);
        }

        private void OnDestroy()
        {
            saveBtn.onClick.RemoveListener(SaveChanges);
            inputField.onValueChanged.RemoveListener(EnableSaveButton);
        }

        private void Start()
        {
            SetDefaultUsername();
        }

        private void SaveChanges()
        {
            if (inputField != null)
            {
                string userName = inputField.text;
                
                if (!string.IsNullOrEmpty(userName))
                {
                    PlayerPrefs.SetString(playerNamePrefKey, userName);
                    GameManager.Instance.SetUsername(userName);
                    UIMenuController.Instance.ShowMainMenu();
                }
            }
        }

        private void SetDefaultUsername()
        {
            if (inputField!=null)
            {
                inputField.text = PlayerPrefs.GetString(playerNamePrefKey, string.Empty);
            }
        }
        
        private void EnableSaveButton(string userName)
        {
            saveBtn.interactable = true;
        }
    }
}


