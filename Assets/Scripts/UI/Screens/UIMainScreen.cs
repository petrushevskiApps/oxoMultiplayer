using System;
using com.petrushevskiapps.Oxo;
using PetrushevskiApps.UIManager;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class UIMainScreen : UIScreen
{
    [SerializeField] private UIButton playOnline;
    [SerializeField] private UIButton playOffline;
    
    [SerializeField] private UIButton createGameBtn;
    [SerializeField] private UIButton userScreenBtn;
    [SerializeField] private UIButton helpBtn;
    [SerializeField] private UIButton settingsBtn;


    private void Start()
    {
        playOffline.onClick.AddListener(OnPlayOfflineClicked);
        playOnline.onClick.AddListener(OnPlayOnlineClick);
        
        createGameBtn.onClick.AddListener(UIManager.Instance.OpenScreen<UICreateGameScreen>);
        userScreenBtn.onClick.AddListener(UIManager.Instance.OpenScreen<UIUserScreen>);
        helpBtn.onClick.AddListener(UIManager.Instance.OpenScreen<UIHelpScreen>);
        settingsBtn.onClick.AddListener(()=> UIManager.Instance.OpenPopup<UISettingsPopup>());
    }

    private void OnPlayOfflineClicked()
    {
        PhotonNetwork.Disconnect();
        NetworkManager.Instance.ConnectionController.PlayOffline = true;
        UIManager.Instance.OpenScreen<UIDifficultyScreen>();
    }

    private void OnPlayOnlineClick()
    {
        NetworkManager.Instance.ConnectionController.PlayOffline = false;
        UIManager.Instance.OpenScreen<UIDifficultyScreen>();
    }
}
