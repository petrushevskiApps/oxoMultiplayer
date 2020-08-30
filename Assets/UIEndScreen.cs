using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEndScreen : MonoBehaviour
{
    [SerializeField] private GameObject wonText;
    [SerializeField] private GameObject lostText;
    [SerializeField] private GameObject background;
    
    private void Awake()
    {
        MatchController.OnMatchEnded.AddListener(MatchEnded);
        background.SetActive(false);
        wonText.SetActive(false);
        lostText.SetActive(false);
    }

    private void OnDestroy()
    {
        MatchController.OnMatchEnded.RemoveListener(MatchEnded);
    }

    private void MatchEnded(int winnerId)
    {
        background.SetActive(true);
        if (Player.LocalInstance.GetPlayerId() == winnerId)
        {
            ShowWonText();
        }
        else ShowLostText();
    }

    public void ShowWonText()
    {
        wonText.SetActive(true);
    }

    public void ShowLostText()
    {
        lostText.SetActive(true);
    }
}
