using System;
using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;
    public NetworkManager NetworkManager => networkManager;
    
    public static GameManager Instance;
    public bool IsUsernameSet { get; private set; }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        NetworkManager.SetupNetwork();
        SetUsername();
        UIMenuController.Instance.ShowMainMenu();
    }

    public void SetUsername(string username = "")
    {
        string nickname = PlayerPrefs.GetString("PlayerName");

        if (!string.IsNullOrEmpty(nickname))
        {
            NetworkManager.SetNetworkUsername(nickname);
            IsUsernameSet = true;
        }
        else IsUsernameSet = false;
    }

}
