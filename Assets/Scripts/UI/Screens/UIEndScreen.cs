﻿using System;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
using PetrushevskiApps.UIManager;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIEndScreen : UIScreen
{
    [Header("Buttons")]
    [SerializeField] private UIButton exitBtn;
    [SerializeField] private UIButton settingsButton;
    [SerializeField] private UIButton replayButton;
    
    [Header("Texts")]
    [SerializeField] private GameObject wonText;
    [SerializeField] private GameObject lostText;
    
    [Header("Images")]
    [SerializeField] private Image image;
    [SerializeField] private List<Sprite> winImages;
    [SerializeField] private List<Sprite> loseImages;

    [Header("Audio")]
    [SerializeField] private AudioClip winAudio;
    [SerializeField] private AudioClip loseAudio;

    [Header("VFX")]
    [SerializeField] private GameObject particles;
 
    public override void Initialize(Action onBackButtonAction)
    {
        base.Initialize(onBackButtonAction);
        
        BoardController.MatchEnded.AddListener(MatchEnded);
        exitBtn.onClick.AddListener(ExitRoom);
        settingsButton.onClick.AddListener(ShowSettings);
        replayButton.onClick.AddListener(RestartScene);

        wonText.SetActive(false);
        lostText.SetActive(false);
    }
    private void OnDestroy()
    {
        BoardController.MatchEnded.RemoveListener(MatchEnded);
    }
    
    private void RestartScene() => PhotonNetwork.LoadLevel(0);

    private void ShowSettings() => UIManager.Instance.OpenPopup<UISettingsPopup>();

    private void ExitRoom() => NetworkManager.Instance.LeaveRoom();

    private void MatchEnded(bool isWin)
    {
        particles.SetActive(isWin);
        ShowText(isWin);
        SetIcon(isWin);
        PlayAudio(isWin);
    }

    private void PlayAudio(bool isWin)
    {
        AudioManager.Instance.PlaySoundEffect(isWin ? winAudio : loseAudio);
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