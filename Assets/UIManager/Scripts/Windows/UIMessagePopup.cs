﻿
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PetrushevskiApps.UIManager
{
    public class UIMessagePopup : UIPopup
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI message;
        [SerializeField] private Button closeButton;
    
        private void Awake()
        {
            closeButton.onClick.AddListener(OnBackButtonPressed);
        }

        public UIMessagePopup SetTitle(string title)
        {
            this.title.text = title;
            return this;
        }

        public UIMessagePopup SetMessage(string message)
        {
            this.message.text = message;
            return this;
        }
    }
}