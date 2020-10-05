using System;
using com.petrushevskiapps.Oxo;
using PetrushevskiApps.UIManager;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyScreen : UIScreen
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private UIButton startGame;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI labelText;

    private void Awake()
    {
        startGame.onClick.AddListener(StartGame);
        backButton.onClick.AddListener(ExitRoom);
    }

    private void OnEnable()
    {
        NetworkManager.PlayerEnteredRoom.AddListener(SetStartButton);
        NetworkManager.PlayerExitedRoom.AddListener(SetStartButton);
        
        SetRoomTitle();
        SetRoomLabel(false);
        SetStartButton();
    }

    public void OnDisable()
    {
        NetworkManager.PlayerEnteredRoom.RemoveListener(SetStartButton);
        NetworkManager.PlayerExitedRoom.RemoveListener(SetStartButton);
    }

    private void SetStartButton(Photon.Realtime.Player player = null)
    {
        bool isRoomReady = PhotonNetwork.CurrentRoom.MaxPlayers == PhotonNetwork.CurrentRoom.PlayerCount;
        startGame.SetInteractableStatus(isRoomReady);
        SetRoomLabel(isRoomReady);
    }
    
    private void ExitRoom()
    {
        GameManager.Instance.NetworkManager.LeaveRoom();
    }

    private void StartGame()
    {
        GameManager.Instance.NetworkManager.StartMatch();
    }
    
    private void SetRoomTitle()
    {
        titleText.text = PhotonNetwork.CurrentRoom.Name;
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
