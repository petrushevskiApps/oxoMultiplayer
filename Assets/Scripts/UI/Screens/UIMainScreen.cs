using System;
using com.petrushevskiapps.Oxo;
using PetrushevskiApps.UIManager;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class UIMainScreen : UIScreen
{
    [SerializeField] private UIButton userScreenBtn;
    [SerializeField] private UIButton helpBtn;
    [SerializeField] private UIButton settingsBtn;


    private void Start()
    {
        userScreenBtn.onClick.AddListener(UIManager.Instance.OpenScreen<UIUserScreen>);
        helpBtn.onClick.AddListener(UIManager.Instance.OpenScreen<UIHelpScreen>);
        settingsBtn.onClick.AddListener(()=> UIManager.Instance.OpenPopup<UISettingsPopup>());
    }
}
