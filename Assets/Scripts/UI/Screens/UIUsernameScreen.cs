using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
using PetrushevskiApps.UIManager;
using UnityEngine;
using UnityEngine.UI;

public class UIUsernameScreen : UIScreen
{
    [SerializeField] private Button saveBtn;
    [SerializeField] private InputField inputField;
//    
//    [SerializeField] private Button defaultConnect;
//    [SerializeField] private Button facebookConnect;
    
    private void Awake()
    {
        base.Awake();
        saveBtn.interactable = false;
        saveBtn.onClick.AddListener(SaveChanges);
//        defaultConnect.onClick.AddListener(OnDefaultConnectButton);
//        facebookConnect.onClick.AddListener(OnFacebookConnectButton);
        inputField.onValueChanged.AddListener(EnableSaveButton);
    }
    private void OnDestroy()
    {
        saveBtn.onClick.RemoveListener(SaveChanges);
//        defaultConnect.onClick.RemoveListener(OnDefaultConnectButton);
//        facebookConnect.onClick.RemoveListener(OnFacebookConnectButton);
        inputField.onValueChanged.RemoveListener(EnableSaveButton);
    }
//    
//    private void OnFacebookConnectButton()
//    {
//        GameManager.Instance.FacebookService.LogIn();
//    }
//
//    private void OnDefaultConnectButton()
//    {
//        NetworkManager.Instance.ConnectionController.SetAuthAndConnect(new DefaultAuth());
//    }
    private void Start()
    {
        SetDefaultUsername();
    }
    
    private void SetDefaultUsername()
    {
        if (inputField != null)
        {
            inputField.text = PlayerDataController.Username;
        }
    }

    private void SaveChanges()
    {
        if (inputField != null)
        {
            string userName = inputField.text;
                
            if (!string.IsNullOrEmpty(userName))
            {
                PlayerDataController.Username = userName;
                OnBackButtonPressed();
            }
        }
    }

    
        
    private void EnableSaveButton(string userName)
    {
        if (!string.IsNullOrEmpty(userName))
        {
            saveBtn.interactable = true;
        }
        else saveBtn.interactable = false;
    }
}
