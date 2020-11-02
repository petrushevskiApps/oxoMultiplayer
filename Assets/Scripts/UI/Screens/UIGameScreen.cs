using System;
using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using PetrushevskiApps.UIManager;
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
        settingsButton.onClick.AddListener(() => UIManager.Instance.OpenPopup<UISettingsPopup>());
        
        MatchController.MatchStarted.AddListener(OnMatchStarted);
        MatchController.MatchEnded.AddListener(OnMatchEnded);
        MatchController.RoundStarted.AddListener(OnRoundStarted);
        MatchController.RoundEnded.AddListener(OnRoundEnded);
        
    }

    private void OnDestroy()
    {
        MatchController.MatchStarted.RemoveListener(OnMatchStarted);
        MatchController.MatchEnded.RemoveListener(OnMatchEnded);
        MatchController.RoundStarted.RemoveListener(OnRoundStarted);
        MatchController.RoundEnded.RemoveListener(OnRoundEnded);
        
        Timer.Stop(this, "MidContent");
    }

    private void OnMatchStarted()
    {
        players = RoomController.Instance.PlayersInRoom;

        for (int i = 0; i < players.Count; i++)
        {
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
        for (int i = 0; i < players.Count; i++)
        {
            playerRefs[i].Clear();
        }
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
            SetupBackgrounds(player.IsActive);
            
            player.ScoreUpdated.AddListener(UpdateScoreText);
            player.ActiveStatusChanged.AddListener(SetupBackgrounds);
        }

        public void Clear()
        {
            player.ScoreUpdated.RemoveListener(UpdateScoreText);
            player.ActiveStatusChanged.AddListener(SetupBackgrounds);
        }
        
        private void SetupBackgrounds(bool isActive)
        {
            backgrounds.ForEach(background => background.color = isActive ? activeColor : normalColor);
        }
        
        private void UpdateScoreText(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}
