using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private UIMenuController menuController;
    
    [SerializeField] private Button playRandomBtn;
    [SerializeField] private Button joinGameBtn;
    [SerializeField] private Button createGameBtn;
    
    private void Awake()
    {
        playRandomBtn.onClick.AddListener(GameManager.Instance.NetworkManager.JoinRandomRoom);
        joinGameBtn.onClick.AddListener(menuController.ShowJoinMenu);
        createGameBtn.onClick.AddListener(menuController.ShowCreateMenu);
    }

    private void OnDestroy()
    {
        playRandomBtn.onClick.RemoveListener(GameManager.Instance.NetworkManager.JoinRandomRoom);
        joinGameBtn.onClick.RemoveListener(menuController.ShowJoinMenu);
        createGameBtn.onClick.RemoveListener(menuController.ShowCreateMenu);
    }
}
