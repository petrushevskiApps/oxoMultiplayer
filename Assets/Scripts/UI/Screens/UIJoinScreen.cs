using System;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using PetrushevskiApps.UIManager;
using UnityEngine;
using UnityEngine.UI;

public class UIJoinScreen : UIScreen
{
    [SerializeField] private UIButton joinBtn;
    [SerializeField] private InputField inputField;
    
    private void Awake()
    {
        base.Awake();
        joinBtn.onClick.AddListener(JoinRoom);
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
        joinBtn.SetInteractableStatus(!string.IsNullOrEmpty(input));
    }
    private void JoinRoom()
    {
        string roomName = inputField.text;
        
        if (ValidateRoomName(roomName))
        {
            NetworkManager.Instance.JoinRoom(roomName);
        }
    }

    private bool ValidateRoomName(string roomName)
    {
        if (!NetworkManager.Instance.IsRoomExisting(roomName))
        {
            UIManager.Instance.OpenPopup<UIMessagePopup>()
                .SetTitle(Constants.ROOM_NOT_EXIST_TITLE_TXT)
                .SetMessage(string.Format(Constants.ROOM_NOT_EXIST_MESSAGE_TXT, roomName));
            return false;
        }

        return true;
    }
}
