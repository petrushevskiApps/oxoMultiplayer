using System;
using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
using PetrushevskiApps.UIManager;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

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
        if (scene.buildIndex != 0) return;

        if (PhotonNetwork.InRoom)
        {
            UIManager.Instance.OpenScreen<UILobbyScreen>();
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
