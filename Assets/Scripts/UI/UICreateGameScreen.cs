using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.menumanager;
using PetrushevskiApps.UIManager;
using UnityEngine;
using UnityEngine.UI;

public class UICreateGameScreen : UIScreen
{
    [SerializeField] private UIButton createBtn;
    [SerializeField] private InputField inputField;
    
    private void Awake()
    {
        createBtn.onClick.AddListener(JoinRoom);
    }
    
    private void JoinRoom()
    {
        string roomName = inputField.text;
        
        if (ValidateRoomName(roomName))
        {
            GameManager.Instance.NetworkManager.CreateRoom(roomName);
        }
    }

    private bool ValidateRoomName(string roomName)
    {
        if (string.IsNullOrEmpty(roomName))
        {
            Debug.Log("Empty room name! Please enter room name!");
            UIManager.Instance.OpenPopup<UIMessagePopup>()
                .SetTitle("EMPTY ROOM NAME")
                .SetMessage("Please enter room name and try again...");
            return false;
        }

        if (GameManager.Instance.NetworkManager.IsRoomExisting(roomName))
        {
            UIManager.Instance.OpenPopup<UIMessagePopup>()
                .SetTitle("ROOM EXISTS")
                .SetMessage("Room with name: <b>" + roomName + "</b> already exists! Please enter another room name and try again.");
            return false;
        }

        return true;
    }
}
