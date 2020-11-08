using System;
using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using PetrushevskiApps.UIManager;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIGameScreen : UIScreen
{
    [Header("Buttons")]
    [SerializeField] private UIButton backButton;
    [SerializeField] private UIButton settingsButton;

    [Header("Round Table")] 
    [SerializeField] private TextMeshProUGUI roundNumberTextTop;

    [Header("Middle Content")]
    [SerializeField] private TextMeshProUGUI roundNumberTextMid;
    [SerializeField] private GameObject middleContent;

    [Header("Players")] 
    [SerializeField] private List<PlayerUI> playerRefs = new List<PlayerUI>();

    [Header("Backgrounds")] 
    [SerializeField] private Color activeColor;
    [SerializeField] private Color normalColor;
    
    [SerializeField] private TileImages symbols;

    private List<NetworkPlayer> players;

    private void Awake()
    {
        base.Awake();
        
        backButton.onClick.AddListener(OnBackButtonPressed);
        settingsButton.onClick.AddListener(() =>
        {
            PhotonNetwork.Disconnect();
//            StartCoroutine(NetworkManager.Instance.ConnectionController.Reconnect());
//            UIManager.Instance.OpenPopup<UISettingsPopup>();
        });
        
        MatchController.MatchStartSynced.AddListener(OnMatchStarted);
        MatchController.MatchEnd.AddListener(OnMatchEnded);
        MatchController.RoundStart.AddListener(OnRoundStarted);
        MatchController.RoundEnd.AddListener(OnRoundEnded);
        RoomController.PlayerEnteredRoom.AddListener(OnPlayerEnteredRoom);
    }

    private void OnPlayerEnteredRoom(NetworkPlayer arg0)
    {
        SetupPlayerRefs();
    }
    

    private void OnDestroy()
    {
        MatchController.MatchStartSynced.RemoveListener(OnMatchStarted);
        MatchController.MatchEnd.RemoveListener(OnMatchEnded);
        MatchController.RoundStart.RemoveListener(OnRoundStarted);
        MatchController.RoundEnd.RemoveListener(OnRoundEnded);
        RoomController.PlayerEnteredRoom.RemoveListener(OnPlayerEnteredRoom);
        ClearPlayerUIs();
        Timer.Stop(this, "MidContent");
    }

    private void OnMatchStarted()
    {
        SetupPlayerRefs();
    }
    private void SetupPlayerRefs()
    {
        players = RoomController.Instance.PlayersInRoom;
        
        for (int i = 0; i < players.Count; i++)
        {
            Debug.Log($"Flow 3:: UIGameScreen:: OnMatchStarted:: Player: {players[i].Nickname}");
            playerRefs[i].Setup(players[i], symbols, activeColor, normalColor);
        }
    }
    private void OnRoundStarted(int round)
    {
        string roundText = "Round " + MatchController.LocalInstance.Round;
        roundNumberTextTop.text = roundText;
        roundNumberTextMid.text = roundText;
        middleContent.SetActive(true);
        Timer.Start(this, "MidContent", 1.5f, () => middleContent.SetActive(false));
    }
  
    private void OnRoundEnded()
    {
        
    }

    private void OnMatchEnded(bool arg0)
    {
        ClearPlayerUIs();
    }

    private void ClearPlayerUIs()
    {
        playerRefs.ForEach(playerRef => playerRef.Clear());
    }

    protected override void OnBackButtonPressed()
    {
        UIManager.Instance.OpenPopup<UILeavePopup>()
            .SetTitle(Constants.LEAVE_MATCH_TITLE)
            .SetMessage(Constants.LEAVE_MATCH_MESSAGE);
    }
    
    [Serializable]
    public class PlayerUI
    {
        private NetworkPlayer player;
        
        [SerializeField] private TextMeshProUGUI nicknameText;
        [SerializeField] private Image symbolImage;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private List<Image> backgrounds;
        
        private Color activeColor;
        private Color normalColor;
        
        public void Setup(NetworkPlayer player, TileImages symbols,Color activeColor,Color normalColor)
        {
            this.player = player;
            this.activeColor = activeColor;
            this.normalColor = normalColor;
            
            nicknameText.text = player.Nickname;
            symbolImage.sprite = symbols.GetEndTileState(player.PlayerSymbol);
            
            UpdateScoreText(player.Score);
            SetupBackgrounds();
            
            Debug.Log($"Flow 4:: PlayerUI:: Setup");
            player.ScoreUpdated.AddListener(UpdateScoreText);
            MatchController.TurnChanged.AddListener(SetupBackgrounds);
        }

        public void Clear()
        {
            if(player == null) return;
            player.ScoreUpdated.RemoveListener(UpdateScoreText);
            MatchController.TurnChanged.RemoveListener(SetupBackgrounds);
        }

        private void SetupBackgrounds(int arg0 = 0)
        {
            backgrounds.ForEach(background => background.color = player.IsActive ? activeColor : normalColor);
        }
        
        private void UpdateScoreText(int score)
        {
            Debug.Log($"Flow 5:: UpdatePlayerStatuses::");
            scoreText.text = score.ToString();
        }
    }
}
