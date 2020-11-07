﻿using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using Data;
using PetrushevskiApps.UIManager;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool IsUsernameSet { get; private set; }
    public bool IsApplicationQuiting { get; private set; } = false;

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
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        SetUsername();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (PhotonNetwork.InRoom)
        {
            RoomState state = RoomController.Instance.State;

            Debug.Log($"ROOM STATUS:: {state}");

            switch (state)
            {
//                case RoomState.InGame:
//                    UIManager.Instance.OpenScreen<UIGameScreen>();
//                    break;
                default:
                    UIManager.Instance.OpenScreen<UIRoomScreen>();
                    break;
            }
        }
        else
        {
            UIManager.Instance.OpenScreen<UIMainScreen>();
        }
    }

    public void SetUsername(string username = "")
    {
        string nickname = PlayerDataController.Username;

        if (!string.IsNullOrEmpty(nickname))
        {
            NetworkManager.Instance.SetNetworkUsername(nickname);
            IsUsernameSet = true;
        }
        else IsUsernameSet = false;
    }
    
    private void OnApplicationQuit()
    {
        IsApplicationQuiting = true;
    }
    
    
}
