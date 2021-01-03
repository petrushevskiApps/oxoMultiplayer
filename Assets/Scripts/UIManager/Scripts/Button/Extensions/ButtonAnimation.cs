using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PetrushevskiApps.UIManager
{
    public class ButtonAnimation : ButtonExtension, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private ButtonAnimationConfig animationConfig;

        private RectTransform rectTransform;
        private Vector3 defaultScale = Vector3.one;
        private Coroutine scaleDownCoroutine;

        protected void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            defaultScale = rectTransform.localScale;
        }
        protected void OnDisable()
        {
            ResetScale();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (scaleDownCoroutine == null)
            {
                scaleDownCoroutine = StartCoroutine(Scale(animationConfig.scale));
            }
        }
    
        public void OnPointerUp(PointerEventData eventData)
        {
            ResetScale();
        }

        private void ResetScale()
        {
            if (scaleDownCoroutine != null)
            {
                StopCoroutine(scaleDownCoroutine);
                scaleDownCoroutine = null;
            }
            rectTransform.localScale = defaultScale;
        }
    
        private IEnumerator Scale(Vector3 newScale)
        {
            Vector3 lScale = rectTransform.localScale;
        
            while (Vector3.Distance(lScale, newScale) >= 0.01f)
            {
                lScale = Vector3.Lerp(lScale, newScale, Time.deltaTime * animationConfig.scaleSpeed);
                rectTransform.localScale = lScale;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            // Smooth out the values of rect scale
            rectTransform.localScale = newScale;
        }

    }

}
