using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private Toggle toggle;

    private UnityEvent<bool> playerEvent;

    private NetworkPlayer player;

    public void SetupRow(NetworkPlayer player)
    {
        this.player = player;
        
        playerEvent = player.ReadyStatusChanged;
        playerName.text = player.Nickname;
        RegisterToggleEvent();
    }
    
    private void OnEnable()
    {
        SetToggle(player?.IsReady ?? false);
    }

    private void OnDestroy()
    {
        UnregisterEvent();
    }

    private void SetToggle(bool isReady)
    {
        if (toggle != null)
        {
            toggle.isOn = isReady;
        }
        
    }

    private void RegisterToggleEvent()
    {
        playerEvent.AddListener(SetToggle);
    }

    private void UnregisterEvent()
    {
        playerEvent?.RemoveListener(SetToggle);
    }
}
