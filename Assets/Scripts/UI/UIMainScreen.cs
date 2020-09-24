using System;
using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.menumanager;
using com.petrushevskiapps.Oxo;
using UnityEngine;
using UnityEngine.UI;

public class UIMainScreen : UIScreen
{
    [SerializeField] private UIAnimatedButton playRandomBtn;
    [SerializeField] private UIAnimatedButton joinGameBtn;
    [SerializeField] private UIAnimatedButton createGameBtn;
    [SerializeField] private UIAnimatedButton userScreenBtn;

    private void Start()
    {
        joinGameBtn.onClick.AddListener(UIManager.Instance.OpenScreen<UIJoinScreen>);
        playRandomBtn.onClick.AddListener(GameManager.Instance.NetworkManager.JoinRandomRoom);
        createGameBtn.onClickUp.AddListener(UIManager.Instance.OpenScreen<UICreateGameScreen>);
        userScreenBtn.onClick.AddListener(UIManager.Instance.OpenScreen<UIUserScreen>);
        GameManager.Instance.NetworkChecker.OnNetworkStatusChange.AddListener(OnNetworkStatusChange);
    }

    private void OnNetworkStatusChange(bool isConnected)
    {
        playRandomBtn.interactable = isConnected;
        joinGameBtn.interactable = isConnected;
        createGameBtn.interactable = isConnected;
    }

    
}
