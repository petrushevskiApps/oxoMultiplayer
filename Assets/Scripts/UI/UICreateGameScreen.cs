using System;
using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.menumanager;
using com.petrushevskiapps.Oxo;
using PetrushevskiApps.UIManager;
using UnityEngine;
using UnityEngine.UI;

public class UICreateGameScreen : UIScreen
{
    [SerializeField] private UIButton createBtn;
    [SerializeField] private InputField inputField;
    
    private void Awake()
    {
        base.Awake();
        createBtn.onClick.AddListener(JoinRoom);
        inputField.onValueChanged.AddListener(ToggleButtonInteraction);
    }

    private void OnDestroy()
    {
        inputField.onValueChanged.RemoveListener(ToggleButtonInteraction);
    }

    private void OnEnable()
    {
        ToggleButtonInteraction(inputField.text);
    }

    private void ToggleButtonInteraction(string input)
    {
        createBtn.SetInteractableStatus(!string.IsNullOrEmpty(input));
    }

    private void JoinRoom()
    {
        string roomName = inputField.text;
        
        if (ValidateRoomName(roomName))
        {
            NetworkManager.Instance.CreateRoom(roomName);
        }
    }

    private bool ValidateRoomName(string roomName)
    {
        if (NetworkManager.Instance.IsRoomExisting(roomName))
        {
            UIManager.Instance.OpenPopup<UIMessagePopup>()
                .SetTitle("ROOM EXISTS")
                .SetMessage("Room with name: <b>" + roomName + "</b> already exists! Please enter another room name and try again.");
            return false;
        }

        return true;
    }
}
