using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PetrushevskiApps.UIManager
{
    public class UIButton : Button
    {
        private bool isNetworkAvailable = true;
        private bool isInteractable = true;
        
        public void SetNetworkStatus(bool status)
        {
            isNetworkAvailable = status;
            SetInteractable();
        }

        public void SetInteractableStatus(bool status)
        {
            isInteractable = status;
            SetInteractable();
        }
        private void SetInteractable()
        {
            interactable = isInteractable && isNetworkAvailable;
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();
            onClick.RemoveAllListeners();
        }

    }
}