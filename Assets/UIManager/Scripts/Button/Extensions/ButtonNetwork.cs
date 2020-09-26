using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PetrushevskiApps.UIManager
{
    public class ButtonNetwork : ButtonExtension, IPointerUpHandler
    {
        public void OnPointerUp(PointerEventData eventData)
        {
            ShowNoNetworkPopup();
        }
        private void ShowNoNetworkPopup()
        {
            if (GameManager.Instance.NetworkChecker.IsOffline)
            {
                UIManager.Instance.OpenPopup<UINetworkPopup>();
            }
        }

        private void Start()
        {
            GameManager.Instance.NetworkChecker.OnNetworkStatusChange.AddListener(ToggleInteractivity);
        }

        private void OnDestroy()
        {
            GameManager.Instance.NetworkChecker.OnNetworkStatusChange.RemoveListener(ToggleInteractivity);
        }

        private void ToggleInteractivity(bool networkStatus)
        {
            GetComponent<UIButton>().interactable = networkStatus;
        }
    }
}