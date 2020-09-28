using PetrushevskiApps.UIManager;
using UnityEngine;
using UnityEngine.UI;

public class UIJoinScreen : UIScreen
{
    [SerializeField] private UIButton joinBtn;
    [SerializeField] private InputField inputField;
    
    private void Awake()
    {
        joinBtn.onClick.AddListener(JoinRoom);
    }
    
    private void JoinRoom()
    {
        string roomName = inputField.text;
        
        if (ValidateRoomName(roomName))
        {
            GameManager.Instance.NetworkManager.JoinRoom(roomName);
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

        if (!GameManager.Instance.NetworkManager.IsRoomExisting(roomName))
        {
            Debug.Log("Room: " + roomName + " does not exist!");
            UIManager.Instance.OpenPopup<UIMessagePopup>()
                .SetTitle("WRONG ROOM NAME")
                .SetMessage("Room with name: <b>" + roomName + "</b> does not exist! Please enter another room name and try again.");
            return false;
        }

        return true;
    }
}
