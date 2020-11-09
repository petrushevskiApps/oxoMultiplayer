using System;
using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo.Utilities;
using PetrushevskiApps.UIManager;
using TMPro;
using UnityEngine;

public class UITimerPopup : UIPopup
{
    [SerializeField] private TextMeshProUGUI popupMessage;
    [SerializeField] private TextMeshProUGUI timerText;

    private Coroutine timer;
    private int timerSeconds = 30;

    private void OnPlayerEntered(NetworkPlayer player)
    {
        OnBackButtonPressed();
    }

    private void OnEnable()
    {
        timer = StartCoroutine(StartTimer());
        RoomController.PlayerEnteredRoom.AddListener(OnPlayerEntered);
    }

    private void OnDisable()
    {
        RoomController.PlayerEnteredRoom.RemoveListener(OnPlayerEntered);
        
        if (timer != null)
        {
            StopCoroutine(timer);
            timer = null;
        }
    }

    public void InitializePopup(string playerName)
    {
        string message = Constants.TIMER_POPUP_MESSAGE;
        popupMessage.text = message.Replace("{playerName}", playerName);
    }
    private void UpdateTimer()
    {
        timerText.text = timerSeconds.ToString();
    }

    private void TimerCompleted()
    {
        MatchController.LocalInstance.EndMatch(true);
    }
    
    private IEnumerator StartTimer()
    {
        while (timerSeconds > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            timerSeconds--;
            UpdateTimer();
        }
        
        TimerCompleted();
    }
}
