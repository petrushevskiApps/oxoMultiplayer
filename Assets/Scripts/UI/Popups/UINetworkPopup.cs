using System.Collections;
using System.Collections.Generic;
using com.petrushevskiapps.menumanager;
using PetrushevskiApps.UIManager;
using UnityEngine;
using UnityEngine.UI;

public class UINetworkPopup : UIPopup
{
    [SerializeField] private UIButton closeButton;
    
    private void Awake()
    {
        closeButton.onClick.AddListener(OnBackButtonPressed);
    }
}
