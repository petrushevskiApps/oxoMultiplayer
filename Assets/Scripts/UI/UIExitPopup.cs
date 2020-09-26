using System;
using System.Collections;
using System.Collections.Generic;
using PetrushevskiApps.UIManager;
using UnityEngine;
using UnityEngine.UI;

public class UIExitPopup : UIPopup
{
   [SerializeField] private Button negativeButton;
   [SerializeField] private Button positiveButton;

   private void Awake()
   {
      negativeButton.onClick.AddListener(OnBackButtonPressed);
      positiveButton.onClick.AddListener(UIManager.Instance.CloseApplication);
   }
}
