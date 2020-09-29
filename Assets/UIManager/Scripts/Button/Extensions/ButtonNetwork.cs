using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PetrushevskiApps.UIManager
{
    public class ButtonNetwork : ButtonExtension, IPointerUpHandler
    {
        private void OnEnable()
        {
            try
            {
                ToggleInteractivity(GameManager.Instance.ConnectionController.IsOnline);
            }
            catch (NullReferenceException e)
            {
                ToggleInteractivity(false);
            }
        }

        private void Start()
        {
            GameManager.Instance.ConnectionController.OnNetworkStatusChange.AddListener(ToggleInteractivity);
        }

        private void OnDestroy()
        {
            GameManager.Instance.ConnectionController.OnNetworkStatusChange.RemoveListener(ToggleInteractivity);
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            ShowNoNetworkPopup();
        }
        private void ShowNoNetworkPopup()
        {
            if (GameManager.Instance.ConnectionController.IsOffline)
            {
                UIManager.Instance.OpenPopup<UINetworkPopup>();
            }
        }
        private void ToggleInteractivity(bool networkStatus)
        {
            GetComponent<UIButton>().interactable = networkStatus;
        }
    }
}