using System;
using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
using PetrushevskiApps.UIManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIDifficultyScreen : UIScreen
{
    [SerializeField] private List<ButtonConfiguration> buttonConfigurations;

    private void Awake()
    {
        buttonConfigurations.ForEach(config => config.SetButtonAction(OnButtonClick));
    }

    private void OnButtonClick(RoomConfiguration roomConfiguration)
    {
        UIManager.Instance.OpenScreen<UILoadingScreen>();
        
        NetworkManager.Instance.SetConfiguration(roomConfiguration);
        
        if (NetworkManager.Instance.ConnectionController.PlayOffline)
        {
           NetworkManager.Instance.CreateRoom();
           return;
        }
        NetworkManager.Instance.JoinRandomRoom();
    }
    
    
    [Serializable]
    public class ButtonConfiguration
    {
        [SerializeField] private UIButton button;
        [SerializeField] private RoomConfiguration configuration;

        public void SetButtonAction(UnityAction<RoomConfiguration> onButtonClick)
        {
            button.onClick.AddListener(() => onButtonClick.Invoke(configuration));
        }
    }
}

