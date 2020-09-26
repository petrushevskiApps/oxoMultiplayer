using System;
using UnityEngine;

namespace PetrushevskiApps.UIManager
{
    public abstract class UIWindow : MonoBehaviour
    {
        private Action BackButtonAction;
        
        public void Initialize(Action onBackButtonAction)
        {
            BackButtonAction = onBackButtonAction;
        }
        
        protected virtual void OnBackButtonPressed()
        {
            BackButtonAction();
        }
       
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                OnBackButtonPressed();
            }
        }

        public abstract void Open();    
        public abstract void Show();
        public abstract void Hide();
        public abstract void Close();

    
    }
}


