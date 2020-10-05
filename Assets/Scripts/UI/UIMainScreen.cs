using System;
using com.petrushevskiapps.Oxo;
using PetrushevskiApps.UIManager;
using UnityEngine;

public class UIMainScreen : UIScreen
{
    [SerializeField] private UIButton playRandomBtn;
    [SerializeField] private UIButton joinGameBtn;
    [SerializeField] private UIButton createGameBtn;
    [SerializeField] private UIButton userScreenBtn;
    [SerializeField] private UIButton helpBtn;
    [SerializeField] private UIButton settingsBtn;

    private void OnEnable()
    {
        if (string.IsNullOrEmpty(PlayerDataController.Username))
        {
            UIManager.Instance.OpenScreen<UIUsernameScreen>();
        }
    }

    private void Start()
    {
        joinGameBtn.onClick.AddListener(UIManager.Instance.OpenScreen<UIJoinScreen>);
        playRandomBtn.onClick.AddListener(GameManager.Instance.NetworkManager.JoinRandomRoom);
        createGameBtn.onClick.AddListener(UIManager.Instance.OpenScreen<UICreateGameScreen>);
        userScreenBtn.onClick.AddListener(UIManager.Instance.OpenScreen<UIUserScreen>);
        helpBtn.onClick.AddListener(UIManager.Instance.OpenScreen<UIHelpScreen>);
        settingsBtn.onClick.AddListener(()=> UIManager.Instance.OpenPopup<UISettingsPopup>());
    }

}
