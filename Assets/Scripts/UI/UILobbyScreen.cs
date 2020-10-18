using System;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using ExitGames.Client.Photon;
using PetrushevskiApps.UIManager;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyScreen : UIScreen
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private UIButton startGame;
    [SerializeField] private UIButton playerReady;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI labelText;

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
        SetRoomLabel(NetworkManager.Instance.RoomController.IsRoomReady);

        if (NetworkManager.Instance.IsMasterClient)
        {
            startGame.gameObject.SetActive(true);
            playerReady.gameObject.SetActive(false);
            SetStartButton(NetworkManager.Instance.RoomController.IsRoomReady);
        }
        else
        {
            startGame.gameObject.SetActive(false);
            playerReady.gameObject.SetActive(true);
        }
    }

    public void OnDisable()
    {
        RoomController.RoomStatusChange.RemoveListener(SetStartButton);
        RoomController.RoomStatusChange.AddListener(SetRoomLabel);
        
        startGame.gameObject.SetActive(false);
        playerReady.gameObject.SetActive(false);
    }

    private void OnPlayerReadyClicked()
    {
        NetworkManager.Instance.ChangePlayerProperty(Constants.PLAYER_READY_KEY, true);
    }
    
    private void SetStartButton(bool isRoomReady)
    {
        startGame.SetInteractableStatus(isRoomReady);
    }

    protected override void OnBackButtonPressed()
    {
        ExitRoom();
    }
    private void StartGame()
    {
        NetworkManager.Instance.StartMatch();
    }
    private void ExitRoom()
    {
        NetworkManager.Instance.LeaveRoom();
    }

    private void SetRoomTitle()
    {
        titleText.text = RoomController.RoomName;
    }

    private void SetRoomLabel(bool isReady)
    {
        if (isReady)
        {
            labelText.text = "All players ready. You can start the game now.";
        }
        else labelText.text = "Waiting for players to join your game...";
    }
}
