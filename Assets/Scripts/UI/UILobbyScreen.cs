using System;
using System.Collections;
using System.Collections.Generic;
using PetrushevskiApps.UIManager;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyScreen : UIScreen
{
    [SerializeField] private Button startGame;

    private void Awake()
    {
        startGame.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        GameManager.Instance.NetworkManager.StartMatch();
    }
}
