using System;
using System.Collections.Generic;
using PetrushevskiApps.UIManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIEndScreen : UIScreen
{
    [SerializeField] private MatchController matchController;
    
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button replayButton;
    
    [SerializeField] private GameObject wonText;
    [SerializeField] private GameObject lostText;
    [SerializeField] private GameObject background;
    [SerializeField] private Image image;
    [SerializeField] private List<Sprite> winImages;
    [SerializeField] private List<Sprite> loseImages;

    private void Awake()
    {
        base.Awake();
        BoardController.OnMatchEnded.AddListener(MatchEnded);
        exitBtn.onClick.AddListener(ExitRoom);
        settingsButton.onClick.AddListener(ShowSettings);
        replayButton.onClick.AddListener(RestartScene);
        background.SetActive(false);
        wonText.SetActive(false);
        lostText.SetActive(false);
    }

    private void RestartScene()
    {
        matchController.RestartScene();
    }

    private void ShowSettings()
    {
        UIManager.Instance.OpenPopup<UISettingsPopup>();
    }

    private void ExitRoom()
    {
        GameManager.Instance.NetworkManager.LeaveRoom();
    }

    private void OnDestroy()
    {
        BoardController.OnMatchEnded.RemoveListener(MatchEnded);
    }

    private void MatchEnded(int winnerId)
    {
        background.SetActive(true);
        bool isWin = Player.LocalInstance.GetPlayerId() == winnerId;
        ShowText(isWin);
        SetIcon(isWin);
    }

    private void SetIcon(bool isWin)
    {
        List<Sprite> sprites = isWin ? winImages : loseImages;
        int randIndex = Random.Range(0, sprites.Count - 1);
        image.sprite = sprites[randIndex];
    }
    private void ShowText(bool isWin)
    {
        wonText.SetActive(isWin);
        lostText.SetActive(!isWin);
    }

}
