using System;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
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
                .SetTitle(Constants.ROOM_EXIST_TITLE_TXT)
                .SetMessage(string.Format(Constants.ROOM_EXIST_MESSAGE_TXT, roomName));
            return false;
        }
        return true;
    }
}
