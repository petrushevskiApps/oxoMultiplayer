using System;
using com.petrushevskiapps.Oxo;
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
                ToggleInteractivity(NetworkManager.Instance.ConnectionController.IsOnline);
            }
            catch (NullReferenceException e)
            {
                ToggleInteractivity(false);
            }
            
            NetworkManager.Instance.ConnectionController.NetworkStatusChange.AddListener(ToggleInteractivity);
        }

        private void OnDisable()
        {
            NetworkManager.Instance.ConnectionController.NetworkStatusChange.RemoveListener(ToggleInteractivity);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ShowNoNetworkPopup();
        }
        private void ShowNoNetworkPopup()
        {
            if (!NetworkManager.Instance.ConnectionController.IsOnline)
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