using System;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
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

    [SerializeField] private MatchController matchController;
    
    private void Awake()
    {
        base.Awake();
        
        startGame.onClick.AddListener(StartGame);
        playerReady.onClick.AddListener(OnPlayerReadyClicked);
        backButton.onClick.AddListener(ExitRoom);
    }

    private void OnEnable()
    {
        RoomController.RoomStatusChange.AddListener(SetStartButton);
        RoomController.RoomStatusChange.AddListener(SetRoomLabel);
        
        SetRoomTitle();
        SetRoomLabel(NetworkManager.Instance.RoomController.RoomCurrentStatus);

        if (NetworkManager.Instance.IsMasterClient)
        {
            SetButtonsStatus(true, false);
            SetStartButton(NetworkManager.Instance.RoomController.RoomCurrentStatus);
        }
        else
        {
            SetButtonsStatus(false, true);
        }
    }
    public void OnDisable()
    {
        RoomController.RoomStatusChange.RemoveListener(SetStartButton);
        RoomController.RoomStatusChange.AddListener(SetRoomLabel);
        
        SetButtonsStatus(false, false);
    }
    
    private void SetButtonsStatus(bool startBtnShow, bool readyBtnShow)
    {
        startGame.gameObject.SetActive(startBtnShow);
        playerReady.gameObject.SetActive(readyBtnShow);
    }
    
    private void SetStartButton(RoomController.RoomStatus currentRoomStatus)
    {
        startGame.SetInteractableStatus(currentRoomStatus == RoomController.RoomStatus.Ready);
    }

    private void OnPlayerReadyClicked()
    {
        NetworkManager.Instance.ChangePlayerProperty(Constants.PLAYER_READY_KEY, true);
    }
   
    private void SetRoomTitle()
    {
        titleText.text = RoomController.RoomName;
    }

    private void SetRoomLabel(RoomController.RoomStatus currentRoomStatus)
    {
        switch (currentRoomStatus)
        {
            case RoomController.RoomStatus.Waiting:
                labelText.text = Constants.WAITING_PLAYERS_MESSAGE;
                break;
            case RoomController.RoomStatus.Full:
                labelText.text = Constants.PLAYERS_NOT_READY_MESSAGE;
                break;
            case RoomController.RoomStatus.Ready:
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
        ExitRoom();
    }
}
