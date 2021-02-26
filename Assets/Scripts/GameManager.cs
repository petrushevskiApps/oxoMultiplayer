﻿using com.petrushevskiapps.Oxo;
using Data;
using PetrushevskiApps.UIManager;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private AiNicknames nicknames;
    public AiNicknames Nicknames
    {
        get => nicknames;
    }

    public FacebookService FacebookService { get; private set; }
    public bool IsApplicationQuiting { get; private set; }

    protected override void RegisterListeners()
    {
//        FacebookService = GetComponent<FacebookService>();
//        FacebookService.Initialize();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    protected override void UnregisterListeners()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        if (PhotonNetwork.InRoom)
        {
            UIManager.Instance.OpenScreen<UIGameScreen>();
        }
        else
        {
            if (PlayerDataController.IsUsernameSet)
            {
                UIManager.Instance.OpenScreen<UIMainScreen>();
                NetworkManager.Instance.ConnectionController.SetOfflineMode(false, ()=> { });
            }
            else UIManager.Instance.OpenScreen<UIUsernameScreen>();
        }
    }

    private void OnApplicationQuit()
    {
        IsApplicationQuiting = true;
    }
    
    
}
