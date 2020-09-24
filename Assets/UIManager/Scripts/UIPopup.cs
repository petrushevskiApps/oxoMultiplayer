using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.petrushevskiapps.menumanager
{
    public abstract class UIPopup : UIWindow
    {
        public static UnityEvent OnPopupShown = new UnityEvent();
        public static UnityEvent OnPopupHiden = new UnityEvent();
        public static UnityEvent OnPopupOpen = new UnityEvent();
        public static UnityEvent OnPopupClosed = new UnityEvent();

        public override void Show()
        {
            gameObject.SetActive(true);
            OnPopupShown.Invoke();
        }
        
        public override void Hide()
        {
            gameObject.SetActive(false);
            OnPopupHiden.Invoke();
        }

        public override void Open()
        {
            gameObject.SetActive(true);
            OnPopupOpen.Invoke();
        }
        public override void Close()
        {
            gameObject.SetActive(false);
            OnPopupClosed.Invoke();
        }
        
    }
}


