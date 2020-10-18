using com.petrushevskiapps.Oxo;
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
            Debug.Log("Room: " + roomName + " does not exist!");
            UIManager.Instance.OpenPopup<UIMessagePopup>()
                .SetTitle("WRONG ROOM NAME")
                .SetMessage("Room with name: <b>" + roomName + "</b> does not exist! Please enter another room name and try again.");
            return false;
        }

        return true;
    }
}
