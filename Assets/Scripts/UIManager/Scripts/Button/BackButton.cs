using System;
using UnityEngine;

namespace PetrushevskiApps.UIManager
{
    public class BackButton : MonoBehaviour
    {
        private UIButton backButton;
        
        private void Awake()
        {
            backButton = gameObject.GetComponent<UIButton>();

            if (backButton != null)
            {
                backButton.onClick.AddListener(()=>UIManager.Instance.OnBack());
            }
            else Debug.LogError("UIManager:: BackButton:: UIButton not found !!");
        }
    }
}

