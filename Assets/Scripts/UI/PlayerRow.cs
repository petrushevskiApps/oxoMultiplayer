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
    
    private void OnDestroy()
    {
        UnregisterEvent();
    }

    public void SetupRow(bool isReady, string playerName, UnityEvent<bool> playerEvent)
    {
        this.playerEvent = playerEvent;
        this.playerName.text = playerName;
        SetToggle(isReady);
        RegisterToggleEvent();
    }
    
    private void SetToggle(bool isReady)
    {
        toggle.isOn = isReady;
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
