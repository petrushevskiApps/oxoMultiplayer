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

    [Header("Context")] 
    [SerializeField] private RoomScreenContext masterContext;
    [SerializeField] private RoomScreenContext clientContext;

    private RoomScreenContext activeContext;
    private void Awake()
    {
        base.Awake();
        
        startGame.onClick.AddListener(StartGame);
        playerReady.onClick.AddListener(OnPlayerReadyClicked);
        backButton.onClick.AddListener(OnBackButtonPressed);
    }

    private void OnEnable()
    {
        NetworkManager.MasterSwitched.AddListener(OnMasterSwitched);
        RoomController.StatusChanged.AddListener(SetStartButton);
        RoomController.StatusChanged.AddListener(SetRoomLabel);
        
        SetupScreen();
    }
    public void OnDisable()
    {
        NetworkManager.MasterSwitched.RemoveListener(OnMasterSwitched);
        RoomController.StatusChanged.RemoveListener(SetStartButton);
        RoomController.StatusChanged.RemoveListener(SetRoomLabel);
    }
    
    private void OnMasterSwitched()
    {
        SetupScreen();
    }

    private void SetupScreen()
    {
        SetContext();
        SetRoomTitle();
        SetRoomLabel(RoomController.Instance.Status);
        SetButtonsStatus();
        SetStartButton(RoomController.Instance.Status);
    }

    
    
    private void SetContext()
    {
        activeContext = NetworkManager.Instance.IsMasterClient ? masterContext : clientContext;
    }
    
    private void SetButtonsStatus()
    {
        startGame.gameObject.SetActive(activeContext.startButtonStatus);
        playerReady.gameObject.SetActive(activeContext.readyButtonStatus);
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
        labelText.text = activeContext.GetRoomLabel(currentRoomStatus);
    }

    private void StartGame() => MatchController.LocalInstance.StartMatch();

    protected override void OnBackButtonPressed()
    {
        UIManager.Instance.OpenPopup<UILeavePopup>()
            .SetTitle(Constants.LEAVE_ROOM_TITLE)
            .SetMessage(Constants.LEAVE_ROOM_MESSAGE);
    }
}
