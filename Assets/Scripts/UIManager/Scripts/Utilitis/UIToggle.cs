using UnityEngine;
using UnityEngine.UI;

namespace com.petrushevskiapps.menumanager
{
    public class UIToggle : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private Sprite positiveImage;
        [SerializeField] private Sprite negativeImage;

        private bool status = false;
    
        public bool ToggleStatus
        {
            get => status;
            set
            {
                status = value;
                ToggleImage();
            }
        }
    
        private void ToggleImage()
        {
            if (ToggleStatus) image.sprite = positiveImage;
            else image.sprite = negativeImage;
        }
    }
}


