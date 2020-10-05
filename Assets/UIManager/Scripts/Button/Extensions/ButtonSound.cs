using UnityEngine;
using UnityEngine.EventSystems;

namespace PetrushevskiApps.UIManager
{
    public class ButtonSound : ButtonExtension, IPointerDownHandler
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] protected ButtonSoundConfig soundConfig;

        public void OnPointerDown(PointerEventData eventData)
        {
            if(audioSource == null) return;
            
            if (GetComponent<UIButton>().interactable)
            {
                audioSource.clip = soundConfig.positiveSound;
            }
            else
            {
                audioSource.clip = soundConfig.negativeSound;
            }
           
            audioSource.Play();
        }
    }
}