using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyScreen : MonoBehaviour
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
