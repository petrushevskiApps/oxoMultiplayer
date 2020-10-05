using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PetrushevskiApps.UIManager
{
    public class UIScreen : UIWindow
    {
        [SerializeField] private List<GameObject> globalUiElements;
        
        public static UnityEvent OnScreenShown = new UnityEvent();
        public static UnityEvent OnScreenHiden = new UnityEvent();
        public static UnityEvent OnScreenOpen = new UnityEvent();
        public static UnityEvent OnScreenClosed = new UnityEvent();

        private void ActivateGlobalUIElements()
        {
            globalUiElements.ForEach(x => x.SetActive(true));
        }
        private void DeactivateGlobalUIElements()
        {
            globalUiElements.ForEach(x => x.SetActive(false));
        }
        public override void Show()
        {
            ActivateGlobalUIElements();
            gameObject.SetActive(true);
            OnScreenShown.Invoke();
        }
        
        public override void Hide()
        {
            DeactivateGlobalUIElements();
            gameObject.SetActive(false);
            OnScreenHiden.Invoke();
        }

        public override void Open()
        {
            ActivateGlobalUIElements();
            gameObject.SetActive(true);
            
            OnScreenOpen.Invoke();
        }
        public override void Close()
        {
            gameObject.SetActive(false);
            DeactivateGlobalUIElements();
            OnScreenClosed.Invoke();
        }
    }

}

