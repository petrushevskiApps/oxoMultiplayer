using System;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using Data;
using ExitGames.Client.Photon;
using PetrushevskiApps.UIManager;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRoomScreen : UIScreen
{
    [Header("Buttons")]
    [SerializeField] private UIButton startGame;
    [SerializeField] private UIButton playerReady;
    [SerializeField] private Button backButton;
    
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI labelText;

    private void Awake()
    {
        base.Awake();
        
        startGame.onClick.AddListener(StartGame);
        playerReady.onClick.AddListener(OnPlayerReadyClicked);
        backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnEnable()
    {
        RoomController.StatusChanged.AddListener(SetStartButton);
        RoomController.StatusChanged.AddListener(SetRoomLabel);
        
        SetRoomTitle();
        SetRoomLabel(RoomController.Instance.Status);

        if (NetworkManager.Instance.IsMasterClient)
        {
            SetButtonsStatus(true, false);
            SetStartButton(RoomController.Instance.Status);
        }
        else
        {
            SetButtonsStatus(false, true);
        }
    }
    public void OnDisable()
    {
        RoomController.StatusChanged.RemoveListener(SetStartButton);
        RoomController.StatusChanged.AddListener(SetRoomLabel);
        
        SetButtonsStatus(false, false);
    }
    
    private void SetButtonsStatus(bool startBtnShow, bool readyBtnShow)
    {
        startGame.gameObject.SetActive(startBtnShow);
        playerReady.gameObject.SetActive(readyBtnShow);
    }
    
    private void SetStartButton(RoomStatus currentRoomStatus)
    {
        startGame.SetInteractableStatus(currentRoomStatus == RoomStatus.Ready);
    }

    private void OnPlayerReadyClicked()
    {
        RoomController.Instance.LocalPlayer.IsReady = true;
    }
   
    private void SetRoomTitle()
    {
        titleText.text = RoomController.RoomName;
    }

    private void SetRoomLabel(RoomStatus currentRoomStatus)
    {
        switch (currentRoomStatus)
        {
            case RoomStatus.Waiting:
                labelText.text = Constants.WAITING_PLAYERS_MESSAGE;
                break;
            case RoomStatus.Full:
                labelText.text = Constants.PLAYERS_NOT_READY_MESSAGE;
                break;
            case RoomStatus.Ready:
                labelText.text = Constants.PLAYERS_READY_MESSAGE;
                break;
            default:
                labelText.text = Constants.WAITING_PLAYERS_MESSAGE;
                break;
        }
    }

    private void StartGame() => MatchController.LocalInstance.StartMatch();

    private void ExitRoom() => NetworkManager.Instance.LeaveRoom();

    protected override void OnBackButtonPressed()
    {
        UIManager.Instance.OpenPopup<UILeavePopup>()
            .SetTitle(Constants.LEAVE_ROOM_TITLE)
            .SetMessage(Constants.LEAVE_ROOM_MESSAGE);
    }
}
