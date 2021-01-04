using System;
using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.Oxo;
using com.petrushevskiapps.Oxo.Utilities;
using PetrushevskiApps.UIManager;
using UnityEngine;

public class UILeavePopup : UIMessagePopup
{
    [SerializeField] private UIButton negativeButton;
    [SerializeField] private UIButton positiveButton;

    private Action OnPositiveButtonClicked;

    protected override void Awake()
    {
        base.Awake();
        negativeButton.onClick.AddListener(OnBackButtonPressed);
        positiveButton.onClick.AddListener(LeaveRoom);
    }

    private void OnDestroy()
    {
        negativeButton.onClick.RemoveListener(OnBackButtonPressed);
        positiveButton.onClick.RemoveListener(LeaveRoom);
    }

    public void SetPositiveAction(Action positiveAction)
    {
        OnPositiveButtonClicked = positiveAction;
    }
    
    private void LeaveRoom() => NetworkManager.Instance.LeaveRoom();
}
