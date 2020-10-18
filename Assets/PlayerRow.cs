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

    public void SetToggle(bool isReady)
    {
        toggle.isOn = isReady;
    }
    
    public void SetPlayerName(string name)
    {
        playerName.text = name;
    }

    public void RegisterToggleEvent(UnityEvent<bool> playerEvent)
    {
        playerEvent.AddListener(SetToggle);
    }

    public void UnregisterEvent(UnityEvent<bool> playerEvent)
    {
        playerEvent.RemoveListener(SetToggle);
    }
}
