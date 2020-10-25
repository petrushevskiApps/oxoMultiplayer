using System;
using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
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
    [SerializeField] private List<TextMeshProUGUI> playerScores;

    [Header("Middle Content")]
    [SerializeField] private TextMeshProUGUI roundNumberTextMid;
    [SerializeField] private GameObject middleContent;
    
    [Header("Players")]
    [SerializeField] private TextMeshProUGUI playerOneNickname;
    [SerializeField] private Image playerOneSymbol;
    [SerializeField] private TextMeshProUGUI playerTwoNickname;
    [SerializeField] private Image playerTwoSymbol;

    [Header("Backgrounds")] 
    [SerializeField] private Color activePlayerColor;
    [SerializeField] private Color normalPlayerColor;
    [SerializeField] private List<BackgroundsList> playerBackgrounds;

    [SerializeField] private TileImages symbols;
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
    
    private void OnMatchStarted()
    {
        RegisterScoreListeners();
        RegisterActiveListeners();
        
        List<NetworkPlayer> players = NetworkManager.Instance.RoomController.PlayersInRoom;
        
        playerOneNickname.text = players[0].Nickname;
        playerOneSymbol.sprite = symbols.GetEndTileState(players[0].PlayerSymbol);
        
        playerTwoNickname.text = players[1].Nickname;
        playerTwoSymbol.sprite = symbols.GetEndTileState(players[1].PlayerSymbol);
    }

    private void OnRoundStarted(int round)
    {
        string roundText = "Round " + MatchController.LocalInstance.Round;
        roundNumberTextTop.text = roundText;
        roundNumberTextMid.text = roundText;
        middleContent.SetActive(true);
        StartCoroutine(Delay(() => middleContent.SetActive(false)));
    }
  
    private void OnRoundEnded()
    {
        
    }

    private void OnMatchEnded(bool arg0)
    {
        UnregisterScoreListeners();
        UnregisterActiveListeners();
    }

    private void RegisterScoreListeners()
    {
        for (int i = 0; i < playerScores.Count; i++)
        {
            TextMeshProUGUI text = playerScores[i];
            text.text = "0";
            NetworkManager.Instance.RoomController.PlayersInRoom[i].PlayerScoreUpdated.AddListener((score) =>
            {
                text.text = score.ToString();
            });
        }
    }
    private void UnregisterScoreListeners()
    {
        for (int i = 0; i < playerScores.Count; i++)
        {
            NetworkManager.Instance.RoomController.PlayersInRoom[i].PlayerScoreUpdated.RemoveAllListeners();
        }
    }
    private void RegisterActiveListeners()
    {
        for (int i = 0; i < playerBackgrounds.Count; i++)
        {
            BackgroundsList backgrounds = playerBackgrounds[i];
            
            NetworkManager.Instance.RoomController.PlayersInRoom[i].PlayerActiveStatusChange.AddListener((isActive) =>
            {
                if (isActive)
                {
                    backgrounds.list.ForEach(background => background.color = activePlayerColor);
                }
                else
                {
                    backgrounds.list.ForEach(background => background.color = normalPlayerColor);
                }
            });
        }
    }

    private void UnregisterActiveListeners()
    {
        for (int i = 0; i < playerBackgrounds.Count; i++)
        {
            NetworkManager.Instance.RoomController.PlayersInRoom[i].PlayerActiveStatusChange.RemoveAllListeners();
        }
    }

    private IEnumerator Delay(Action delayedAction)
    {
        yield return new WaitForSeconds(1.5f);
        delayedAction.Invoke();
    }

    [Serializable]
    public class BackgroundsList
    {
        public List<Image> list;
    }
}
