using System;
using UnityEngine.EventSystems;

namespace PetrushevskiApps.UIManager
{
    public class ButtonNetwork : ButtonExtension, IPointerUpHandler
    {
        private UIButton button;
        
        private void Awake()
        {
            button =  GetComponent<UIButton>();
        }

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
            
            GameManager.Instance.ConnectionController.OnNetworkStatusChange.AddListener(ToggleInteractivity);
        }

        private void OnDisable()
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
            button.SetNetworkStatus(networkStatus);
        }
    }
}