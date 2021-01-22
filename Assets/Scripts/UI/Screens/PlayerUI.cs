using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public partial class UIGameScreen
{
    [Serializable]
    public class PlayerUI
    {
        private IPlayer player;
        
        [SerializeField] private TextMeshProUGUI nicknameText;
        [SerializeField] private Image symbolImage;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private List<Image> backgrounds;
        
        private Color activeColor;
        private Color normalColor;
        
        public void Setup(IPlayer player, TileImages symbols,Color activeColor,Color normalColor)
        {
            this.player = player;
            this.activeColor = activeColor;
            this.normalColor = normalColor;
            
            nicknameText.text = player.GetNickname();
            symbolImage.sprite = symbols.GetEndTileState(player.GetSign());
            
            UpdateScoreText(player.GetScore());
            SetupBackgrounds();
            
            player.RegisterScoreListener(UpdateScoreText);
            MatchController.TurnChanged.AddListener(SetupBackgrounds);
        }

        public void Clear()
        {
            if(player == null) return;
            player.UnregisterScoreListener(UpdateScoreText);
            MatchController.TurnChanged.RemoveListener(SetupBackgrounds);
        }

        private void SetupBackgrounds(int arg0 = 0)
        {
            backgrounds.ForEach(background => background.color = player.IsActive() ? activeColor : normalColor);
        }
        
        private void UpdateScoreText(int score)
        {
            scoreText.text = score.ToString();
        }
    }
}