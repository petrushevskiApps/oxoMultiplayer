using UnityEngine;
using UnityEngine.EventSystems;

namespace PetrushevskiApps.UIManager
{
    public class ButtonSound : ButtonExtension, IPointerDownHandler
    {
        [SerializeField] protected ButtonSoundConfig soundConfig;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (GetComponent<UIButton>().interactable)
            {
                AudioManager.Instance.PlaySoundEffect(soundConfig.positiveSound);
            }
            else
            {
                AudioManager.Instance.PlaySoundEffect(soundConfig.negativeSound);
            }
        }
    }
}