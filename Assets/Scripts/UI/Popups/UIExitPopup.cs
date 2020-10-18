using System;
using System.Collections;
using System.Collections.Generic;
using PetrushevskiApps.UIManager;
using UnityEngine;
using UnityEngine.UI;

public class UIExitPopup : UIPopup
{
   [SerializeField] private UIButton negativeButton;
   [SerializeField] private UIButton positiveButton;
   [SerializeField] private UIButton closeButton;
   
   private void Awake()
   {
      negativeButton.onClick.AddListener(OnBackButtonPressed);
      positiveButton.onClick.AddListener(UIManager.Instance.CloseApplication);
      closeButton.onClick.AddListener(OnBackButtonPressed);
   }
}
