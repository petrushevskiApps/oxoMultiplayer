using System;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using PetrushevskiApps.UIManager;
using TMPro;
using UnityEngine;

public class UILoadingScreen : UIScreen
{
    [SerializeField] private TextMeshProUGUI message;
    
    public override void Initialize(Action onBackButtonAction)
    {
        base.Initialize(() => 
        {
            NetworkManager.Instance.LeaveRoom();
            onBackButtonAction?.Invoke();
        });
        NetworkManager.JoinedRoom.AddListener(OnJoinedRoom);
        NetworkManager.JoinRandomFailed.AddListener(OnJoinRandomFailed);
        
        SetMessage(LoadingMessage.ROOM_SEARCHING);
        
    }

    private void OnDestroy()
    {
        NetworkManager.JoinedRoom.RemoveListener(OnJoinedRoom);
        NetworkManager.JoinRandomFailed.RemoveListener(OnJoinRandomFailed);
    }

    private void OnJoinedRoom()
    {
        SetMessage(LoadingMessage.ROOM_JOINED);
    }
    private void OnJoinRandomFailed()
    {
        SetMessage(LoadingMessage.ROOM_NOT_FOUND);
    }
    
    private void SetMessage(string textMessage)
    {
        message.text = textMessage;
    }
}
